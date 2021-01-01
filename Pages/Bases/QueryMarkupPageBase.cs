using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class QueryMarkupPageBase : MarkupContentBase
	{
#nullable disable
		[Inject] private Settings Settings { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
		[Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
#nullable restore

		protected string? Page { get; set; }
		protected bool AutoTranslate { get; set; } = true;

		protected override Task OnParametersSetAsync() => HandleQueryParamAsync(Page ?? GetQueryParam());

		protected async Task HandleQueryParamAsync(string? page)
		{
			Url = Filepath = null;

			Page = page;
			page = page?.ToLower();

			if (page is null) return;

			var language = GetRequestLanguage() ?? "en";
			var translate = language != "en";

			if (translate && await TryLoadFileAsync(page, language)) return;
			if (TryGetFileName(page))
				return;

			if (translate && await TryLoadFromUrlAsync(page, language)) return;
			
			await TryGetUrlAsync(page);
		}

		private string? GetRequestLanguage()
		{
			if (HttpContextAccessor.HttpContext is { } httpContext)
			{
				var cultureFeature = httpContext.Features.Get<IRequestCultureFeature>();
				var requestCulture = cultureFeature.RequestCulture.UICulture!;
				return requestCulture.TwoLetterISOLanguageName!;
			}

			return null;
		}

		private async Task<bool> TryGetUrlAsync(string page)
		{
			if (Settings.Urls is not null && Settings.Urls.TryGetValue(page, out var url))
			{
				if (!url.StartsWith("http"))
					url = $"{NavigationManager.BaseUri}{url}";

				Content = MarkdownHelper.Parse(await LoadFromUrlAsync(url));

				return true;
			}

			return false;
		}

		private bool TryGetFileName(string page)
		{
			if (Settings.Files is not null && Settings.Files.TryGetValue(page, out var file))
			{
				Content = MarkdownHelper.Parse(LoadFromFile(file) + Environment.NewLine + "(F)");

				return true;
			}

			return false;
		}

		private async Task<bool> TryLoadFromUrlAsync(string page, string language)
		{
			if (Settings.Urls is null || !Settings.Urls.TryGetValue(page, out var url)) return false;
			
			var langUrl = MakeLanguageFilePath(url, language);

			var content = await LoadFromUrlAsync(langUrl, true);
			if (content is not null)
			{
				Content = MarkdownHelper.Parse(content + Environment.NewLine + "(U)");
				return true;
			}

			content = await LoadFromUrlAsync(url);
			if (content.IsNullOrEmpty()) return false;

			if (!AutoTranslate)
			{
				Content = MarkdownHelper.Parse(content + Environment.NewLine + "(U)");
				return true;
			}

			var translatedContent = await TranslateContent(content!, language);

			Content = MarkdownHelper.Parse(translatedContent ?? content);

			return true;
		}

		private async Task<bool> TryLoadFileAsync(string page, string language)
		{
			if (Settings.Files is null || !Settings.Files.TryGetValue(page, out var filePath)) return false;
			
			var langFilePath = MakeLanguageFilePath(filePath, language);
			if (File.Exists(langFilePath))
			{
				Content = MarkdownHelper.Parse(LoadFromFile(langFilePath) + Environment.NewLine + "(F)");
				return true;
			}

			var content = LoadFromFile(filePath);
			if (content.IsNullOrEmpty()) return false;

			if (!AutoTranslate)
			{
				Content = MarkdownHelper.Parse(content + Environment.NewLine + "(F)");
				return true;
			}

			var translatedContent = await TranslateContent(content!, language);

			Content = MarkdownHelper.Parse(translatedContent ?? content);

			return true;
		}

		protected virtual string? GetQueryParam() => QueryHelpers.ParseQuery(NavigationManager.Uri).Values.FirstOrDefault().ToString();

		private static string MakeLanguageFilePath(string filePath, string language)
		{
			var path = Path.GetDirectoryName(filePath);
			var fileName = Path.GetFileNameWithoutExtension(filePath);
			var extension = Path.GetExtension(filePath);
			return Path.Join(path, $"{fileName}-{language}{extension}");
		}

		private static async Task<string?> TranslateContent(string content, string language)
		{
			var textTranslationSuffix = $"This text has been automatically translated. [{language}]".ColorText(Color.DarkGray) + Environment.NewLine.Repeat(2);

			return (await TranslateHelper.TranslateTextAsync(textTranslationSuffix + content, "en", language))
				?.Remove("</ span>")
				?.Replace("] (", "](");
		}
	}
}