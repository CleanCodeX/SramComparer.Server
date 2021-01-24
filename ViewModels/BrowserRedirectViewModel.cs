using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.ViewModels
{
	public class BrowserRedirectViewModel : BrowserInfoViewModel
	{
		public BrowserRedirectViewModel(IHttpContextAccessor httpContextAccssor) :base(httpContextAccssor) {}

		public IActionResult OnGet()
		{
			var isSupportedBrowser = BrowserInfoHelper.IsSupportedBrowser(BrowserName, Version);
			return Redirect(isSupportedBrowser ? PageUris.Home : PageUris.Unsupported);
		}
	}
}