using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.Services;

namespace WebApp.SoE.Shared
{
	public partial class NavMenu
	{
		private enum ExpandedMenu
		{
			None,
			SramFormat = 0x1,
			SramComparison = 0x2,
			ConsoleApp_Flag = 0x4,
			ConsoleApp = ConsoleApp_Flag | SramComparison,
			WebTools_Flag = 0x8,
			WebTools = WebTools_Flag | SramComparison
		}

		[Inject] private IRandomResTooltip RandomResTooltip { get; set; } = default!;
		[Inject] private ITooltipRandomizer TooltipRandomizer { get; set; } = default!;
		[Inject] private ProtectedSessionStorage SessionStorage { get; set; } = default!;
		[Inject] private NavigationManager NavManager { get; set; } = default!;
		
		private bool _sramFormat;
		private bool _sramComparison;
		private bool _consoleApp;
		private bool _webTools;
		private ExpandedMenu _menu;

		protected override async Task OnInitializedAsync()
		{
			await Task.Yield();
			_menu = (await SessionStorage.GetAsync<ExpandedMenu>(nameof(_menu))).Value;

			if (_menu != ExpandedMenu.None)
				SetMenuStateVariables(_menu);

			NavManager.LocationChanged += OnLocationChanged;
		}

		public void Dispose() => NavManager.LocationChanged -= OnLocationChanged;

		private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			var path = Path.Join("/", NavManager.ToBaseRelativePath(e.Location).ToLower());

			var formatItems = new [] { 
				PageUris.Goals.ToLower(), 
				PageUris.Unknowns.ToLower(), 
				PageUris.Exploring.ToLower(), 
				PageUris.RosettaStone.ToLower(),
				PageUris.Glossary.ToLower(),
			};

			var comparisonItems = new[]
			{
				PageUris.Features.ToLower(),
				PageUris.Tutorials.ToLower()
			};

			var consoleItems = new[]
			{
				PageUris.Imagery.ToLower(), 
				PageUris.ChangelogConsole.ToLower(),
				PageUris.Changelog.ToLower(),
				PageUris.ConsoleApp.ToLower()
			};

			var menu = path switch
			{
				// SramFormat
				_ when formatItems.Contains(path) => ExpandedMenu.SramFormat,

				// SramComparison
				_ when comparisonItems.Contains(path) => ExpandedMenu.SramComparison,

				// ConsoleApp
				_ when consoleItems.Contains(path) => ExpandedMenu.ConsoleApp,
				_ when path.StartsWith(PageUris.Guides, true) => ExpandedMenu.ConsoleApp,

				// WebTools
				_ when path == PageUris.Comparison.ToLower() => ExpandedMenu.WebTools,
				_ when path.StartsWith(PageUris.Offset, true) => ExpandedMenu.WebTools,
				_ when path == PageUris.SlotSummary.ToLower() => ExpandedMenu.WebTools,
				_ when path == PageUris.TerminalCodes.ToLower() => ExpandedMenu.WebTools,

				_ => ExpandedMenu.None
			};

			if (menu == _menu)
				return;

			if (_menu != menu)
			{
				_menu = menu;
#pragma warning disable 4014
				SessionStorage.SetAsync(nameof(_menu), _menu);
#pragma warning restore 4014
			}
		}

		private void SetMenuStateVariables(ExpandedMenu menu)
		{
			_sramComparison = menu.HasFlag(ExpandedMenu.SramComparison);
			_sramFormat = menu.HasFlag(ExpandedMenu.SramFormat);
			_consoleApp = menu.HasFlag(ExpandedMenu.ConsoleApp);
			_webTools = menu.HasFlag(ExpandedMenu.WebTools);
		}
	}
}
