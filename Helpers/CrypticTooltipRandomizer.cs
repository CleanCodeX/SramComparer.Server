using RosettaStone.Sram.SoE.Extensions;
using WebApp.SoE.Properties.Hidden;

namespace WebApp.SoE.Helpers
{
	public static class CrypticTooltipRandomizer
	{
		public static string GetTooltip() => TooltipRandomizer.Random.NextInclusive(1, 5) == 1
			? Resources.LabelTooltipCrypticSymbols /* 20% chance */
			: TooltipRandomizer.NextTooltip();
	}
}