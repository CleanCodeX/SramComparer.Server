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
		private bool CopyButtonDisabled => ViewModel.IsError || ViewModel.IsComparing || ViewModel.OutputMessage.ToString().IsNullOrEmpty();

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

		private Task OnCurrentFileChange(InputFileChangeEventArgs arg) => ViewModel.SetCurrentFileAsync(arg.File);

		private async Task OnComparisonFileChange(InputFileChangeEventArgs arg)
		{
			ViewModel.ComparisonFileName = arg.File.Name;
			ViewModel.ComparisonFileStream = await arg.File.OpenReadStream().CopyAsMemoryStreamAsync();
		}

		protected override Task OnInitializedAsync() => ViewModel.LoadOptionsAsync();

		private async Task<string> CopyTextAsync()
		{
			if (!ViewModel.ColorizeOutput)
				return ViewModel.OutputMessage.ToString();

			ViewModel.ColorizeOutput = false;
			await ViewModel.CompareAsync();
			ViewModel.ColorizeOutput = true;

			return ViewModel.OutputMessage.ReplaceHtmlLineBreaks();
		}

		public async Task DownloadAsync()
		{
			try
			{
				await JsRuntime.StartDownloadAsync("Output.txt", Encoding.UTF8.GetBytes(await CopyTextAsync()));
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
