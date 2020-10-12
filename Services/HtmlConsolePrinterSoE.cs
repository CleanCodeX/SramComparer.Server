using System;
using SramComparer.Server.Extensions;
using SramComparer.SoE.Services;

namespace SramComparer.Server.Services
{
	public class HtmlConsolePrinterSoE : ConsolePrinterSoE
	{
		public override void PrintColored(ConsoleColor color, object text) =>
			base.Print(text.ToString()
				.ColorText(color.ToColor()));

		public override void PrintColoredLine(ConsoleColor color, object text) =>
			base.PrintLine(text.ToString()
				.ColorText(color.ToColor()));

		protected override void PrintBackgroundColored(ConsoleColor color, object text) =>
			base.Print(text.ToString()
				.BgColorText(color.ToColor()));

		protected override void PrintBackgroundColoredLine(ConsoleColor color, object text) =>
			base.PrintLine(text.ToString()
				.BgColorText(color.ToColor()));

		protected override void PrintColored(ConsoleColor foregroundColor, ConsoleColor backgroundColor, object text) => base.Print(text.ToString()
			.ColorText(foregroundColor.ToColor())
			.BgColorText(backgroundColor.ToColor()));

		protected override void PrintColoredLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, object text) =>
			base.PrintLine(text.ToString()
				.ColorText(foregroundColor.ToColor())
				.BgColorText(backgroundColor.ToColor()));
	}
}
