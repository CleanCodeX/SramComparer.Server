using System.Linq;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using SramComparer.Server.Helpers;
using SramComparer.Server.Pages.Bases;

namespace SramComparer.Server.Pages
{
	[Route(PageUris.Show)]
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

			if (Settings.Files.TryGetValue(page, out var file))
				Filepath = file;
			else if (Settings.Urls.TryGetValue(page, out var url))
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
