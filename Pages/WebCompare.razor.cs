using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using WebServer.SoE.Extensions;
using WebServer.SoE.Helpers;
using WebServer.SoE.ViewModels;

namespace WebServer.SoE.Pages
{
	[Route(PageUris.Compare)]
	public partial class WebCompare
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

		public async Task DownloadAsync()
		{
			try
			{
				await JsRuntime.StartDownloadAsync("Output.txt", Encoding.UTF8.GetBytes(CopyText()));
			}
			catch (Exception ex)
			{
				ViewModel.OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
