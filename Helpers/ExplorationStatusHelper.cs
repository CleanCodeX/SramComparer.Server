using System;
using WebApp.SoE.Properties;
using ResComp = SramComparer.Properties.Resources;
using SaveSlot = RosettaStone.Sram.SoE.Models.SramSizes.SaveSlot;

namespace WebApp.SoE.Helpers
{
	public class ExplorationStatusHelper
	{
		public static string GetExplorationStatus()
		{
			const char est = '≙';
			const int knownBytes = SaveSlot.AllKnowns;
			const int allBytes = SaveSlot.All;
			const int unknownBytes = SaveSlot.AllUnknown;
			var knownPercentage = Math.Round(SaveSlot.KnownPercentage, 1);
			var unknownPercentage = Math.Round(SaveSlot.UnknownPercentage, 1);
			var unknown = Resources.LabelUnknownBytes;
			var known = Resources.LabelKnownBytes;

			return $"{knownPercentage}% ({knownBytes}) {known} + {unknownPercentage}% ({unknownBytes}) {unknown} {est} 100% ({allBytes} {ResComp.Bytes})";
		}
	}
}