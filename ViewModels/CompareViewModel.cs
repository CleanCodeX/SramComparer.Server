using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
		public MarkupString OutputMessage { get; set; }
		public bool IsComparing { get; set; }
		public bool CanCompare => !IsComparing && CurrentFileStream is not null && ComparisonSramFileStream is not null;
		public bool ColorizeOutput { get; set; } = true;
		public bool ShowOutput => OutputMessage.ToString() != string.Empty;

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
			[Display(Name = nameof(Res.DontShow), ResourceType = typeof(Res))]
			None,
			[Display(Name = nameof(Res.ComparedSaveSlots), ResourceType = typeof(Res))]
			Compared,
			[Display(Name = nameof(Res.AllSaveSlots), ResourceType = typeof(Res))]
			All
		}

		public SaveSlotOption Checksum { get; set; }
		public SaveSlotOption Unknown12B { get; set; }

		public async Task CompareAsync()
		{
			try
			{
				CanCompare.ThrowIfFalse(nameof(CanCompare));

				IsComparing = true;

				await SaveOptionsAsync();

				using var output = new StringWriter { NewLine = "<br>" };

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));
				Requires.NotNull(ComparisonSramFileStream, nameof(ComparisonSramFileStream));

				CurrentFileStream.Position = 0;
				ComparisonSramFileStream.Position = 0;

				new CommandHandlerSoE(ColorizeOutput ? new HtmlConsolePrinterSoE() : new ConsolePrinter()).Compare(CurrentFileStream, ComparisonSramFileStream, Options, output);

				OutputMessage = output.ToString().ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}

			IsComparing = false;
		}

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();

			ComparisonSramFileSaveSlot = (SaveSlotId)Options.ComparisonSramFileSaveSlot;

			if (Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ChecksumComparedSlots))
				Checksum = Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.ChecksumAllSlots)
					? SaveSlotOption.All
					: SaveSlotOption.Compared;

			if (Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12BComparedSlots))
				Unknown12B = Options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12BAllSlots)
					? SaveSlotOption.All
					: SaveSlotOption.Compared;
		}

		protected internal override Task SaveOptionsAsync()
		{
			Options.ComparisonSramFileSaveSlot = ComparisonSramFileSaveSlot.ToInt();

			Options.ComparisonFlags = Options.ComparisonFlags & ~ComparisonFlagsSoE.ChecksumAllSlots;
			if (Checksum != default)
				Options.ComparisonFlags = Checksum switch
				{
					SaveSlotOption.All => Options.ComparisonFlags |= ComparisonFlagsSoE.ChecksumAllSlots,
					SaveSlotOption.Compared => Options.ComparisonFlags |= ComparisonFlagsSoE.ChecksumComparedSlots,
				};

			Options.ComparisonFlags = Options.ComparisonFlags & ~ComparisonFlagsSoE.Unknown12BAllSlots;
			if (Unknown12B != default)
				Options.ComparisonFlags = Unknown12B switch
				{
					SaveSlotOption.All => Options.ComparisonFlags |= ComparisonFlagsSoE.Unknown12BAllSlots,
					SaveSlotOption.Compared => Options.ComparisonFlags |= ComparisonFlagsSoE.Unknown12BComparedSlots,
				};

			return base.SaveOptionsAsync();
		}
	}
}
