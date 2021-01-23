using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.LangCheck)]
    public partial class LangCheck
	{
		protected override void OnParametersSet() => ContentId = PageUris.LangCheck;
    }
}
