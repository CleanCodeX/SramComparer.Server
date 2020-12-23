using System;
using System.Globalization;
using SramFormat.SoE.Constants;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared
{
	public partial class CompletionDisplay
	{
		public string? CompletionStatus { get; private set; }

		protected override void OnInitialized()
		{
			var knownBytes = Sizes.Game.AllKnown;
			var allBytes = Sizes.Game.All;
			var unknownBytes = Sizes.Game.AllUnknown;
			var knownPercentage = Math.Round((double)knownBytes / allBytes * 100, 1);
			var unknownPercentage = Math.Round((double)unknownBytes / allBytes * 100, 1);
			var unknown = Resources.UnknownBytes;
			var known = Resources.KnownBytes;
			var status = Resources.ProjectStatus;

			CompletionStatus = $"{status}: {known} {knownBytes} ({knownPercentage}%) vs. {unknown} {unknownBytes} ({unknownPercentage}%)";
		}
	}
}
