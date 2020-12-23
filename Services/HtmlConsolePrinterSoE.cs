using System;
using WebApp.SoE.Extensions;
using SramComparer.SoE.Services;

namespace WebApp.SoE.Services
{
	/// <summary>
	/// HTML formatted ConsolePrinter for SoE
	/// </summary>
	public class HtmlConsolePrinterSoE : ConsolePrinterSoE
	{
		public override void PrintColored(ConsoleColor color, string text) =>
			base.Print(text.ColorText(color.ToColor()));

		public override void PrintColoredLine(ConsoleColor color, string text) =>
			base.PrintLine(text.ColorText(color.ToColor()));

		protected override void PrintBackgroundColored(ConsoleColor color, string text) =>
			base.Print(text.BgColorText(color.ToColor()));

		protected override void PrintBackgroundColoredLine(ConsoleColor color, string text) =>
			base.PrintLine(text.BgColorText(color.ToColor()));

		protected override void PrintColored(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string text) => base.Print(text.ColorText(foregroundColor.ToColor())
			.BgColorText(backgroundColor.ToColor()));

		protected override void PrintColoredLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string text) =>
			base.PrintLine(text.ColorText(foregroundColor.ToColor())
				.BgColorText(backgroundColor.ToColor()));
	}
}
