using Microsoft.AspNetCore.Components;
using SramComparer.Server.Extensions;
using SramComparer.Server.Helpers;
using SramComparer.Server.Services;

namespace SramComparer.Server.Shared
{
	public partial class NavMenu
	{
#nullable disable
		[Inject] private IAppInfoService AppInfoService { get; set; }
		[Inject] private Settings Settings { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
#nullable restore

		private enum ExpandedMenu
		{
			None,
			SramComparer,
			WebTools
		}

		private bool _sramComparer = true;
		private bool _webTools = true;

		private string? GetUrl(string key) => Settings.Urls.GetValue(key);

		//private string? GetNavMenuCssClass(bool flag) => !flag ? null : "collapse";

		private void ExpandMenu(ExpandedMenu menu)
		{
			return;
			
			_sramComparer = menu == ExpandedMenu.SramComparer;
			_webTools = menu == ExpandedMenu.WebTools;
		}

		private void CollapseMenu() => ExpandMenu(ExpandedMenu.None);

		private void OpenChangelog() => NavigationManager.NavigateTo(PageUris.Changelog);
	}
}
