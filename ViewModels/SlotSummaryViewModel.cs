using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using SRAM.Comparison.SoE.Services;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.Properties;
using WebApp.SoE.ViewModels.Bases;
// ReSharper disable ValueParameterNotUsed
#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Viewmodel for SoE S-RAM save slot summary</summary>
	public class SlotSummaryViewModel : ViewModelBase
	{
		public bool IsError { get; private set; }
		public MarkupString OutputMessage { get; set; }
		public bool IsBusy { get; set; }
		
		public bool CanShowOutput => !IsBusy && CurrentFileStream is not null;
		public bool ShowOutput => OutputMessage.ToString() != string.Empty;
		
		public async Task GetSummaryAsync()
		{
			try
			{
				CanShowOutput.ThrowIfFalse(nameof(CanShowOutput));
				CurrentFileStream.ThrowIfNull(nameof(CurrentFileStream));

				(IsError, IsBusy) = (false, true);

				await SaveOptionsAsync();
				
				CurrentFileStream.Position = 0;
				Options.CurrentFilePath = CurrentFileName;

				CommandHandlerSoE commandHandler = new();
				var summary = commandHandler.GetSummary(CurrentFileStream, Options);

				OutputMessage = summary.ReplaceWithHtmlLineBreaks();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsBusy = false;
		}

		public virtual MarkupString GetCurrentSaveslotChecksumStatus()
		{
			if (CurrentFileStream is null) return Resources.NotSet.ColorText(Color.Cyan).ToMarkup();

			return SaveslotChecksumStatusFormatter.GetSaveslotChecksumStatus(CurrentFileSaveSlot.ToInt() - 1, CurrentFileStream, GameRegion);
		}
	}
}
