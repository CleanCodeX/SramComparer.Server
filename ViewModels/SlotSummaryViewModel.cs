using System;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using IO;
using IO.Modules.Services;
using IO.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SRAM.Comparison;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Services;
using WebApp.SoE.Extensions;
using WebApp.SoE.Services;
using WebApp.SoE.Shared.Enums;
using WebApp.SoE.ViewModels.Bases;
// ReSharper disable ValueParameterNotUsed
#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Viewmodel for SoE S-RAM save slot summary</summary>
	public class SlotSummaryViewModel : ViewModelBase
	{
		public MarkupString OutputMessage { get; set; }
		public bool IsBusy { get; set; }
		public bool CanShowSummary => !IsBusy && CurrentFileStream is not null && CurrentFileSaveSlot != SaveSlotId.All;

		public bool ShowOutput => OutputMessage.ToString() != string.Empty;
		public bool IsError { get; private set; }

		public override async Task SetCurrentFileAsync(IBrowserFile file) => await base.SetCurrentFileAsync(file);

		public async Task GetSummaryAsync()
		{
			try
			{
				CanShowSummary.ThrowIfFalse(nameof(CanShowSummary));

				IsError = false;
				IsBusy = true;

				await SaveOptionsAsync();

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));

				CurrentFileStream.Position = 0;
				Options.CurrentFilePath = CurrentFileName;

				var commandHandler = new CommandHandlerSoE();
	
				var summary = commandHandler.GetSummary(CurrentFileStream, Options);

				OutputMessage = summary.Replace(Environment.NewLine, "<br>").ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsBusy = false;
		}
	}
}
