using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.LangDebug)]
    public partial class LangDebug
	{
		protected override void OnParametersSet() => ContentId = nameof(PageUris.LangDebug);
    }
}
