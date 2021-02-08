using Microsoft.AspNetCore.Components;

namespace WebApp.SoE.Services
{
	public interface IMarkdownBuilder
	{
		MarkupString Parse(string? markdown);
	}
}