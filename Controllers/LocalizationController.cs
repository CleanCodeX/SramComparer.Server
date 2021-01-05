using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Controllers
{
	[Route("[Controller]")]
	[ApiController]
	public class LocalizationController : ControllerBase
	{
		public ActionResult<string> Get(string? culture, string? targetCulture = null) => Content(LocalizationHelper.GetResourceStrings(culture, Options(targetCulture)));

		[HttpGet(nameof(Html))]
		public ActionResult<string> Html(string? culture, string? targetCulture = null) => Content(LocalizationHelper.GetResourceStrings(culture, Options(targetCulture, true)), "text/html");

		[HttpGet(nameof(Csv))]
		public IActionResult Csv(string? culture, string? targetCulture = null)
		{
			culture ??= "EN";
			targetCulture ??= "Translation";

			var content = Encoding.UTF8.GetBytes(LocalizationHelper.GetResourceStrings(culture, Options(targetCulture)));
			var fileName = $"{culture}_{targetCulture}.csv";
			
			return File(content, "application/text", fileName);
		}

		private static LocalizationOptions Options(string? targetCulture = null, bool returnHtml = false) => LocalizationOptions.Create(targetCulture, returnHtml);
	}
}