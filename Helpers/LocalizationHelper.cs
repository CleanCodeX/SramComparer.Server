using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using Common.Shared.Min.Extensions;
using WebApp.SoE.Extensions;
// ReSharper disable RedundantNameQualifier

namespace WebApp.SoE.Helpers
{
	public class LocalizationOptions
	{
		public string? TargetCultureInitText { get; set; }
		public string? TargetCulture { get; set; }
		public bool HideEnglish { get; set; }

		public static LocalizationOptions Create(string? targetCulture = null) => new() { TargetCulture = targetCulture};
	}

	public static class LocalizationHelper
	{
		private const string TargetLanguageDefaultText = "YOUR TRANSLATION";

		private static readonly Type[] ResTypes = 
		{
			typeof(WebApp.SoE.Properties.Resources),
			typeof(SRAM.Comparison.SoE.Properties.Resources),
			typeof(SRAM.Comparison.Properties.Resources),
			typeof(global::SoE.Properties.Resources),
			typeof(IO.Properties.Resources),
		};

		public static Dictionary<string, string> GetResourceStrings(string culture)
		{
			var cultureInfo = CultureInfo.GetCultureInfo(culture);

			Dictionary<string, string> result = new();

			foreach (var type in ResTypes)
			{
				var assemblyName = type.Assembly.GetName().Name!;
				var resManager = new ResourceManager(type);
				var resourceSet = resManager.GetResourceSet(cultureInfo, true, true)!;

				foreach (var entry in resourceSet.OfType<DictionaryEntry>().OrderBy(e => e.Key))
				{
					var key = $"{assemblyName}.{entry.Key}";
					var value = entry.Value!.ToString()!;

					result.Add(key, value);
				}
			}

			return result;
		}

		public static string GetLocalizationsCsv(string? sourceCulture) => GetLocalizationsCsv(sourceCulture, null);
		public static string GetLocalizationsCsv(string? sourceCulture, LocalizationOptions? options)
		{
			options ??= new LocalizationOptions();
			sourceCulture ??= "en";

			var targetCulture = options.TargetCulture ?? "Translation";
			var targetCultureInitText = options.TargetCultureInitText ?? TargetLanguageDefaultText;
			var cultureInfo = CultureInfo.GetCultureInfo(sourceCulture);
			var enCultureInfo = CultureInfo.GetCultureInfo("en");
			var showEnglish = sourceCulture.ToLower() != "en" && !options.HideEnglish;

			sourceCulture = sourceCulture.ToUpperInvariant();
			targetCulture = targetCulture.ToUpperInvariant();
			
			CultureInfo? targetCultureInfo = null;
			if (options.TargetCulture is not null)
				targetCultureInfo = CultureInfo.GetCultureInfo(options.TargetCulture);

			var sb = new StringBuilder();

			sb.Append("ID");

			if (showEnglish)
				sb.Append(";EN");

			sb.Append($";{sourceCulture}");
			sb.AppendLine($";{targetCulture}");

			foreach (var type in ResTypes)
			{
				var assemblyName = type.Assembly.GetName().Name!;
				var resManager = new ResourceManager(type);
				var resourceSet = resManager.GetResourceSet(cultureInfo, true, true)!;
				ResourceSet? targetResourceSet = null;

				if (targetCultureInfo is not null)
					targetResourceSet = resManager.GetResourceSet(targetCultureInfo, true, false)!;

				var enResourceSet = resManager.GetResourceSet(enCultureInfo, true, true)!;

				foreach (var entry in resourceSet.OfType<DictionaryEntry>().OrderBy(e => (string)e.Key))
				{
					var key = (string)entry.Key;
					var qualifiedKey = $"{assemblyName}.{key}";
					var value = entry.Value!.ToString()!;

					sb.Append(qualifiedKey);

					if (showEnglish)
						sb.Append($";{GetEnCultureText()}");

					sb.Append($";{value}");
					sb.AppendLine($";{TryGetTargetCultureText()}");

					string TryGetTargetCultureText() => targetResourceSet?.GetString(key) ?? targetCultureInitText;
					string GetEnCultureText() => enResourceSet.GetString(key) ?? targetCultureInitText;
				}
			}

			return sb.ToString();
		}

		public static string GetLocalizationsHtml(string? sourceCulture) => GetLocalizationsHtml(sourceCulture, null);
		public static string GetLocalizationsHtml(string? sourceCulture, LocalizationOptions? options)
		{
			options ??= new LocalizationOptions();
			sourceCulture ??= "en";

			var hideTargetColumn = options.TargetCulture is null;
			var targetCulture = options.TargetCulture ?? "Translation";
			var targetCultureInitText = options.TargetCultureInitText ?? TargetLanguageDefaultText;
			var cultureInfo = CultureInfo.GetCultureInfo(sourceCulture);
			var enCultureInfo = CultureInfo.GetCultureInfo("en");
			var showEnglish = sourceCulture.ToLower() != "en" && !options.HideEnglish;

			sourceCulture = sourceCulture.ToUpperInvariant();
			targetCulture = targetCulture?.ToUpperInvariant();

			CultureInfo? targetCultureInfo = null;
			if (options.TargetCulture is not null)
				targetCultureInfo = CultureInfo.GetCultureInfo(options.TargetCulture);

			var sb = new StringBuilder();

			sb.AppendLine("<table>");
			sb.Append("<tr style='text-align:left'><th id='id'>ID</th>");
			
			if (showEnglish)
				sb.Append("<th id='EN'>EN</th>");

			sb.Append($"<th id='source'>{sourceCulture}</th>");

			if (!hideTargetColumn)
				sb.Append($"<th id='target'>{targetCulture}</th>");

			sb.AppendLine("</tr>");

			const string tdTemplate = "<td>{0}</td>";

			foreach (var type in ResTypes)
			{
				var assemblyName = type.Assembly.GetName().Name!;
				var resManager = new ResourceManager(type);
				var resourceSet = resManager.GetResourceSet(cultureInfo, true, true)!;
				ResourceSet? targetResourceSet = null;

				if (targetCultureInfo is not null)
					targetResourceSet = resManager.GetResourceSet(targetCultureInfo, true, false)!;

				var enResourceSet = resManager.GetResourceSet(enCultureInfo, true, true)!;

				foreach (var entry in resourceSet.OfType<DictionaryEntry>().OrderBy(e => (string)e.Key))
				{
					var key = (string)entry.Key;
					var qualifiedKey = $"{assemblyName}.{key}";
					var value = entry.Value!.ToString()!;

					sb.Append("<tr>");
					sb.Append(tdTemplate.InsertArgs(qualifiedKey));

					if (showEnglish)
						sb.Append(tdTemplate.InsertArgs(GetEnCultureText().ReplaceWithHtmlLineBreaks()));

					sb.Append(tdTemplate.InsertArgs(value.ReplaceWithHtmlLineBreaks()));

					if (!hideTargetColumn)
						sb.Append(tdTemplate.InsertArgs(TryGetTargetCultureText().ReplaceWithHtmlLineBreaks()));

					sb.AppendLine("</tr>");

					string TryGetTargetCultureText() => targetResourceSet?.GetString(key) ?? targetCultureInitText;
					string GetEnCultureText() => enResourceSet.GetString(key) ?? targetCultureInitText;
				}
			}

			sb.AppendLine("</table>");

			return sb.ToString();
		}
	}
}
