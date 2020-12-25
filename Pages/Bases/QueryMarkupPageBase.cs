using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class QueryMarkupPageBase: MarkupContentBase
	{
#nullable disable
		[Inject] private Settings Settings { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
#nullable restore
		
		protected string? Page { get; set; }

		protected override void OnParametersSet()
		{
			NavigationManager.LocationChanged += NavigationManagerOnLocationChanged;

			Page = QueryHelpers.ParseQuery(NavigationManager.Uri).Values.FirstOrDefault().ToString();
			if (Page is null)
				return;

			var page = Page.ToLower();

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
			if (e.IsNavigationIntercepted)
				NavigationManager.NavigateTo(e.Location, true);
		}
	}
}