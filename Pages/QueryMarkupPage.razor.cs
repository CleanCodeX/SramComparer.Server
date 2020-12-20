using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using WebServer.SoE.Helpers;
using WebServer.SoE.Pages.Bases;

namespace WebServer.SoE.Pages
{
	[Route(PageUris.Embed)]
	public partial class QueryMarkupPage : MarkupContentBase
	{
#nullable disable
		[Inject] private Settings Settings { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
#nullable restore

		protected override void OnParametersSet()
		{
			NavigationManager.LocationChanged += NavigationManagerOnLocationChanged;
				
			if (Url is not null || Filepath is not null) return;

			var page = QueryHelpers.ParseQuery(NavigationManager.Uri).Values.FirstOrDefault().ToString();
			if (page is null)
				return;

			page = page.ToLower();

			if (Settings.Files is not null && Settings.Files.TryGetValue(page, out var file))
				Filepath = file;
			else if (Settings.Urls is not null && Settings.Urls.TryGetValue(page, out var url))
			{
				if (!url.StartsWith("http"))
					url = $"{NavigationManager.BaseUri}{url}";

				Url = url;
			}
		}

		private void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			if(e.IsNavigationIntercepted)
				NavigationManager.NavigateTo(e.Location, true);
		}
	}
}
