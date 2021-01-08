using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using Microsoft.AspNetCore.Components;
using SramComparer.Helpers;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Services;
using WebApp.SoE.Extensions;
using WebApp.SoE.Services;
using WebApp.SoE.Shared.Enums;
using WebApp.SoE.ViewModels.Bases;
using Res = WebApp.SoE.Properties.Resources;
#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Viewmodel for SoE SRAM comparison</summary>
	public class CompareViewModel : ViewModelBase
	{
		public SaveSlotId ComparisonSramFileSaveSlot { get; set; }
		public MemoryStream? ComparisonSramFileStream { get; set; }
		public string? ComparisonFileName { get; set; }
		public MarkupString OutputMessage { get; set; }
		public bool IsComparing { get; set; }
		public bool CanCompare => !IsComparing && CurrentFileStream is not null && ComparisonSramFileStream is not null;
		public bool ColorizeOutput { get; set; } = true;
		public bool ShowOutput => OutputMessage.ToString() != string.Empty;
		public bool IsError { get; private set; }

		public bool SlotByteByByteComparison
		{
			get => Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.SlotByteByByteComparison);
			set => Options.ComparisonFlags = (ComparisonFlagsSoE)EnumHelper.InvertUIntFlag(Options.ComparisonFlags, ComparisonFlagsSoE.SlotByteByByteComparison);
		}

		public bool NonSlotByteByByteComparison
		{
			get => Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.NonSlotByteByByteComparison);
			set => Options.ComparisonFlags = (ComparisonFlagsSoE)EnumHelper.InvertUIntFlag(Options.ComparisonFlags, ComparisonFlagsSoE.NonSlotByteByByteComparison);
		}

		public enum SaveSlotOption : uint
		{
			[Display(Name = nameof(Res.EnumDontShow), ResourceType = typeof(Res))]
			None,
			[Display(Name = nameof(Res.EnumComparedSaveSlotsIfDifferent), ResourceType = typeof(Res))]
			ComparedIfDifferent,
			[Display(Name = nameof(Res.EnumComparedSaveSlots), ResourceType = typeof(Res))]
			Compared
		}

		public SaveSlotOption Checksum { get; set; }
		public SaveSlotOption Unknown12B { get; set; }

		public async Task CompareAsync()
		{
			try
			{
				CanCompare.ThrowIfFalse(nameof(CanCompare));

				IsError = false;
				IsComparing = true;

				await SaveOptionsAsync();

				await using var output = new StringWriter {NewLine = "<br>"};

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));
				Requires.NotNull(ComparisonSramFileStream, nameof(ComparisonSramFileStream));

				CurrentFileStream.Position = 0;
				ComparisonSramFileStream.Position = 0;

				Options.CurrentSramFilepath = CurrentFileName;
				Options.ComparisonSramFilepath = ComparisonFileName;

				new CommandHandlerSoE(ColorizeOutput ? new HtmlConsolePrinterSoE() : new ConsolePrinter()).Compare(
					CurrentFileStream, ComparisonSramFileStream, Options, output);

				OutputMessage = output.ToString().ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}

			IsComparing = false;
		}

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();

			ComparisonSramFileSaveSlot = (SaveSlotId)Options.ComparisonSramFileSaveSlot;

			if (Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ChecksumIfDifferent))
				Checksum = Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Checksum)
					? SaveSlotOption.Compared
					: SaveSlotOption.ComparedIfDifferent;

			if (Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12BIfDifferent))
				Unknown12B = Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12B)
					? SaveSlotOption.Compared
					: SaveSlotOption.ComparedIfDifferent;
		}

		protected internal override Task SaveOptionsAsync()
		{
			Options.ComparisonSramFileSaveSlot = ComparisonSramFileSaveSlot.ToInt();

			Options.ComparisonFlags = Options.ComparisonFlags & ~ComparisonFlagsSoE.Checksum;
			if (Checksum != default)
				Options.ComparisonFlags = Checksum switch
				{
					SaveSlotOption.Compared => Options.ComparisonFlags |= ComparisonFlagsSoE.Checksum,
					SaveSlotOption.ComparedIfDifferent => Options.ComparisonFlags |= ComparisonFlagsSoE.ChecksumIfDifferent,
				};

			Options.ComparisonFlags = Options.ComparisonFlags & ~ComparisonFlagsSoE.Unknown12B;
			if (Unknown12B != default)
				Options.ComparisonFlags = Unknown12B switch
				{
					SaveSlotOption.Compared => Options.ComparisonFlags |= ComparisonFlagsSoE.Unknown12B,
					SaveSlotOption.ComparedIfDifferent => Options.ComparisonFlags |= ComparisonFlagsSoE.Unknown12BIfDifferent,
				};

			return base.SaveOptionsAsync();
		}
	}
}
