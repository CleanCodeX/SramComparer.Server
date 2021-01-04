using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;
using WebApp.SoE.Pages.Bases;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.Contributors)]
    public partial class Contributors : AutoLangContentIdMarkupBase
    {
	    protected override void OnParametersSet() => ContentId = nameof(PageUris.Contributors);
	}
}
