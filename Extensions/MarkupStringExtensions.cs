using Microsoft.AspNetCore.Components;

namespace WebApp.SoE.Extensions
{
	/// <summary>
	/// Extensions for MarkupString
	/// </summary>
	public static class MarkupStringExtensions
	{
		public static string ReplaceHtmlLineBreaks(this MarkupString source) => source.ToString().ReplaceHtmlLineBreaks();

		public static MarkupString Replace(this MarkupString source, string oldValue, string newValue) => source.ToString().Replace(oldValue, newValue).ToMarkup();
	}
}
