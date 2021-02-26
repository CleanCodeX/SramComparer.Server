using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace WebApp.SoE.Extensions
{
	public static class HttpContextExtensions
	{
		public static string? GetRequestCultureId(this HttpContext? httpContext)
		{
			if (httpContext is null) return null;

			var cultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
			var requestCulture = cultureFeature.RequestCulture.UICulture!;

			return requestCulture.TwoLetterISOLanguageName!;
		}
	}
}
