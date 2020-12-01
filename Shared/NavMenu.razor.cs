using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SramComparer.Server.Extensions;
using SramComparer.Server.Services;

namespace SramComparer.Server.Shared
{
	public partial class NavMenu
	{
#nullable disable
		[Inject] private IAppInfoService AppInfoService { get; set; }
		[Inject] private IJSRuntime JsRuntime { get; set; }
#nullable restore

		private bool collapseNavMenu = true;

		private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

		private void ToggleNavMenu() => collapseNavMenu = !collapseNavMenu;

		public async Task DownloadChangelogAsync()
		{
			try
			{
				var filename = "changelog.txt";
				await JsRuntime.StartDownloadAsync(filename, File.ReadAllBytes(filename));
			}
			catch
			{ }
		}
    }
}
