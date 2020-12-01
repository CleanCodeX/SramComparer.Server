using Microsoft.AspNetCore.Components;
using SramComparer.Server.Helpers;
using SramComparer.Server.Services;

namespace SramComparer.Server.Shared
{
	public partial class NavMenu
	{
#nullable disable
		[Inject] private IAppInfoService AppInfoService { get; set; }
		[Inject] private Settings Settings { get; set; }
		[Inject] private NavigationManager Navigation { get; set; }
#nullable restore

		private bool collapseNavMenu = true;

		private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

		private void ToggleNavMenu() => collapseNavMenu = !collapseNavMenu;

		private void OpenChangelog() => Navigation.NavigateTo(Settings.ChangeLogUrl!);
	}
}
