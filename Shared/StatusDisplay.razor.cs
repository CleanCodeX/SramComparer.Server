using System;
using RosettaStone.Sram.SoE.Constants;
using WebApp.SoE.Properties;
using Res = SramComparer.Properties.Resources;

namespace WebApp.SoE.Shared
{
	public partial class StatusDisplay
	{
		public string? Status { get; private set; }

		protected override void OnInitialized()
		{
			var est = '≙';
			var knownBytes = Sizes.SaveSlot.AllKnown;
			var allBytes = Sizes.SaveSlot.All;
			var unknownBytes = Sizes.SaveSlot.AllUnknown;
			var knownPercentage = Math.Round((double)knownBytes / allBytes * 100, 1);
			var unknownPercentage = Math.Round((double)unknownBytes / allBytes * 100, 1);
			var unknown = Resources.LabelUnknownBytes;
			var known = Resources.LabelKnownBytes;

			Status = $"{knownPercentage}% ({knownBytes}) {known} + {unknownPercentage}% ({unknownBytes}) {unknown} {est} 100% ({allBytes} {Res.Bytes})";
		}
	}
}
