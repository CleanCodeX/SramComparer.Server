using System.Collections.Generic;

namespace WebApp.SoE.Services
{
	public interface ILocalizationCollector
	{
		Dictionary<string, string> GetResourceStrings(string culture);
		string GetLocalizationsCsv(string? sourceCulture) => GetLocalizationsCsv(sourceCulture, null);
		string GetLocalizationsCsv(string? sourceCulture, LocalizationOptions? options);
		string GetLocalizationsHtml(string? sourceCulture) => GetLocalizationsHtml(sourceCulture, null);
		string GetLocalizationsHtml(string? sourceCulture, LocalizationOptions? options);
	}
}