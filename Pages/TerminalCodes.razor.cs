using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.Shared.Enums;
using WebApp.SoE.ViewModels;

namespace WebApp.SoE.Pages
{
	[Route(PageUris.TerminalCodes)]
	public partial class TerminalCodes
	{
		private static readonly MarkupString Ns = (MarkupString)"&nbsp;";

		[Inject] private TerminalCodesViewModel ViewModel { get; set; } = default!;

		private bool SaveButtonDisabled => ViewModel.IsError || !ViewModel.CanSave;
		private bool ShowOutputButtonDisabled => !ViewModel.CanShowOutput;
		private bool CopyOutputButtonDisabled => ShowOutputButtonDisabled || ViewModel.OutputMessage.ToString().IsNullOrEmpty();

		private const string SelectSelectedStyle = "color: cyan;background-color: black;";
		private const string SelectUnselectedStyle = "color: white;background-color: black;";
		private const string ButtonStyle = "color: cyan;width: 600px;";
		
		private string CurrentSramFileStyle => ViewModel.CurrentFileSaveSlot == default ? SelectUnselectedStyle : SelectSelectedStyle;
		private string GameRegionStyle => ViewModel.GameRegion == default ? SelectUnselectedStyle : SelectSelectedStyle;

		protected override Task OnInitializedAsync()
		{
			ViewModel.CurrentFileSaveSlot = MandatorySaveSlotId.One;
			ViewModel.PropertyChanged += (_, _) => InvokeAsync(StateHasChanged);
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

		private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e) =>
			InvokeAsync(StateHasChanged);

		private async Task<string> CopyTerminalCodesAsync()
		{
			await ViewModel.GetTerminalCodes(false);

			return ViewModel.OutputMessage.ReplaceHtmlLineBreaks();
		}
	}
}
