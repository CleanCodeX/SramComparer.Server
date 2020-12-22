using Microsoft.AspNetCore.Components;
using WebServer.SoE.Services;

namespace WebServer.SoE.Shared
{
	public partial class NavMenu
	{
#nullable disable
		[Inject] private IAppInfoService AppInfoService { get; set; }
#nullable restore

		private enum ExpandedMenu
		{
			None,
			SramHacking = 0x1,
			SramComparison = 0x2,
			ConsoleApp = 0x4 | SramComparison,
			WebTools = 0x8 | SramComparison
		}

		private static bool _sramHacking = false;
		private static bool _sramComparison = false;
		private static bool _consoleApp = false;
		private static bool _webTools = false;

		private void ExpandMenu(ExpandedMenu menu)
		{
			_sramHacking = menu.HasFlag(ExpandedMenu.SramHacking);
			_sramComparison = menu.HasFlag(ExpandedMenu.SramComparison);
			_consoleApp = menu.HasFlag(ExpandedMenu.ConsoleApp);
			_webTools = menu.HasFlag(ExpandedMenu.WebTools);

			InvokeAsync(StateHasChanged);
		}

		private void CollapseMenu() => ExpandMenu(ExpandedMenu.None);
	}
}
