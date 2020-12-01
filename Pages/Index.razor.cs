using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SramComparer.Server.Helpers;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace SramComparer.Server.Pages
{
	[Route("/")]
	public partial class Index
	{
#nullable disable
		[Inject]
		private IHttpClientFactory ClientFactory { get; set; }
		[Inject]
		private Settings Settings { get; set; }
#nullable restore

		private MarkupString ReadMeContent { get; set; }

		protected override async Task OnInitializedAsync() => ReadMeContent = MarkdownHelper.Parse(await InitContentFromUrl());

		private async Task<string> InitContentFromUrl()
		{
			try
			{
				var http = ClientFactory.CreateClient();
				var response = await http.GetAsync(Settings.ReadMeUrl);
				var result = response.IsSuccessStatusCode ?
					await response.Content.ReadAsStringAsync() : response.ReasonPhrase!;

				return result;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
