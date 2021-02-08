using System;

namespace WebApp.SoE.Services
{
	public class RandomResTooltip : IRandomResTooltip
	{
		private const int RandomizedOneOutOfChance = 20;

		private static readonly Random Random = new();

		private ITooltipRandomizer TooltipRandomizer { get; }

		public RandomResTooltip(ITooltipRandomizer tooltipRandomizer) => TooltipRandomizer = tooltipRandomizer;

		public string this[string resText, int randomizedOneOutOfChance = 0, int tooltipIndex = -1]
		{
			get
			{
				if (randomizedOneOutOfChance == 0) randomizedOneOutOfChance = RandomizedOneOutOfChance;

				try
				{
					if (Random.Next(1, randomizedOneOutOfChance) == 1)
						return tooltipIndex > -1
							? TooltipRandomizer.GetTooltip(tooltipIndex)
							: TooltipRandomizer.NextMenuTooltip();

					return resText;
				}
				catch (Exception ex)
				{
					return "Error: " + ex.Message;
				}
			}
		}
	}
}
