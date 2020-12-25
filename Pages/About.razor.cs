using Microsoft.AspNetCore.Components;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.Services;

namespace WebApp.SoE.Pages
{
	[Route("/about")]
    public partial class About
    {
#nullable disable
	    [Inject] private IAppInfoService AppInfoService { get; set; }
        [Inject] private Settings Settings { get; set; }
#nullable restore

        protected override void OnInitialized()
        {
            const string? key = "contributors";
            if (Settings.Files is not null && Settings.Files.TryGetValue(key, out var file))
                Filepath = file;
            else
                Url = Settings.Urls.GetValue(key);
        }
    }
}
