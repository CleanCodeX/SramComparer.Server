using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebServer.SoE.Services;

namespace WebServer.SoE.Shared
{
	public partial class NavMenu
	{
#nullable disable
		[Inject] private IAppInfoService AppInfoService { get; set; }
		[Inject] private ProtectedSessionStorage SessionStorage { get; set; }
#nullable restore

		private enum ExpandedMenu
		{
			None,
			SramHacking = 0x1,
			SramComparison = 0x2,
			ConsoleApp_Flag = 0x4,
			ConsoleApp = ConsoleApp_Flag | SramComparison,
			WebTools_Flag = 0x8,
			WebTools = WebTools_Flag | SramComparison
		}

		private bool _sramHacking = false;
		private bool _sramComparison = false;
		private bool _consoleApp = false;
		private bool _webTools = false;
		private ExpandedMenu _menu;

		protected override async Task OnInitializedAsync()
		{
			_menu = (await SessionStorage.GetAsync<ExpandedMenu>(nameof(_menu))).Value;
			SetMenuStateVariables(_menu);
		}

		private async Task ExpandMenu(ExpandedMenu menu)
		{
			_menu = menu;

			SetMenuStateVariables(menu);

			await SessionStorage.SetAsync(nameof(_menu), _menu);
		}

		private void SetMenuStateVariables(ExpandedMenu menu)
		{
			_sramComparison = menu.HasFlag(ExpandedMenu.SramComparison);
			_sramHacking = menu.HasFlag(ExpandedMenu.SramHacking);
			_consoleApp = menu.HasFlag(ExpandedMenu.ConsoleApp);
			_webTools = menu.HasFlag(ExpandedMenu.WebTools);
		}

		private Task CollapseMenu() => ExpandMenu(ExpandedMenu.None);
	}
}
