using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using WebApp.SoE.Services;

namespace WebApp.SoE.ViewModels
{
	public class BrowserInfoViewModel : PageModel
	{
		private IHttpContextAccessor HttpContextAccssor { get; }
		protected IBrowserInfo BrowserInfo { get; }

		public string UserAgent { get; }
		public string IPAddress { get; set; }
		public string BrowserName { get; set; }
		public int Version { get; set; }

		public BrowserInfoViewModel(IServiceProvider serviceProvider)
		{
			HttpContextAccssor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
			BrowserInfo = serviceProvider.GetRequiredService<IBrowserInfo>();

			var httpContext = HttpContextAccssor.HttpContext!;
			UserAgent = httpContext.Request.Headers["User-Agent"];
			IPAddress = httpContext.Connection.RemoteIpAddress!.ToString();

			(BrowserName, Version) = BrowserInfo.ExtractBrowserInfo(UserAgent);
		}
	}
}