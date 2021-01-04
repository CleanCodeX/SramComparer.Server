using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;

namespace WebApp.SoE.Pages.Bases
{
	public abstract class AutoLangContentIdMarkupBase : LangContentIdMarkupBase
	{
		protected enum TranslateOptionEnum
		{
			Markup,
			Html
		}

		private static readonly TranslateOptionEnum TranslateOption = TranslateOptionEnum.Markup;

		private bool _autoTranslate = true;

		protected bool AutoTranslate
		{
			get => _autoTranslate;
			set
			{
				if (_autoTranslate == value) return;

				_autoTranslate = value;

				LoadContentAsync().ContinueWith(_ => InvokeAsync(StateHasChanged));
			}
		}

		protected bool ShowAutoTranslateOption => !LangFileFound;

		protected override async Task<MarkupString> ParseContentAsync(string? content)
		{
			if (LangFileFound || !AutoTranslate) return await base.ParseContentAsync(content);

			MarkupString markup;

			if (TranslateOption == TranslateOptionEnum.Markup)
			{
				content = await TranslateContent(content!) ?? content;
				markup = await base.ParseContentAsync(content);
			}
			else
			{
				markup = await base.ParseContentAsync(content);
				markup = (await TranslateContent(markup.Value) ?? content)!.ToMarkup();
			}

			return markup;
		}

		private async Task<string?> TranslateContent(string content) => ContentCorrections(await TranslateHelper.TranslateTextAsync(PrefixContent(content), "en", Language));

		private string PrefixContent(string content)
		{
			var textTranslationSuffix = $"This text has been automatically translated. [{Language}]".ColorText(Color.DarkGray) + Environment.NewLine.Repeat(2);

			return textTranslationSuffix + content;
		}

		private static string? ContentCorrections(string? content)
		{
			if(content is null) return null;

			return content.Remove("</ span>")!.Replace("] (", "](");
		}
	}
}