using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using IO.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Enums;
using SRAM.Comparison.SoE.Services;
using WebApp.SoE.Extensions;
using WebApp.SoE.Services;
using WebApp.SoE.Shared.Enums;
using WebApp.SoE.ViewModels.Bases;
using Res = WebApp.SoE.Properties.Resources;
using ResComp = SRAM.Comparison.Properties.Resources;
// ReSharper disable ValueParameterNotUsed
#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Viewmodel for SoE S-RAM comparison</summary>
	public class ComparisonViewModel : ViewModelBase
	{
		public enum SaveSlotOption : uint
		{
			[Display(Name = nameof(Res.EnumDontShow), ResourceType = typeof(Res))]
			None,
			[Display(Name = nameof(Res.EnumComparedSaveSlotsIfDifferent), ResourceType = typeof(Res))]
			IfComparedAndDifferent,
			[Display(Name = nameof(Res.EnumComparedSaveSlots), ResourceType = typeof(Res))]
			IfCompared
		}

		public override SaveSlotId CurrentFileSaveSlot
		{
			get => base.CurrentFileSaveSlot;
			set
			{
				if (base.CurrentFileSaveSlot == value) return;

				base.CurrentFileSaveSlot = value;

				if (ComparisonFileSaveSlot != SaveSlotId.All)
					if (value == SaveSlotId.All || value == ComparisonFileSaveSlot)
						ComparisonFileSaveSlot = SaveSlotId.All;

				StartComparison();
			}
		}

		private SaveSlotId _comparisonFileSaveSlot;
		public SaveSlotId ComparisonFileSaveSlot
		{
			get => _comparisonFileSaveSlot;
			set
			{
				if (_comparisonFileSaveSlot == value) return;

				if (CurrentFileSaveSlot == default && value != default)
				{
					OutputMessage = ResComp.ErrorCompSaveSlotSetButNotForCurrFile.ColorText(Color.Red).ToMarkup();
					return;
				}
					
				_comparisonFileSaveSlot = value;
				StartComparison();
			}
		}

		public MemoryStream? ComparisonFileStream { get; set; }
		public string? ComparisonFileName { get; set; }
		public MarkupString OutputMessage { get; set; }
		public bool IsBusy { get; set; }
		public bool CanCompare => !IsBusy && CurrentFileStream is not null && ComparisonFileStream is not null;
		public bool CanShowSummary => !IsBusy && CurrentFileStream is not null && CurrentFileSaveSlot != SaveSlotId.All;
		public bool ColorizeOutput { get; set; } = true;

#pragma warning disable 4014
		private void StartComparison()
		{
			if (!CanCompare) return;

			//CompareAsync(); // TODO Currently buggy. Changes will only be updated the next time
		}
#pragma warning restore 4014

		public bool ShowOutput => OutputMessage.ToString() != string.Empty;
		public bool IsError { get; private set; }

		public bool SlotByteComparison
		{
			get => Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.SlotByteComparison);
			set
			{
				Options.ComparisonFlags = Options.ComparisonFlags.InvertUInt32Flags(ComparisonFlagsSoE.SlotByteComparison);
				StartComparison();
			}
		}

		public bool NonSlotComparison
		{
			get => Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.NonSlotComparison);
			set
			{
				Options.ComparisonFlags = Options.ComparisonFlags.InvertUInt32Flags(ComparisonFlagsSoE.NonSlotComparison);
				StartComparison();
			}
		}

		public bool ChecksumStatus
		{
			get => Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ChecksumStatus);
			set
			{
				Options.ComparisonFlags = Options.ComparisonFlags.InvertUInt32Flags(ComparisonFlagsSoE.ChecksumStatus);
				StartComparison();
			}
		}

		private SaveSlotOption _checksum;
		public SaveSlotOption Checksum
		{
			get => _checksum;
			set
			{
				if (_checksum == value) return;
				_checksum = value;
				StartComparison();
			}
		}

		private SaveSlotOption _scriptedEventTimer;
		public SaveSlotOption ScriptedEventTimer
		{
			get => _scriptedEventTimer;
			set
			{
				if (_scriptedEventTimer == value) return;
				_scriptedEventTimer = value;
				StartComparison();
			}
		}

		public override async Task SetCurrentFileAsync(IBrowserFile file)
		{
			await base.SetCurrentFileAsync(file);
			StartComparison();
		}

		public async Task SetComparisonFileAsync(IBrowserFile file)
		{
			CheckFileExtension(file.Name);

			ComparisonFileName = file.Name;
			ComparisonFileStream = await file.OpenReadStream().CopyAsMemoryStreamAsync();
			StartComparison();
		}

		public async Task CompareAsync()
		{
			try
			{
				CanCompare.ThrowIfFalse(nameof(CanCompare));

				IsError = false;
				IsBusy = true;

				await SaveOptionsAsync();

				await using var output = new StringWriter {NewLine = "<br>"};

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));
				Requires.NotNull(ComparisonFileStream, nameof(ComparisonFileStream));

				CurrentFileStream.Position = 0;
				ComparisonFileStream.Position = 0;

				Options.CurrentFilePath = CurrentFileName;
				Options.ComparisonPath = ComparisonFileName;

				var commandHandler = new CommandHandlerSoE(ColorizeOutput ? new HtmlConsolePrinterSoE() : new ConsolePrinter());
				commandHandler.Compare(CurrentFileStream, ComparisonFileStream, Options, output);

				OutputMessage = output.ToString().ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsBusy = false;
		}

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

				var commandHandler = new CommandHandlerSoE(ColorizeOutput ? new HtmlConsolePrinterSoE() : new ConsolePrinter());
				var summary = commandHandler.GetSummary(CurrentFileStream, Options);

				OutputMessage = summary.ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsBusy = false;
		}

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();

			ComparisonFileSaveSlot = (SaveSlotId)Options.ComparisonFileSaveSlot;

			if (Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ChecksumIfDifferent))
				Checksum = Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Checksum)
					? SaveSlotOption.IfCompared
					: SaveSlotOption.IfComparedAndDifferent;

			if (Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ScriptedEventTimerIfDifferent))
				ScriptedEventTimer = Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ScriptedEventTimer)
					? SaveSlotOption.IfCompared
					: SaveSlotOption.IfComparedAndDifferent;
		}

		protected internal override Task SaveOptionsAsync()
		{
			Options.ComparisonFileSaveSlot = ComparisonFileSaveSlot.ToInt();

			Options.ComparisonFlags &= ~ComparisonFlagsSoE.Checksum;
			if (Checksum != default)
				Options.ComparisonFlags = Checksum switch
				{
					SaveSlotOption.IfCompared => Options.ComparisonFlags |= ComparisonFlagsSoE.Checksum,
					SaveSlotOption.IfComparedAndDifferent => Options.ComparisonFlags |= ComparisonFlagsSoE.ChecksumIfDifferent,
				};

			Options.ComparisonFlags &= ~ComparisonFlagsSoE.ScriptedEventTimer;
			if (ScriptedEventTimer != default)
				Options.ComparisonFlags = ScriptedEventTimer switch
				{
					SaveSlotOption.IfCompared => Options.ComparisonFlags |= ComparisonFlagsSoE.ScriptedEventTimer,
					SaveSlotOption.IfComparedAndDifferent => Options.ComparisonFlags |= ComparisonFlagsSoE.ScriptedEventTimerIfDifferent,
				};

			return base.SaveOptionsAsync();
		}
	}
}
