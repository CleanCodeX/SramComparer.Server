using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebServer.SoE.Helpers;

namespace WebServer.SoE.Pages.Bases
{
	public abstract class MarkupContentBase : ComponentBase
	{
#nullable disable
		[Inject] private IHttpClientFactory ClientFactory { get; set; }
#nullable restore

		[Parameter] public string? Url { get; set; }
		[Parameter] public string? Filepath { get; set; }

		protected MarkupString Content { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			if (Filepath is not null)
				Content = MarkdownHelper.Parse(LoadFromFile(Filepath));
			else if (Url is not null)
				Content = MarkdownHelper.Parse(await LoadFromUrlAsync(Url));
		}

		private async Task<string> LoadFromUrlAsync(string url)
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
				return ex.Message;
			}
		}

		private static string LoadFromFile(string filepath)
		{
			try
			{
				return File.ReadAllText(filepath);
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
