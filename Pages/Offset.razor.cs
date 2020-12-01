using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SramComparer.Server.Helpers;
using SramComparer.Server.ViewModels;

namespace SramComparer.Server.Pages
{
	[Route(PageUris.OffsetSoE)]
	public partial class Offset
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

#nullable disable
		[Inject]
		private IJSRuntime JsRuntime { get; set; }

		private SetOffsetValueViewModel ViewModel { get; set; }
#nullable restore

		private bool SaveButtonDisabled => !ViewModel.CanSave;

		private const string SelectSelectedStyle = "color: cyan;background-color: black;";
		private const string SelectUnselectedStyle = "color: white;background-color: black;";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string CurrentGameStyle => ViewModel.CurrentGame == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string RegionStyle => ViewModel.Region == default ? SelectUnselectedStyle : SelectSelectedStyle;

		protected override void OnParametersSet() => ViewModel = new() { JsRuntime = JsRuntime };

		private Task OnCurrentFileChange(InputFileChangeEventArgs arg) => ViewModel.SetCurrentFileAsync(arg.File);
	}
}
