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
	[Route(PageUris.SlotSummary)]
	public partial class SlotSummary
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

		[Inject] private IJSRuntime JsRuntime { get; set; } = default!;
		[Inject] private SlotSummaryViewModel ViewModel { get; set; } = default!;

		private bool ShowSummaryButtonDisabled => !ViewModel.CanShowSummary;
		private bool CopySummaryButtonDisabled => ShowSummaryButtonDisabled || ViewModel.OutputMessage.ToString().IsNullOrEmpty();

		private const string SelectSelectedStyle = "color: cyan;background-color: black;";
		private const string SelectUnselectedStyle = "color: white;background-color: black;";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string CurrentSramFileStyle => ViewModel.CurrentFileSaveSlot == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string GameRegionStyle => ViewModel.GameRegion == default ? SelectUnselectedStyle : SelectSelectedStyle;

		private MandatorySaveSlotId CurrentFileSaveSlot
		{
			get => (MandatorySaveSlotId)ViewModel.CurrentFileSaveSlot;
			set => ViewModel.CurrentFileSaveSlot = (SaveSlotId)value;
		}

		protected override Task OnInitializedAsync()
		{
			CurrentFileSaveSlot = MandatorySaveSlotId.One;
			return ViewModel.LoadOptionsAsync();
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

		private async Task<string> CopySummaryTextAsync()
		{
			await ViewModel.GetSummaryAsync();

			return ViewModel.OutputMessage.ReplaceHtmlLineBreaks();
		}

		public async Task DownloadSummaryAsync()
		{
			try
			{
				await JsRuntime.StartDownloadAsync($"Saveslot_{(int)ViewModel.CurrentFileSaveSlot}.txt", Encoding.UTF8.GetBytes(await CopySummaryTextAsync()));
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
