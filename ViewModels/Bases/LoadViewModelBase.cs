using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SramFormat.SoE;
using WebApp.SoE.Extensions;
using WebApp.SoE.Shared.Enums;

#pragma warning disable 8509

namespace WebApp.SoE.ViewModels.Bases
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public abstract class LoadViewModelBase : ViewModelBase
	{
		protected SramFileSoE? SramFile { get; set; }

		public bool IsLoading { get; set; }
		public virtual bool CanLoad => !IsLoading && CurrentFileStream is not null;
		public MarkupString OutputMessage { get; set; }
		public new MandatoryGameId CurrentSramSaveSlot { get; set; } = MandatoryGameId.One;
		public bool IsLoaded => SramFile is not null;
		public bool ShowOutput => OutputMessage.ToString() != string.Empty;
		
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

				IsLoading = true;

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));

				CurrentFileStream.Position = 0;

				SramFile = new SramFileSoE(CurrentFileStream, GameRegion);
				CurrentFileStream = null;
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}

			IsLoading = false;
		}

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();

			if(Options.CurrentSramFileSaveSlot > 0)
				CurrentSramSaveSlot = (MandatoryGameId)base.CurrentSramSaveSlot;
		}

		protected internal override Task SaveOptionsAsync()
		{
			base.CurrentSramSaveSlot = (SaveSlotId)CurrentSramSaveSlot;

			return base.SaveOptionsAsync();
		}
	}
}
