using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SramComparer.Server.Extensions;
using SramComparer.Server.Helpers;
using SramComparer.Server.ViewModels;
using Res = SramComparer.Server.Properties.Resources;

namespace SramComparer.Server.Pages
{
	[Route(PageUris.CompareSoE)]
	public partial class Compare
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

#nullable disable
		[Inject]
		private IJSRuntime JsRuntime { get; set; }
#nullable restore

		private CompareViewModel ViewModel { get; } = new();
		private bool CompareButtonDisabled => !ViewModel.CanCompare;
		private bool CopyButtonDisabled => ViewModel.IsComparing || ViewModel.OutputMessage.ToString().IsNullOrEmpty();

		private const string SelectSelectedStyle = "color: cyan;background-color: black;";
		private const string SelectUnselectedStyle = "color: white;background-color: black;";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string WholeGameStyle => ViewModel.WholeGameBuffer == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string NonGameStyle => ViewModel.NonGameBuffer == default ? SelectUnselectedStyle : SelectSelectedStyle;

		private string CurrentGameStyle => ViewModel.CurrentGame == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string ComparisonGameStyle => ViewModel.ComparisonGame == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string RegionStyle => ViewModel.Region == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string Unknown12BStyle => ViewModel.Unknown12B == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string GameChecksumStyle => ViewModel.GameChecksum == default ? SelectUnselectedStyle : SelectSelectedStyle;

		private Task OnCurrentFileChange(InputFileChangeEventArgs arg) => ViewModel.SetCurrentFileAsync(arg.File);

		private async Task OnComparisonFileChange(InputFileChangeEventArgs arg) => ViewModel.ComparisonFileStream = await arg.File.OpenReadStream().CopyAsMemoryStreamAsync();

		private string CopyText()
		{
			if (!ViewModel.UseColoredOutput)
				return ViewModel.OutputMessage.ToString();

			ViewModel.UseColoredOutput = false;
			ViewModel.Compare();
			ViewModel.UseColoredOutput = true;

			return ViewModel.OutputMessage.ReplaceHtmlLinebreaks();
		}

		private async Task DownloadAsync()
		{
			try
			{
				var filename = "Output.txt";
				var result = await JsRuntime.InvokeAsync<bool>("confirm", Res.DownloadConfirmationFileTemplate.InsertArgs(filename));
				if (!result) return;

				var bytes = Encoding.UTF8.GetBytes(CopyText());
				
				await JsRuntime.InvokeVoidAsync(
					"downloadFromByteArray",
					new
					{
						ByteArray = bytes,
						FileName = filename,
						ContentType = "application/octet-stream"
					});
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
