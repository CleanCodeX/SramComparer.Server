using Markdig;
using Microsoft.AspNetCore.Components;

namespace SramComparer.SoE.Server.Helpers
{
	public static class MarkdownHelper
	{
		public static MarkupString Parse(string markdown)
		{
			var pipeline = new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.Build();
			return (MarkupString)Markdown.ToHtml(markdown, pipeline);
		}
	}
}
