using IO.Extensions;
using WebApp.SoE.Properties.Hidden;

namespace WebApp.SoE.Helpers
{
	public static class CrypticTooltipRandomizer
	{
		public static string NextTooltip() => TooltipRandomizer.Random.NextInclusive(1, 5) == 1
			? Resources.LabelTooltipCrypticSymbols /* 20% chance */
			: TooltipRandomizer.NextTooltip();
	}
}