using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;
using WebApp.SoE.Pages.Bases;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.Contributors)]
    public partial class Contributors : QueryMarkupPageBase
    {
	    private const string defaultPage = "contributors";

	    public string? Author => Page != defaultPage ? Page : null;

		protected override void OnInitialized() => Page = defaultPage;
    }
}
