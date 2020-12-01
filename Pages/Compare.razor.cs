using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SramComparer.Server.Helpers;
using SramComparer.Server.ViewModels;

namespace SramComparer.Server.Pages
{
	[Route(PageUris.CompareSoE)]
	public partial class Compare
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

		private CompareViewModel ViewModel { get; } = new();
		private bool CompareButtonDisabled => !ViewModel.CanCompare;

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
	}
}
