using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using WebApp.SoE.Extensions;

namespace WebApp.SoE.Helpers
{
	public class LocalizationOptions
	{
		public bool ReturnHtml { get; set; }
		public string? TargetCultureInitText { get; set; }
		public string? TargetCulture { get; set; }
		public bool HideTargetColumn { get; set; }

		public static LocalizationOptions Create(string? targetCulture = null, bool returnHtml = false, bool hideTargetColumn = false) =>
			new() { TargetCulture = targetCulture, ReturnHtml = returnHtml, HideTargetColumn = hideTargetColumn };
	}

	public static class LocalizationHelper
	{
		public static string GetResourceStrings() => GetResourceStrings("en");
		public static string GetResourceStrings(string? sourceCulture) => GetResourceStrings(sourceCulture, null);
		public static string GetResourceStrings(string? sourceCulture, LocalizationOptions? options)
		{
			options ??= new LocalizationOptions();
			sourceCulture ??= "en";

			var returnHtml = options.ReturnHtml;
			var hideTargetColumn = options.HideTargetColumn;
			var targetCulture = options.TargetCulture ?? "Translation";
			var targetCultureInitText = options.TargetCultureInitText ?? "NO TRANSLATION YET";
			var cultureInfo = CultureInfo.GetCultureInfo(sourceCulture);
			
			sourceCulture = sourceCulture.ToUpperInvariant();
			targetCulture = targetCulture.ToUpperInvariant();

			var resTypes = new[]
			{
				typeof(WebApp.SoE.Properties.Resources),
				typeof(SramComparer.SoE.Properties.Resources),
				typeof(SramComparer.Properties.Resources),
				typeof(SramFormat.SoE.Properties.Resources),
				typeof(SramCommons.Properties.Resources),
			};
			
			var sb = new StringBuilder();

			if (returnHtml)
			{
				sb.AppendLine("<table>");
				sb.AppendLine($"<tr><th id='id'>ID</th><th id='source'>{sourceCulture}</th>{(hideTargetColumn ? string.Empty : $"<th id='target'>{targetCulture}</th>")}</tr>");
			}
			else
				sb.AppendLine($"ID;{sourceCulture};{targetCulture}");

			foreach (var type in resTypes)
			{
				var assemblyName = type.Assembly.GetName().Name!;
				var resManager = new ResourceManager(type);
				var resourceSet = resManager.GetResourceSet(cultureInfo, true, true)!;

				foreach (var entry in resourceSet.OfType<DictionaryEntry>().OrderBy(e => e.Key))
				{
					var key = $"{assemblyName}.{entry.Key}";
					var value = entry.Value!.ToString()!;

					if(returnHtml)
						sb.AppendLine($"<tr><td>{key}</td><td>{value.ReplaceWithHtmlLineBreaks()}</td>{(hideTargetColumn ? string.Empty : $"<td>{targetCultureInitText.ReplaceSpaces()}</td>")}</tr>");
					else
						sb.AppendLine($"{key};{value};{targetCultureInitText}");
				}
			}

			if (returnHtml)
				sb.AppendLine("</table>");

			return sb.ToString();
		}
	}
}
