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

		[Parameter] public string? Url { get; set; }
		[Parameter] public string? Filepath { get; set; }
		
		protected MarkupString Content { get; set; }

		protected override Task OnParametersSetAsync() => LoadContent();

		protected async Task LoadContent()
		{
			if (Filepath is not null)
				Content = MarkdownHelper.Parse(LoadFromFile(Filepath));
			else if (Url is not null)
				Content = MarkdownHelper.Parse(await LoadFromUrlAsync(Url));
		}

		protected async Task<string> LoadFromUrlAsync(string url)
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

		protected static string LoadFromFile(string filepath)
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
