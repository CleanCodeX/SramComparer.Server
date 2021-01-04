using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class LangContentIdMarkupBase : ContentIdMarkupBase
	{
#nullable disable
		[Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
		[Inject] private SupportedCultures SupportedCultures { get; set; }

		protected string Language { get; private set; }
#nullable restore

		protected bool ShouldTranslate { get; private set; }
		protected bool LangFileFound { get; private set; }

		protected override void OnInitialized()
		{
			Language = GetRequestLanguage() ?? "en";
			ShouldTranslate = Language != "en" && SupportedCultures.Cultures.Contains(Language);
		}

		protected override bool TryGetFileFromSettings(string key, [NotNullWhen(true)] out string? filePath)
		{
			if (!base.TryGetFileFromSettings(key, out filePath)) return false;
			if (!ShouldTranslate) return true;

			var langFilePath = MakeLanguageFilePath(filePath, Language);
			if (File.Exists(langFilePath))
			{
				filePath = langFilePath;
				LangFileFound = true;
			}

			return File.Exists(filePath);
		}

		protected override bool TryGetUrlFromSettings(string key, [NotNullWhen(true)] out string? url)
		{
			if(!base.TryGetUrlFromSettings(key, out url)) return false;
			if (!ShouldTranslate) return true;

			var langUrl = MakeLanguageFileUrl(url, Language);

			if (UrlExists(langUrl))
			{
				url = langUrl;
				LangFileFound = true;
			}

			return true;
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

		private static bool UrlExists(string url)
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);
				//Setting the Request method HEAD, you can also use GET too.
				request.Method = "HEAD";

				using var response = (HttpWebResponse)request.GetResponse();
					return response.StatusCode == HttpStatusCode.OK;
			}
			catch
			{
				return false;
			}
		}

		private static string MakeLanguageFilePath(string filePath, string language)
		{
			var path = Path.GetDirectoryName(filePath);
			var fileName = Path.GetFileName(filePath);
			return Path.Join(path, language, fileName);
		}

		private static string MakeLanguageFileUrl(string url, string language)
		{
			var uri = new Uri(url);
			var fileName = Path.GetFileName(uri.LocalPath);
			var langFilename = $"{language}/{fileName}";
			return url.Replace(fileName, langFilename);
		}
	}
}