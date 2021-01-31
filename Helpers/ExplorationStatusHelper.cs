using System;
using System.Diagnostics;
using WebApp.SoE.Properties;
using ResComp = SRAM.Comparison.Properties.Resources;
using SaveSlot = SRAM.SoE.Models.SramSizes.SaveSlot;

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
			var knownPercentage = Math.Round(SaveSlot.KnownPercentage, 1, MidpointRounding.AwayFromZero);
			var unknownPercentage = Math.Round(SaveSlot.UnknownPercentage, 1, MidpointRounding.ToZero);

			var combinedPercentage = SaveSlot.KnownPercentage + SaveSlot.UnknownPercentage;
			var roundedPercentage = Math.Round(combinedPercentage, 1, MidpointRounding.ToZero);
			Debug.Assert(roundedPercentage.Equals(100));

			var unknown = Resources.LabelUnknown;
			var known = Resources.LabelKnown;
			
			return $"{Resources.MenuSramFormat} [{Resources.SaveSlot}]: ~{knownPercentage}% {known} ({knownBytes} {ResComp.Bytes}) & ~{unknownPercentage}% {unknown} ({unknownBytes} {ResComp.Bytes}) {est} 100% ({allBytes} {ResComp.Bytes})";
		}
	}
}