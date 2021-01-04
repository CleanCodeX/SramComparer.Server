using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class MarkupContentBase : ComponentBase
	{
#nullable disable
		[Inject] private IHttpClientFactory ClientFactory { get; set; }
#nullable restore

		protected MarkupString Content { get; set; }

		protected virtual async Task<bool> SetContentFromFileAsync(string filePath)
		{
			Content = await ParseContentAsync(LoadFromFile(filePath));
			return Content.Value != string.Empty;
		}

		protected virtual async Task<bool> SetContentFromUrlAsync(string url)
		{
			Content = await ParseContentAsync(await LoadFromUrlAsync(url));
			return Content.Value != string.Empty;
		}

		protected virtual Task<MarkupString> ParseContentAsync(string? content) => Task.FromResult(MarkdownHelper.Parse(content));

		protected async Task<string?> LoadFromUrlAsync(string url, bool allowFailure = false)
		{
			try
			{
				var http = ClientFactory.CreateClient();
				var response = await http.GetAsync(url);
				var result = response.IsSuccessStatusCode ?
					await response.Content.ReadAsStringAsync() : response.ReasonPhrase!;

				return result;
			}
			catch (Exception ex)
			{
				return allowFailure ? null : ex.Message;
			}
		}

		protected static string LoadFromFile(string filePath)
		{
			try
			{
				return File.ReadAllText(filePath);
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
