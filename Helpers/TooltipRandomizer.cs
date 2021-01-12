using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApp.SoE.Helpers
{
	public static class TooltipRandomizer
	{
		private const string TooltipFile = "wwwroot/Tooltips.txt";
		private const int IHaveSpokenWaitTimeInSeconds = 300;

		private static readonly Random Random = new();
		private static readonly List<string> Tooltips = new();
		private static DateTimeOffset lastLockedAt;
		private static int lastLockedIndex;
		

		public static string NextTooltip() => GetTooltip(Random.Next(Tooltips.Count));
		public static string NextMenuTooltip() => GetTooltip(Random.Next(Tooltips.IndexOf(string.Empty)));

		static TooltipRandomizer()
		{
			if (File.Exists(TooltipFile))
				Tooltips = File.ReadAllLines(TooltipFile).ToList();
		}

		public static string GetTooltip(int index)
		{
			if ((DateTimeOffset.Now - lastLockedAt).TotalSeconds <= IHaveSpokenWaitTimeInSeconds)
				index = lastLockedIndex;
			else
				lastLockedAt = default;

			if (index >= Tooltips.Count)
				index = 0;

			var tooltip = Tooltips[index];
			if (tooltip.Contains("I have spoken."))
			{
				lastLockedIndex = index;
				lastLockedAt = DateTimeOffset.Now;
			}

			return  $"»{tooltip}«";
		}
	}
}
