using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SramComparer.Server.ViewModels;

namespace SramComparer.Server.Pages
{
	public partial class CompareSoE
	{
		private readonly MarkupString Ns = (MarkupString)"&nbsp;";

		private CompareSoEViewModel ViewModel { get; } = new CompareSoEViewModel();
		private bool CompareButtonDisabled => !ViewModel.CanCompare;

		private string selectSelectedStyle = "color: cyan;background-color: black;";
		private string selectUnselectStyle = "color: white;background-color: black;";
		private string ButtonStyle = "color: cyan;width: 600px;";
		
		private string WholeGameStyle => ViewModel.WholeGameBuffer == default ? selectUnselectStyle : selectSelectedStyle;
		private string NonGameStyle => ViewModel.NonGameBuffer == default ? selectUnselectStyle : selectSelectedStyle;

		private string CurrentGameStyle => ViewModel.CurrentGame == default ? selectUnselectStyle : selectSelectedStyle;
		private string ComparisonGameStyle => ViewModel.ComparisonGame == default ? selectUnselectStyle : selectSelectedStyle;
		private string RegionStyle => ViewModel.Region == default ? selectUnselectStyle : selectSelectedStyle;
		private string Unknown12BStyle => ViewModel.Unknown12B == default ? selectUnselectStyle : selectSelectedStyle;
		private string GameChecksumStyle => ViewModel.GameChecksum == default ? selectUnselectStyle : selectSelectedStyle;

		private async Task OnCurrentFileChange(InputFileChangeEventArgs arg) => ViewModel.CurrentFileStream = await arg.File.OpenReadStream().CopyAsMemoryStreamAsync();

		private async Task OnComparisonFileChange(InputFileChangeEventArgs arg) => ViewModel.ComparisonFileStream = await arg.File.OpenReadStream().CopyAsMemoryStreamAsync();
	}
}
