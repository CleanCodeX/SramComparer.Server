using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.ViewModels
{
	public class BrowserRedirectViewModel : BrowserInfoViewModel
	{
		public BrowserRedirectViewModel(IServiceProvider serviceProvider) :base(serviceProvider) {}

		public IActionResult OnGet()
		{
			var isSupportedBrowser = BrowserInfo.IsSupportedBrowser(BrowserName, Version);
			return Redirect(isSupportedBrowser ? PageUris.Home : PageUris.Unsupported);
		}
	}
}