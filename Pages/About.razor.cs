using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;
using WebApp.SoE.Services;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.About)]
    public partial class About
    {
#nullable disable
	    [Inject] private IAppInfoService AppInfoService { get; set; }
#nullable restore

	    protected override void OnParametersSet() => ContentId = "contributors";
    }
}
