using System;
using System.Collections.Generic;
using System.IO;

namespace WebApp.SoE.Helpers
{
	public static class TooltipRandomizer 
	{
		private const string TooltipFile = "wwwroot/Phrases.txt";
		private const int IHaveSpokenWaitTimeInSeconds = 300;
		private const int IHaveSpokenStateChanceMaxValue = 10;

		internal  static readonly Random Random = new();

		private static readonly List<string> Tooltips = new();
		private static readonly object LockObj;
		private static int ListCount;

		[ThreadStatic]
		private static DateTimeOffset lastLockedAt;
		[ThreadStatic]
		private static int lastLockedIndex;

		static TooltipRandomizer()
		{
			LockObj ??= new();
			Initialize();
		}

		public static string NextTooltip(bool allowFreeze = true) => GetTooltip(Random.Next(ListCount), allowFreeze);
		public static string NextMenuTooltip(bool allowFreeze = true) => GetTooltip(Random.Next(FindIndex(string.Empty)), allowFreeze);
		public static string GetTooltip(int index, bool allowFreeze = true) => InternalGetTooltip(index, allowFreeze);

		private static void Initialize()
		{
			if (ListCount != 0) return;

			lock (LockObj)
			{
				if (ListCount > 0) return;

				try
				{
					if (!File.Exists(TooltipFile)) return;

					var lines = File.ReadAllLines(TooltipFile);
					if (ListCount > 0) return;

					Tooltips.AddRange(lines);
					ListCount = lines.Length;
				}
				catch
				{
					// We don't care
				}
			}
		}

		private static string InternalGetTooltip(int index, bool allowFreeze)
		{
			if (ListCount == 0) return string.Empty;

			if (index >= ListCount)
				index = 0;

			try
			{
				if (lastLockedIndex > 0)
				{
					if ((DateTimeOffset.Now - lastLockedAt).TotalSeconds <= IHaveSpokenWaitTimeInSeconds)
						return Template(GetFromIndex(lastLockedIndex - 1));

					lastLockedAt = default;
					lastLockedIndex = 0;
				}

				var tooltip = GetFromIndex(index);
				if (allowFreeze && lastLockedIndex == 0 && tooltip.StartsWith("I have spoken."))
				{
					if (Random.Next(IHaveSpokenStateChanceMaxValue) == 1) // lower these chances
					{
						lastLockedIndex = index + 1;
						lastLockedAt = DateTimeOffset.Now;
					}
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
		private static string Template(string text) => text != string.Empty ? $"»{text}«" : text;
	}
}
