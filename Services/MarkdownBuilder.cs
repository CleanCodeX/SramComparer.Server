using Common.Shared.Min.Extensions;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Microsoft.AspNetCore.Components;

namespace WebApp.SoE.Services
{
	/// <summary>Parses markdown content (md-files) into a MarkupString which can be displayed in blazor</summary>
	public class MarkdownBuilder : IMarkdownBuilder
	{
		public MarkupString Parse(string? markdown)
		{
			if (markdown.IsNullOrEmpty()) return default;
			
			var pipeline = new MarkdownPipelineBuilder()
				.UseEmojiAndSmiley()
				.UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
				.UseAutoLinks()
				.Build();
			return (MarkupString)Markdown.ToHtml(markdown, pipeline);
		}
	}
}
