using Common.Shared.Min.Extensions;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace WebApp.SoE.Helpers
{
	/// <summary>Parses markdown content (md-files) into a MarkupString which can be displayed in blazor</summary>
	public static class MarkdownHelper
	{
		public static MarkupString Parse(string? markdown)
		{
			if (markdown.IsNullOrEmpty()) return default;
			
			var pipeline = new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.Build();
			return (MarkupString)Markdown.ToHtml(markdown, pipeline);
		}
	}
}
