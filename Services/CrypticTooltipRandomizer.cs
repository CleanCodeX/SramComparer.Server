using WebApp.SoE.Properties.Hidden;

namespace WebApp.SoE.Services
{
	public class CrypticTooltipRandomizer : TooltipRandomizer
	{
		public CrypticTooltipRandomizer(TooltipRandomizerOptions options) : base(options)
		{ }

		public override string NextTooltip(bool allowFreeze = true) => Random.Next(0, 5) == 0
			? Resources.LabelTooltipCrypticSymbols /* 20% chance */
			: base.NextTooltip(allowFreeze);
	}
}