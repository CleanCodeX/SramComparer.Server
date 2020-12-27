using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class QueryMarkupPageBase : MarkupContentBase
	{
#nullable disable
		[Inject] private Settings Settings { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
#nullable restore

		protected string? Page { get; set; }

		protected override void OnParametersSet()
		{
			HandleQueryParam(Page ?? GetQueryParam());

			NavigationManager.LocationChanged += NavigationManagerOnLocationChanged;
		}

		private bool HasQueryParamChanged(bool handleNewPage = true)
		{
			if (GetQueryParam() is { } newPage && Page != newPage)
			{
				if(handleNewPage)
					HandleQueryParam(newPage);
				return true;
			}

			return false;
		}

		protected bool HandleQueryParam(string? page)
		{
			Page = page;
			page = page?.ToLower();

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

		private string? GetQueryParam() => QueryHelpers.ParseQuery(NavigationManager.Uri).Values.FirstOrDefault().ToString();

		private void NavigationManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			var forceReload = false;
			
			if(e.IsNavigationIntercepted)
			{
				if (!HasQueryParamChanged(false)) return;

				forceReload = true;
			}
			
			NavigationManager.NavigateTo(e.Location, forceReload);
		}
	}
}