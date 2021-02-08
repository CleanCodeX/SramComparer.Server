using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Common.Shared.Min.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApp.SoE.Services;

namespace WebApp.SoE.Controllers
{
	[Route("[Controller]")]
	[ApiController]
	public class LocalizationController : ControllerBase
	{
		private ILocalizationCollector LocalizationCollector { get; }

		public LocalizationController(IServiceProvider serviceProvider)
		{
			LocalizationCollector = serviceProvider.GetRequiredService<ILocalizationCollector>();
		}

		public ActionResult<string> Get(string? culture)
		{
			SetCultureDefault(ref culture);
			CheckCultures(culture);
			return Content(JsonSerializer.Serialize(LocalizationCollector.GetResourceStrings(culture!)),
				"application/json");
		}

		[HttpGet(nameof(Html))]
		public ActionResult<string> Html(string? culture, string? targetCulture = null)
		{
			SetCultureDefault(ref culture);
			CheckCultures(culture, targetCulture);
			return Content(LocalizationCollector.GetLocalizationsHtml(culture, Options(targetCulture)), "text/html");
		}

		[HttpGet(nameof(Csv))]
		public IActionResult Csv(string? culture, string? targetCulture = null)
		{
			try
			{
				SetCultureDefault(ref culture);
				CheckCultures(culture, targetCulture);

				culture = culture.ToUpper();
				targetCulture = targetCulture?.ToUpper();

				var content = Encoding.UTF8.GetBytes(LocalizationCollector.GetLocalizationsCsv(culture, Options(targetCulture)));

				targetCulture ??= "Translation";
				var fileName = $"{culture}_{targetCulture}.csv";

				if (culture.ToUpper() != "EN")
					fileName = $"EN_{fileName}";

				return File(content, "application/text", fileName);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

		private static void SetCultureDefault([NotNull] ref string? culture) => culture ??= "EN";

		private static void CheckCultures(string culture, string? targetCulture = null)
		{
			Requires.Equal(culture.Length, 2, nameof(culture), $"Culture [{culture}] must be of two letters");

			if (targetCulture is not null)
				Requires.Equal(targetCulture.Length, 2, nameof(targetCulture),
					$"Target culture [{targetCulture}] must be of two letters");
		}

		private static LocalizationOptions Options(string? targetCulture = null) => LocalizationOptions.Create(targetCulture);
	}
}