using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;
using WebApp.SoE.Pages.Bases;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.Contributors)]
    public partial class Contributors : LangContentIdMarkupBase
    {
	    private const string defaultPage = "contributors";

	    public string? Author => ContentId != defaultPage ? ContentId : null;

		protected override string? GetQueryParam() => base.GetQueryParam() ?? defaultPage;
    }
}
