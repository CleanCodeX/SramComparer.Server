using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class MarkupContentBase : ComponentBase
	{
		[Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;

		private protected string? UrlToLoad;
		private protected string? FileToLoad;
		private protected bool? FileExists;
		private protected bool? UrlExists;

		protected MarkupString Content { get; set; }

		protected virtual async Task<bool> SetContentFromTextAsync(string text)
		{
			Content = await ParseContentAsync(text);
			return Content.Value != string.Empty;
		}

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

		protected async Task<string?> LoadFromUrlAsync(string url)
		{
			try
			{
				UrlToLoad = url;

				var http = ClientFactory.CreateClient();
				var response = await http.GetAsync(url);
				var result = response.IsSuccessStatusCode ?
					await response.Content.ReadAsStringAsync() : response.ReasonPhrase!;

				UrlExists = true;

				return result;
			}
			catch (Exception ex)
			{
				UrlExists = false;

				return ex.Message;
			}
		}

		protected string LoadFromFile(string filePath)
		{
			try
			{
				FileToLoad = filePath;
				FileExists = File.Exists(filePath);

				return File.ReadAllText(filePath);
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
