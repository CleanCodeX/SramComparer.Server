using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.ViewModels;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.Offset)]
	[Route(PageUris.OffsetEditing)]
	public partial class Offset
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

#nullable disable
		[Inject] private SetOffsetValueViewModel ViewModel { get; set; }
#nullable restore

		private bool SaveButtonDisabled => ViewModel.IsError || !ViewModel.CanSave;

		private const string SelectSelectedStyle = "color: cyan;background-color: black;";
		private const string SelectUnselectedStyle = "color: white;background-color: black;";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string CurrentSramFileStyle => ViewModel.CurrentFileSaveSlot == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string GameRegionStyle => ViewModel.GameRegion == default ? SelectUnselectedStyle : SelectSelectedStyle;

		protected override Task OnInitializedAsync() => ViewModel.LoadOptionsAsync();

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
	}
}
