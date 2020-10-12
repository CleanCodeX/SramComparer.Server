using System.Drawing;

namespace SramComparer.Server.Extensions
{
	public static class StringExtensions
	{
		public static string ColorText(this string? source, Color color) => $"<span style='color:{color.Name}'>{source}</span>";
		public static string BgColorText(this string? source, Color color) => $"<span style='background-color:{color.Name}'>{source}</span>";

		public static string Title(this string source, string title) => $"<span title='{title}'>{source}</span>";
	}
}
