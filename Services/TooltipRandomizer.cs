using System;
using System.Collections.Generic;
using System.IO;

namespace WebApp.SoE.Services
{
	public class TooltipRandomizer : ITooltipRandomizer
	{
		private const string TooltipFile = "wwwroot/Phrases.txt";
		
		internal  static readonly Random Random = new();

		private static readonly List<string> Tooltips = new();
		private static readonly object LockObj;
		private static int ListCount;

		[ThreadStatic]
		private static DateTimeOffset lastLockedAt;
		[ThreadStatic]
		private static int lastLockedIndex;

		public TooltipRandomizerOptions Options { get; }

		static TooltipRandomizer()
		{
			LockObj ??= new();
			Initialize();
		}

		public TooltipRandomizer(TooltipRandomizerOptions options) => Options = options;

		public virtual string NextTooltip(bool allowFreeze = true) => GetTooltip(Random.Next(ListCount), allowFreeze);
		public virtual string NextMenuTooltip(bool allowFreeze = true) => GetTooltip(Random.Next(FindIndex(string.Empty)), allowFreeze);
		public virtual string GetTooltip(int index, bool allowFreeze = true) => InternalGetTooltip(index, allowFreeze);

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

		private string InternalGetTooltip(int index, bool allowFreeze)
		{
			if (ListCount == 0) return string.Empty;

			allowFreeze = allowFreeze && Options.FreezeTooltipOneOutOfChance > 0 && Options.FreezeTooltipBeginMarker is not null;

			if (Random.Next(Options.TooltipOneOutOfChance) > 0) // chance to see a tooltip at all
				return string.Empty;

			if (index >= ListCount)
				index = 0;
			
			try
			{
				if (lastLockedIndex > 0)
				{
					if ((DateTimeOffset.Now - lastLockedAt).TotalSeconds <= Options.FreezeTooltipWaitTimeInSeconds)
						return Template(GetFromIndex(lastLockedIndex - 1));

					lastLockedAt = default;
					lastLockedIndex = 0;
				}

				var tooltip = GetFromIndex(index);
				if (allowFreeze && lastLockedIndex == 0 && tooltip.StartsWith(Options.FreezeTooltipBeginMarker!))
				{
					if (Random.Next(Options.FreezeTooltipOneOutOfChance / Options.TooltipOneOutOfChance) == 0) // lower these chances
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
