using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.Shared.Enums;
using WebApp.SoE.ViewModels;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.Comparing)]
	public partial class Comparing
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

#nullable disable
		[Inject] private IJSRuntime JsRuntime { get; set; }
		[Inject] private CompareViewModel ViewModel { get; set; }
#nullable restore
		
		private bool CompareButtonDisabled => !ViewModel.CanCompare;
		private bool ShowSummaryButtonDisabled => !ViewModel.CanShowSummary;
		private bool CopyComparisonButtonDisabled => CompareButtonDisabled || ViewModel.IsError || ViewModel.OutputMessage.ToString().IsNullOrEmpty();
		private bool CopySummaryButtonDisabled => ShowSummaryButtonDisabled || ViewModel.OutputMessage.ToString().IsNullOrEmpty();

		private const string BgColor = "#111111";

		private static readonly string SelectSelectedStyle = $"color: cyan;background-color: {BgColor};";
		private static readonly string SelectUnselectedStyle = $"color: white;background-color: {BgColor};";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string SlotByteCompStyle => ViewModel.SlotByteComparison == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string NonSlotCompStyle => ViewModel.NonSlotComparison == default ? SelectUnselectedStyle : SelectSelectedStyle;

		private string CurrentFileStyle => ViewModel.CurrentFileSaveSlot == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string ComparisonFileStyle => ViewModel.ComparisonFileSaveSlot == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string GameRegionStyle => ViewModel.GameRegion == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string Unknown12BStyle => ViewModel.Unknown12B == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string ChecksumStatusStyle => ViewModel.ChecksumStatus == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string ChecksumStyle => ViewModel.Checksum == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private bool ComparisonFileSaveSlotDisabled => ViewModel.CurrentFileSaveSlot == SaveSlotId.All;

		public SaveSlotId CurrentFileSaveSlot
		{
			get => ViewModel.CurrentFileSaveSlot;
			set
			{
				if (ViewModel.CurrentFileSaveSlot == value) return;

				if (value == SaveSlotId.All || value == ViewModel.ComparisonFileSaveSlot)
					if (ViewModel.ComparisonFileSaveSlot != SaveSlotId.All)
					{
						ViewModel.ComparisonFileSaveSlot = SaveSlotId.All;
						StateHasChanged();
					}
				
				ViewModel.CurrentFileSaveSlot = value;
			}
		}

		private async Task OnCurrentFileChange(InputFileChangeEventArgs arg)
		{
			try
			{
				await ViewModel.SetCurrentFileAsync(arg.File);
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}

		private async Task OnComparisonFileChange(InputFileChangeEventArgs arg)
		{
			try
			{
				await ViewModel.SetComparisonFileAsync(arg.File);
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}

		protected override Task OnInitializedAsync() => ViewModel.LoadOptionsAsync();

		private async Task<string> CopyComparisonTextAsync()
		{
			var oldText = ViewModel.OutputMessage;
			var wasColored = ViewModel.ColorizeOutput;

			ViewModel.ColorizeOutput = false;
			await ViewModel.CompareAsync();
			ViewModel.ColorizeOutput = true;

			var result = ViewModel.OutputMessage.ReplaceHtmlLineBreaks();

			if(wasColored)
				ViewModel.OutputMessage = oldText;

			return result;
		}

		private async Task<string> CopySummaryTextAsync()
		{
			await ViewModel.GetSummaryAsync();

			return ViewModel.OutputMessage.ReplaceHtmlLineBreaks();
		}

		public async Task DownloadComparisonResultAsync()
		{
			try
			{
				await JsRuntime.StartDownloadAsync("Output.txt", Encoding.UTF8.GetBytes(await CopyComparisonTextAsync()));
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}

		public async Task DownloadSummaryAsync()
		{
			try
			{
				await JsRuntime.StartDownloadAsync($"Saveslot_{(int)CurrentFileSaveSlot}.txt", Encoding.UTF8.GetBytes(await CopySummaryTextAsync()));
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
