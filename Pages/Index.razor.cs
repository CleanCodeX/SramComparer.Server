using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SramComparer.SoE.Server.Helpers;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace SramComparer.SoE.Server.Pages
{
	[Route("/")]
	public partial class Index
	{
		private const string ReadMeUrl = "https://raw.githubusercontent.com/CleanCodeX/SramComparer/master/ReadMe.md";

#nullable disable
		[Inject]
		private IHttpClientFactory ClientFactory { get; set; }
#nullable restore

		private MarkupString ReadMeContent { get; set; }

		protected override async Task OnInitializedAsync() => ReadMeContent = MarkdownHelper.Parse(await InitContentFromUrl());

		private async Task<string> InitContentFromUrl()
		{
			try
			{
				var http = ClientFactory.CreateClient();
				var response = await http.GetAsync(ReadMeUrl);
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
