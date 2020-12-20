using Microsoft.AspNetCore.Components;
using SramComparer.Server.Extensions;
using SramComparer.Server.Helpers;

namespace SramComparer.Server.Pages
{
	[Route(PageUris.ReadMe)]
	public partial class ReadMe
	{
#nullable disable
		[Inject] private Settings Settings { get; set; }
#nullable restore
		protected override void OnInitialized() => Filepath = Settings.Files.GetValue("readme");
	}
}
