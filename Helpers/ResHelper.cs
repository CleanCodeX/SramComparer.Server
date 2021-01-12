using System;
using System.Resources;
using RosettaStone.Sram.SoE.Extensions;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Helpers
{
	public static class ResHelper
	{
		private const int RandomizedWithinMaxDefault = 20;

		private static readonly ResourceManager ResourceManager = new(typeof(Resources));
		private static readonly Random Random = new();

		public static string GetRes(string resKey, int randomizedWithinMax = RandomizedWithinMaxDefault, int tooltip = -1)
		{
			if (Random.NextInclusive(1, randomizedWithinMax) == 1)
			{
				if (tooltip > -1)
					return TooltipRandomizer.GetTooltip(tooltip);

				return TooltipRandomizer.NextMenuTooltip();
			}

			return ResourceManager.GetString(resKey)!;
		}
	}
}
