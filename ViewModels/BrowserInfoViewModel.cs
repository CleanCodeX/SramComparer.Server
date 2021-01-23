using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.ViewModels
{
	public class BrowserInfoViewModel : PageModel
	{
		private IHttpContextAccessor HttpContextAccssor { get; }

		public string UserAgent { get; }
		public string IPAddress { get; set; }
		public string BrowserName { get; set; }
		public int Version { get; set; }

		public BrowserInfoViewModel(IHttpContextAccessor httpContextAccssor)
		{
			HttpContextAccssor = httpContextAccssor;

			var httpContext = HttpContextAccssor.HttpContext!;
			UserAgent = httpContext.Request.Headers["User-Agent"];
			IPAddress = httpContext.Connection.RemoteIpAddress!.ToString();

			(BrowserName, Version) = BrowserInfoHelper.ExtractBrowserInfo(UserAgent);
		}
	}
}