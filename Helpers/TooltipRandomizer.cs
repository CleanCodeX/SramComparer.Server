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

		[ThreadStatic]
		private static readonly Random Random = new();
		[ThreadStatic]
		private static List<string> Tooltips = new();
		[ThreadStatic]
		private static DateTimeOffset lastLockedAt;
		[ThreadStatic]
		private static int lastLockedIndex;
		[ThreadStatic]
		private static object LockObj = new();

		public static string NextTooltip()
		{
			lock (LockObj)
			{
				Initialize();
				return GetTooltip(Random.Next(Tooltips.Count));
			}
		}

		public static string NextMenuTooltip()
		{
			lock (LockObj)
			{
				Initialize();
				return GetTooltip(Random.Next(Tooltips.IndexOf(string.Empty)));
			}
		}

		public static string GetTooltip(int index)
		{
			Initialize();
			return InternalGetTooltip(index);
		}

		private static int Initialize()
		{
			if (Tooltips.Count == 0)
			{
				lock (LockObj)
				{
					if (Tooltips.Count > 0) return Tooltips.Count;

					try
					{
						if (File.Exists(TooltipFile))
							Tooltips = File.ReadAllLines(TooltipFile).ToList();
					}
					catch
					{
					}
				}
			}

			return Tooltips.Count;
		}

		private static string InternalGetTooltip(int index)
		{
			try
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

				return $"»{tooltip}«";
			}
			catch (Exception ex)
			{
				return "Upsi pupsi: " + ex.Message;
			}
		}
	}
}
