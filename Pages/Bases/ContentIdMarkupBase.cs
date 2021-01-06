using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class ContentIdMarkupBase : MarkupContentBase
	{
#nullable disable
		[Inject] protected Settings Settings { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
#nullable restore

		protected string? ContentId { get; set; }

		protected override Task OnParametersSetAsync()
		{
			ContentId ??= GetQueryParam();
			return LoadContentAsync();
		}

		protected virtual async Task LoadContentAsync()
		{
			if (ContentId is null) return;

			var contentId = ContentId.ToLower();

			if (await SetContentFromFileAsync(contentId))
				return;

			if (await SetContentFromUrlAsync(contentId))
				return;

			await SetContentFromTextAsync($"Content '{ContentId}' not found :(");
		}

		protected override async Task<bool> SetContentFromUrlAsync(string page)
		{
			if (TryGetUrlFromSettings(page, out var url))
			{
				if (!url.StartsWith("http"))
					url = $"{NavigationManager.BaseUri}{url}";

				await base.SetContentFromUrlAsync(url);

				return true;
			}

			return false;
		}

		protected override async Task<bool> SetContentFromFileAsync(string page)
		{
			if (TryGetFileFromSettings(page, out var filePath))
			{
				await base.SetContentFromFileAsync(filePath);

				return true;
			}

			return false;
		}

		protected virtual bool TryGetFileFromSettings(string key, [NotNullWhen(true)] out string? filePath)
		{
			if (Settings.Files is not null && Settings.Files.TryGetValue(key, out filePath))
				return true;

			filePath = null;

			return false;
		}

		protected virtual bool TryGetUrlFromSettings(string key, [NotNullWhen(true)] out string? url)
		{
			if (Settings.Urls is not null && Settings.Urls.TryGetValue(key, out url))
				return true;

			url = null;

			return false;
		}

		protected virtual string? GetQueryParam() => QueryHelpers.ParseQuery(NavigationManager.Uri).Values.FirstOrDefault().ToString().ToNullIfEmpty();
	}
}