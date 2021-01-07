using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebApp.SoE.Extensions;
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
		
		private bool _sramFormat;
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
			var path = Path.Join("/", NavManager.ToBaseRelativePath(e.Location).ToLower());
			var menu = path switch
			{
				_ when path == PageUris.Goals.ToLower() => ExpandedMenu.SramHacking,
				_ when path == PageUris.Unknowns.ToLower() => ExpandedMenu.SramHacking,
				_ when path == PageUris.Explore.ToLower() => ExpandedMenu.SramHacking,
				_ when path == PageUris.SramDocu.ToLower() => ExpandedMenu.SramHacking,

				_ when path == PageUris.Features.ToLower() => ExpandedMenu.SramComparison,

				_ when path == PageUris.Imagery.ToLower() => ExpandedMenu.ConsoleApp,
				_ when path == PageUris.ChangelogConsole.ToLower() => ExpandedMenu.ConsoleApp,
				_ when path == PageUris.Guide.ToLower() => ExpandedMenu.ConsoleApp,

				_ when path == PageUris.Compare.ToLower() => ExpandedMenu.WebTools,
				_ when path == PageUris.OffsetEdit.ToLower() => ExpandedMenu.WebTools,

				_ when path == PageUris.Sources.ToLower() => ExpandedMenu.SramComparison,

				_ => ExpandedMenu.None
			};

			if (menu == _menu)
				return;
				
#pragma warning disable 4014
			ExpandMenu(menu);
#pragma warning restore 4014
			
			InvokeAsync(StateHasChanged);
		}

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
			_sramFormat = menu.HasFlag(ExpandedMenu.SramHacking);
			_consoleApp = menu.HasFlag(ExpandedMenu.ConsoleApp);
			_webTools = menu.HasFlag(ExpandedMenu.WebTools);
		}
	}
}
