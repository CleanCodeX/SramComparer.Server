using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;

namespace WebApp.SoE.Extensions
{
	/// <summary>
	/// Various HTML extensions for strings
	/// </summary>
	public static partial class StringExtensions
	{
		/// <summary>Sets a fore color of a string</summary>
		public static string ColorText(this string source, Color color, bool formatAsHtml = true)
		{
			if (!formatAsHtml) return source;

			return $"<span style='color:{color.Name}'>{source}</span>";
		}

		/// <summary>Sets a background color of a string</summary>
		public static string BgColorText(this string source, Color color, bool formatAsHtml = true)
		{
			if (!formatAsHtml) return source;

			return $"<span style='background-color:{color.Name}'>{source}</span>";
		}

		/// <summary>Adds a tooltop to a string</summary>
		public static string AddTooltip(this string source, string text) => $"<span title='{text}'>{source}</span>";
		
		/// <summary>Replaces spaces by non breakable spaces</summary>
		public static MarkupString ReplaceSpaces(this string source) => source.Replace(' ', '\u00A0').ToMarkup();
		
		/// <summary>Creates a markup string</summary>
		public static MarkupString ToMarkup(this string source) => (MarkupString)source;

		public static string ReplaceHtmlLineBreaks([NotNull] this string source) => source
			.Replace("<br>", Environment.NewLine);

		public static string? ToNullIfEmpty(this string? source) => source.IsNullOrEmpty() ? null : source;

		public static MarkupString ReplaceWithHtmlLineBreaks(this string source, bool replace = true)
		{
			if (!replace) return source.ToMarkup();

			return source
				.Replace(Environment.NewLine, "<br>").ToMarkup();
		}

		/// <summary>Replaces quotes</summary>
		public static string RemoveQuotes(this string source) => source.Remove(@"""")!;

		public static bool StartsWith(this string source, string value, bool ignoreCase) => source.StartsWith(value, ignoreCase, null);

		public static string? RemoveSlashes(this string? source) => source.Remove("/");
		public static string? RemovePrefixSlash(this string? source) => source?.StartsWith("/") == true ? source[1..] : source;
	}
}
