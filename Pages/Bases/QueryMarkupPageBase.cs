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
			
			HandleQueryParam();
		}

		private bool HandleQueryParam()
		{
			var page = QueryHelpers.ParseQuery(NavigationManager.Uri).Values.FirstOrDefault().ToString();
			if (page is null || Page == page)
				return false;

			Page = page;
			page = page.ToLower();

			if (Settings.Files is not null && Settings.Files.TryGetValue(page, out var file))
			{
				Filepath = file;
				return true;
			}
			
			if (Settings.Urls is not null && Settings.Urls.TryGetValue(page, out var url))
			{
				if (!url.StartsWith("http"))
					url = $"{NavigationManager.BaseUri}{url}";

				Url = url;

				return true;
			}

			return false;
		}

		private void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			if (e.IsNavigationIntercepted && HandleQueryParam())
			{
				LoadContent().GetAwaiter().GetResult();
				StateHasChanged();
			}
		}
	}
}