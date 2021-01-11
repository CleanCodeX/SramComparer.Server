using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;
using WebApp.SoE.Services;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.About)]
    public partial class About
    {
	    [Inject] private IAppInfoService AppInfoService { get; set; } = default!;

		private bool ShowDebugLink { get; }

		protected override void OnParametersSet() => ContentId = PageUris.Contributors;

		public About()
		{
#if DEBUG
			ShowDebugLink = true;
#endif
		}
    }
}
