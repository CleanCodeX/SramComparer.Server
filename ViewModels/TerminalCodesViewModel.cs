using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SoE.Models.Enums;
using SoE.Models.Structs;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.ViewModels.Bases;
using ResComp = SRAM.Comparison.SoE.Properties.Resources;
// ReSharper disable ValueParameterNotUsed
#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Viewmodel for SoE S-RAM secret door code</summary>
	public class TerminalCodesViewModel : LoadViewModelBase
	{
		[Inject] internal IJSRuntime JsRuntime { get; set; } = default!;

		public bool IsBusy { get; set; }
		public bool Changed { get; set; }
		public bool TerminalCodesAreTheSame { get; set; }
		public TerminalCode AlarmCode { get; set; }
		public TerminalCode SecretCode { get; set; }

		public bool CanSet => IsLoaded && TerminalCodesAreTheSame;
		public bool CanSave => Changed && CanSet && !IsSavestate;
		public bool CanShowOutput => !IsBusy && IsLoaded;

		public override Task SetCurrentFileAsync(IBrowserFile file)
		{
			(AlarmCode, SecretCode, TerminalCodesAreTheSame, Changed, OutputMessage) = (default, default, default, default, default);
			return base.SetCurrentFileAsync(file);
		}

		public async Task GetTerminalCodes(bool formatAsHtml)
		{
			(IsError, IsBusy) = (false, true);

			try
			{
				CanShowOutput.ThrowIfFalse(nameof(CanShowOutput));
				SramFile.ThrowIfNull(nameof(SramFile));
				
				await SaveOptionsAsync();
				
				Options.CurrentFilePath = CurrentFileName;

				var nl = Environment.NewLine;
				var slotIndex = CurrentFileSaveSlot.ToInt() - 1;
				var saveslotData = SramFile.GetSegment(slotIndex);
				var chunk20 = saveslotData.Data.CurrentWeapon_LastLanding;

				(AlarmCode, SecretCode) = (chunk20.AlarmCode, chunk20.SecretCode);
				
				var outputTemplate = $"{ResComp.AlarmCode}: {{0}}{nl}{ResComp.SecretBossRoomCode}: {{1}}";
				Color color = default;
				string? outputSuffix = null;
				
				if (AlarmCode.IsValid && SecretCode.IsValid)
				{
					TerminalCodesAreTheSame = Equals(AlarmCode, SecretCode);
					if (TerminalCodesAreTheSame)
					{
						color = Color.Orange;
						outputSuffix += nl + ResComp.StatusBothTerminalCodesAreEqual.ColorText(Color.Yellow, formatAsHtml);
					}
				}
				else if (AlarmCode.Code1.ToUShort() == TerminalCodeFormatter.EmptySaveSlotValue)
					outputSuffix += nl + ResComp.StatusSaveslotIsEmpty.ColorText(Color.Yellow, formatAsHtml);

				var output = outputTemplate
					             .InsertArgs(TerminalCodeFormatter.FormatTerminalCode(AlarmCode, formatAsHtml, color), TerminalCodeFormatter.FormatTerminalCode(SecretCode, formatAsHtml, color)) 
				             + outputSuffix;

				OutputMessage = output.ReplaceWithHtmlLineBreaks(formatAsHtml);
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsBusy = false;
			Changed = false;
		}

		public async Task ChangeSecretDoorCodeIfSame()
		{
			(IsError, IsBusy, Changed) = (false, true, false);

			try
			{
				CanShowOutput.ThrowIfFalse(nameof(CanShowOutput));
				SramFile.ThrowIfNull(nameof(SramFile));
				
				await SaveOptionsAsync();

				var slotIndex = CurrentFileSaveSlot.ToInt() - 1;
				var saveslotData = SramFile.GetSegment(slotIndex);
				var chunk20 = saveslotData.Data.CurrentWeapon_LastLanding;

				(AlarmCode, SecretCode) = (chunk20.AlarmCode, chunk20.SecretCode);

				if (!Equals(chunk20.SecretCode, chunk20.AlarmCode))
					return;

				var code = chunk20.SecretCode;

				code.Code1 = code.Code1 switch
				{
					TerminalCodeColor.Blue => TerminalCodeColor.Green,
					TerminalCodeColor.Green => TerminalCodeColor.Blue,
					_ => TerminalCodeColor.Green,
				};
				
				chunk20.SecretCode = code;
				saveslotData.Data.CurrentWeapon_LastLanding = chunk20;

				SramFile.SetSegment(slotIndex, saveslotData);

				var output = $"{ResComp.StatusNewSecretBossRoomCode}: {code.ToString().ColorText(Color.Green)}";

				TerminalCodesAreTheSame = true;

				Changed = true;
				OutputMessage = output.ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsBusy = false;
		}

		public async Task SaveAndDownloadAsync()
		{
			IsError = false;

			try
			{
				SramFile.ThrowIfNull(nameof(SramFile));

				var bytes = new byte[8192];
				SramFile.Save(new MemoryStream(bytes));

				await JsRuntime.StartDownloadAsync(CurrentFileName!, bytes);

				Changed = false;
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}
		}
	}
}
