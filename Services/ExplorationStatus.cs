using System;
using System.Diagnostics;
using System.Text;
using WebApp.SoE.Properties;
using ResComp = SRAM.Comparison.Properties.Resources;
using Rom = ROM.SoE.Models.RomSizes;
using Sram = SRAM.SoE.Models.SramSizes;
using Wram = WRAM.Snes9x.SoE.Models.WramSizes;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace WebApp.SoE.Services
{
	public class ExplorationStatus : IExplorationStatus
	{
		private const char Est = '≙';
		private const char Nbsp = '\u00A0';
		private static readonly string Unknown = Resources.LabelUnknown;
		private static readonly string Known = Resources.LabelKnown;
		private static readonly string Bytes = ResComp.Bytes;
		private static readonly string NewLine = Environment.NewLine;

		public static bool ShowWramStatus = Wram.AllKnown > 0;
		public static bool ShowRomStatus = Rom.AllKnown > 0;
		public static bool UsePadding = false;

		public string GetStatus() => GetStatus(ShowWramStatus, ShowRomStatus);
		public string GetStatus(bool showWramStatus, bool showRomStatus)
		{
			var padding = !UsePadding ? 0 : showWramStatus ? 9 : showRomStatus ? 7 : 3;

			StringBuilder sb = new(GetSramStatus(padding));
			
			if (showWramStatus)
				sb.Append(NewLine + GetWramStatus(padding));

			if (showRomStatus)
				sb.Append(NewLine + GetRomStatus(padding));

			return sb.ToString().Replace(' ', Nbsp);
		}

		private const int Padding = 5;
		public static string GetSramStatus(int padding = 0) => GetStatus(typeof(Sram), "S-RAM".PadLeft(Padding, Nbsp), padding);
		public static string GetWramStatus(int padding = 0) => GetStatus(typeof(Wram), "W-RAM".PadLeft(Padding, Nbsp), padding);
		public static string GetRomStatus(int padding = 0) => GetStatus(typeof(Rom), "ROM".PadLeft(Padding, Nbsp), padding);

		private static string GetStatus(Type sizeType, string name, int padding = 0)
		{
			var all = (int)sizeType.GetField("All")!.GetRawConstantValue()!;
			var allKnown = (int)sizeType.GetField("AllKnown")!.GetRawConstantValue()!;
			var allUnknown = (int)sizeType.GetField("AllUnknown")!.GetRawConstantValue()!;
			var knownPercentage = (double)sizeType.GetField("KnownPercentage")!.GetRawConstantValue()!;
			var unknownPercentage = (double)sizeType.GetField("UnknownPercentage")!.GetRawConstantValue()!;

			var pecentagePadding = UsePadding ? 4 : 0;
			var knownRounded = Math.Round(knownPercentage, 1, MidpointRounding.AwayFromZero).ToString().PadLeft(pecentagePadding);
			var unknownRounded = Math.Round(unknownPercentage, 1, MidpointRounding.ToZero).ToString().PadLeft(pecentagePadding);

			var knownString = $"{allKnown:0,0;0;0}".PadRight(padding);
			var unknownString = $"{allUnknown:0,0}".PadRight(padding);
			var allString = $"{all:0,0}".PadRight(padding);
			
			#region Debug

			var combinedPercentage = knownPercentage + unknownPercentage;
			var roundedPercentage = Math.Round(combinedPercentage, 1, MidpointRounding.ToZero);
			Debug.Assert(roundedPercentage.Equals(100));

			#endregion

			return $"{name} :: {Known}: {knownString} (~{knownRounded}%) {Est} {Unknown}: {unknownString} (~{unknownRounded}%) {Est} {allString} {Bytes} (100%)";
		}
	}
}