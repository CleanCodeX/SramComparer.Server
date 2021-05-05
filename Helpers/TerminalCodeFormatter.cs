using System.Drawing;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using SoE.Models.Enums;
using SoE.Models.Structs;
using SoE.Properties;
using WebApp.SoE.Extensions;

namespace WebApp.SoE.Helpers
{
	public class TerminalCodeFormatter
	{
		public const int EmptySaveSlotValue = 24672;

		public static MarkupString FormatTerminalCode(TerminalCode code, bool formatAsHtml, Color overridingColor = default)
		{
			if (!formatAsHtml) return code.ToString().ToMarkup();

			if (code.IsDefault)
				return Resources.StatusNoTerminalCodeSet.ColorText(Color.Cyan).ToMarkup();

			if (!code.IsValid)
				return $"{Resources.StatusInvalidTerminalCode} ({code.Code1}-{code.Code2}-{code.Code3})".ColorText(Color.Red).ToMarkup();
		
			if (code.Code1.ToUShort() == EmptySaveSlotValue)
				overridingColor = Color.Red;

			string result = FormatCodeColor(code.Code1, overridingColor);
			result += "-" + FormatCodeColor(code.Code2, overridingColor);
			result += "-" + FormatCodeColor(code.Code3, overridingColor);

			result += " | " + FormatCodeNumber(code.Code1, overridingColor);
			result += "-" + FormatCodeNumber(code.Code2, overridingColor);
			result += "-" + FormatCodeNumber(code.Code3, overridingColor);

			return result.ToMarkup();

			static string FormatCodeColor(TerminalCodeColor color, Color overridingColor) =>
				color.ToString().ColorText(overridingColor != default ? overridingColor : GetTerminalCodeColor(color));

			static string FormatCodeNumber(TerminalCodeColor color, Color overridingColor) =>
				color.ToUShort().ToString().ColorText(overridingColor !=  default ? overridingColor : GetTerminalCodeColor(color));

			static Color GetTerminalCodeColor(TerminalCodeColor color) =>
				color switch
				{
					TerminalCodeColor.Blue => Color.LightBlue,
					TerminalCodeColor.Green => Color.Green,
					_ => Color.Red,
				};
		}
	}
}