using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SramFormat.SoE;
using WebApp.SoE.Extensions;
using WebApp.SoE.Shared.Enums;
using SavestateFormat.Snes9x.Extensions;
using SramComparer.SoE.Extensions;
using SramFormat.SoE.Constants;

#pragma warning disable 8509

namespace WebApp.SoE.ViewModels.Bases
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public abstract class LoadViewModelBase : ViewModelBase
	{
		protected bool IsSavestate { get; private set; }
		protected SramFileSoE? SramFile { get; set; }
		
		public bool IsLoading { get; set; }
		public virtual bool CanLoad => !IsLoading && CurrentFileStream is not null;
		public MarkupString OutputMessage { get; set; }
		public new MandatoryGameId CurrentFileSaveSlot { get; set; } = MandatoryGameId.One;
		public bool IsLoaded => SramFile is not null;
		public bool ShowOutput => OutputMessage.ToString() != string.Empty;
		public bool IsError { get; protected set; }
		
		public override async Task SetCurrentFileAsync(IBrowserFile file)
		{
			await base.SetCurrentFileAsync(file);
			
			Load();
		}

		private void Load()
		{
			try
			{
				CanLoad.ThrowIfFalse(nameof(CanLoad));

				IsError = false;
				IsLoading = true;

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));

				CurrentFileStream.Position = 0;
				CurrentFileStream = ConvertStreamIfSaveState(CurrentFileStream, CurrentFileName!);

				SramFile = new SramFileSoE(CurrentFileStream, GameRegion);
				CurrentFileStream = null;
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
				IsError = true;
			}

			IsLoading = false;
		}

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();

			if(Options.CurrentFileSaveSlot > 0)
				CurrentFileSaveSlot = (MandatoryGameId)base.CurrentFileSaveSlot;
		}

		protected internal override Task SaveOptionsAsync()
		{
			base.CurrentFileSaveSlot = (SaveSlotId)CurrentFileSaveSlot;

			return base.SaveOptionsAsync();
		}

		private Stream ConvertStreamIfSaveState(Stream stream, string filePath)
		{
			IsSavestate = false;

			var fileExtension = Path.GetExtension(filePath).ToLower();
			if (fileExtension == ".srm") return stream;

			var result = stream.ConvertSnes9xSavestateToSram().GetStreamSlice(Sizes.Sram);

			IsSavestate = true;

			return result;
		}
	}
}
