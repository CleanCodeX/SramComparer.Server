using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebServer.SoE.Helpers;
using WebServer.SoE.ViewModels;

namespace WebServer.SoE.Pages
{
	[Route(PageUris.Offset)]
	public partial class WebOffsetEdit
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

#nullable disable
		[Inject] private SetOffsetValueViewModel ViewModel { get; set; }
#nullable restore

		private bool SaveButtonDisabled => !ViewModel.CanSave;

		private const string SelectSelectedStyle = "color: cyan;background-color: black;";
		private const string SelectUnselectedStyle = "color: white;background-color: black;";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string CurrentGameStyle => ViewModel.CurrentGame == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string RegionStyle => ViewModel.Region == default ? SelectUnselectedStyle : SelectSelectedStyle;

		protected override Task OnInitializedAsync() => ViewModel.LoadOptionsAsync();

		private Task OnCurrentFileChange(InputFileChangeEventArgs arg) => ViewModel.SetCurrentFileAsync(arg.File);
	}
}
