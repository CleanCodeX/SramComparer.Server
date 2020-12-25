using System;
using SramFormat.SoE.Constants;
using WebApp.SoE.Properties;
using Res = SramComparer.Properties.Resources;

namespace WebApp.SoE.Shared
{
	public partial class CompletionDisplay
	{
		public string? CompletionStatus { get; private set; }

		protected override void OnInitialized()
		{
			var est = '≙';
			var knownBytes = Sizes.Game.AllKnown;
			var allBytes = Sizes.Game.All;
			var unknownBytes = Sizes.Game.AllUnknown;
			var knownPercentage = Math.Round((double)knownBytes / allBytes * 100, 1);
			var unknownPercentage = Math.Round((double)unknownBytes / allBytes * 100, 1);
			var unknown = Resources.UnknownBytes;
			var known = Resources.KnownBytes;

			CompletionStatus = $"{knownPercentage}% ({knownBytes}) {known} + {unknownPercentage}% ({unknownBytes}) {unknown} {est} 100% ({allBytes} {Res.Bytes})";
		}
	}
}
