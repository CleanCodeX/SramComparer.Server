﻿using System;
using System.Collections.Generic;
using System.IO;

namespace WebApp.SoE.Helpers
{
	public static class TooltipRandomizer 
	{
		private const string TooltipFile = "wwwroot/Tooltips.txt";
		private const int IHaveSpokenWaitTimeInSeconds = 300;

		private static readonly Random Random = new();
		private static List<string> Tooltips = new();
		private static object LockObj;
		private static int ListCount;

		[ThreadStatic]
		private static DateTimeOffset lastLockedAt;
		[ThreadStatic]
		private static int lastLockedIndex = -1;

		static TooltipRandomizer()
		{
			LockObj ??= new();
			Initialize();
		}

		public static string NextTooltip() => GetTooltip(Random.Next(ListCount));
		public static string NextMenuTooltip() => GetTooltip(Random.Next(FindIndex(string.Empty)));
		public static string GetTooltip(int index) => InternalGetTooltip(index);

		private static void Initialize()
		{
			if (ListCount != 0) return;

			lock (LockObj)
			{
				if (ListCount > 0) return;

				try
				{
					if (File.Exists(TooltipFile))
					{
						var lines = File.ReadAllLines(TooltipFile);
						if (ListCount > 0) return;

						Tooltips.AddRange(lines);
						ListCount = lines.Length;
					}
				}
				catch
				{
				}
			}
		}

		private static string InternalGetTooltip(int index)
		{
			if (ListCount == 0) return string.Empty;

			if (index >= ListCount)
				index = 0;

			try
			{
				if (lastLockedIndex > -1)
				{
					if ((DateTimeOffset.Now - lastLockedAt).TotalSeconds <= IHaveSpokenWaitTimeInSeconds)
						return Template(GetFromIndex(lastLockedIndex));
					else
					{
						lastLockedAt = default;
						lastLockedIndex = -1;
					}
				}

				var tooltip = GetFromIndex(index);
				if (lastLockedIndex == -1 && tooltip.StartsWith("I have spoken."))
				{
					lastLockedIndex = index;
					lastLockedAt = DateTimeOffset.Now;
				}

				return Template(tooltip);
			}
			catch (Exception ex)
			{
				return "Upsi pupsi: " + ex.Message;
			}
		}

		private static string GetFromIndex(int index) => Tooltips[index];
		private static int FindIndex(string text) => Tooltips.IndexOf(text);
		private static string Template(string text) => $"»{text}«";
	}
}
