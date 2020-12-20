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
			SramComparer,
			WebTools
		}

		private bool _sramComparer = true;
		private bool _webTools = true;

		private void ExpandMenu(ExpandedMenu menu)
		{
			return;
			
			_sramComparer = menu == ExpandedMenu.SramComparer;
			_webTools = menu == ExpandedMenu.WebTools;
		}

		private void CollapseMenu() => ExpandMenu(ExpandedMenu.None);
	}
}
