using Microsoft.AspNetCore.Components;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.Contributors)]
    public partial class Contributors 
	{
		protected override void OnParametersSet() => ContentId = PageUris.Contributors;
    }
}
