using System.Drawing;
using System.IO;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using SoE.Models.Enums;
using SRAM.SoE.Models;
using WebApp.SoE.Extensions;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Helpers
{
	public static class SaveslotChecksumStatusFormatter
	{
		public static MarkupString GetSaveslotChecksumStatus(int slotIndex, Stream stream, GameRegion region) =>
			GetSaveslotChecksumStatus(slotIndex, new(stream, region));
		public static MarkupString GetSaveslotChecksumStatus(int slotIndex, SramFileSoE sramFile)
		{
			sramFile.ThrowIfNull(nameof(sramFile));

			if (sramFile.IsEmptySlot(slotIndex))
				return Resources.EmptySaveSlot.ColorText(Color.Gray).ToMarkup();

			var isValid = sramFile.IsValid(slotIndex);

			return FormatIsValid(isValid).ColorText(isValid ? Color.Green : Color.Red).ToMarkup();
		}

		private static string FormatIsValid(bool isValid) => isValid ? SRAM.Comparison.SoE.Properties.Resources.Valid : SRAM.Comparison.SoE.Properties.Resources.Invalid;
	}
}