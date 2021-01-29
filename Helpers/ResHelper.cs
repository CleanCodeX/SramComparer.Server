using System;

namespace WebApp.SoE.Helpers
{
	public static class ResHelper
	{
		private const int RandomizedWithinMaxDefault = 20;

		private static readonly Random Random = new();

		public static string GetRes(string resText, int randomizedWithinMax = RandomizedWithinMaxDefault, int tooltip = -1)
		{
			try
			{
				if (Random.Next(1, randomizedWithinMax) == 1)
					return tooltip > -1 
						? TooltipRandomizer.GetTooltip(tooltip) 
						: TooltipRandomizer.NextMenuTooltip();

				return resText;
			}
			catch (Exception ex)
			{
				return "Upsi pupsi: " + ex.Message;
			}
		}
	}
}
