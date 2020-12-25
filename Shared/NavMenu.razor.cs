using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Shared
{
	public partial class NavMenu
	{
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

#nullable disable
		[Inject] private ProtectedSessionStorage SessionStorage { get; set; }
		[Inject] private NavigationManager NavManager { get; set; }
#nullable restore
		
		private bool _sramHacking;
		private bool _sramComparison;
		private bool _consoleApp;
		private bool _webTools;
		private ExpandedMenu _menu;

		protected override async Task OnInitializedAsync()
		{
			_menu = (await SessionStorage.GetAsync<ExpandedMenu>(nameof(_menu))).Value;
			
			if(_menu != ExpandedMenu.None)
				SetMenuStateVariables(_menu);

			NavManager.LocationChanged += NavManagerOnLocationChanged;
		}

		private void NavManagerOnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			var path = "/" + NavManager.ToBaseRelativePath(e.Location).ToLower();
			var menu = path switch
			{
				PageUris.Goal => ExpandedMenu.SramHacking,
				PageUris.Unknowns => ExpandedMenu.SramHacking,
				PageUris.HowCanIHelp => ExpandedMenu.SramHacking,
				PageUris.SramDocu => ExpandedMenu.SramHacking,

				PageUris.Features => ExpandedMenu.SramComparison,

				PageUris.Imagery => ExpandedMenu.ConsoleApp,
				PageUris.Downloads => ExpandedMenu.ConsoleApp,
				PageUris.HowToUse => ExpandedMenu.ConsoleApp,

				PageUris.Compare => ExpandedMenu.WebTools,
				PageUris.Offset => ExpandedMenu.WebTools,

				PageUris.GitHub => ExpandedMenu.SramComparison,

				_ => ExpandedMenu.None
			};

			if (menu == _menu)
				return;
				
#pragma warning disable 4014
			ExpandMenu(menu);
#pragma warning restore 4014
			
			InvokeAsync(StateHasChanged);
		}

		private Task ExpandSramHacking() => ExpandMenu(ExpandedMenu.SramHacking);
		private Task ExpandSramComparison() => ExpandMenu(ExpandedMenu.SramHacking);
		private Task ExpandConsoleApp() => ExpandMenu(ExpandedMenu.ConsoleApp);
		private Task ExpandWebTools() => ExpandMenu(ExpandedMenu.WebTools);
		private Task CollapseMenu() => ExpandMenu(ExpandedMenu.None);
		
		private async Task ExpandMenu(ExpandedMenu menu)
		{
			if (_menu == menu) return;

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
	}
}
