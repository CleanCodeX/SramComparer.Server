using Microsoft.AspNetCore.Components;
using SramComparer.Server.Helpers;

namespace SramComparer.Server.Pages
{
	[Route("/")]
	public partial class Index : ComponentBase
	{
#nullable disable
		[Inject]
		private NavigationManager NavManager { get; set; }
#nullable restore

		protected override void OnAfterRender(bool firstRender) => NavManager.NavigateTo(PageUris.Features);
	}
}
