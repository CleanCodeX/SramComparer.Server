using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using IO.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SRAM.Comparison;
using SRAM.SoE.Extensions;
using SRAM.SoE.Models;
using WebApp.SoE.Extensions;
using WebApp.SoE.Helpers;
using WebApp.SoE.Properties;
using WebApp.SoE.Shared.Enums;

#pragma warning disable 8509

namespace WebApp.SoE.ViewModels.Bases
{
	/// <summary>Base Viewmodel for loading SoE S-RAM files</summary>
	public abstract class LoadViewModelBase : ViewModelBase
	{
		protected bool IsSavestate { get; private set; }
		protected SramFileSoE? SramFile { get; set; }

		public bool IsError { get; protected set; }
		public bool IsLoading { get; set; }
		public MarkupString OutputMessage { get; set; }
		
		public new MandatorySaveSlotId CurrentFileSaveSlot 
		{
			get => (MandatorySaveSlotId) base.CurrentFileSaveSlot;
			set => base.CurrentFileSaveSlot = (SaveSlotId) value;
		}

		public virtual bool CanLoad => !IsLoading && CurrentFileStream is not null;
		public bool IsLoaded => SramFile is not null;
		public bool ShowOutput => OutputMessage.ToString() != string.Empty;

		public override async Task SetCurrentFileAsync(IBrowserFile file)
		{
			await base.SetCurrentFileAsync(file);
			
			Load();
		}

		public virtual MarkupString GetCurrentSaveslotChecksumStatus()
		{
			if (!IsLoaded) return Resources.NotSet.ColorText(Color.Cyan).ToMarkup();

			return SaveslotChecksumStatusFormatter.GetSaveslotChecksumStatus(CurrentFileSaveSlot.ToInt() - 1, SramFile!);
		}

		private void Load()
		{
			try
			{
				CanLoad.ThrowIfFalse(nameof(CanLoad));
				CurrentFileStream.ThrowIfNull(nameof(CurrentFileStream));

				(IsError, IsLoading) = (false, true);

				IsSavestate = Path.GetExtension(CurrentFileName)!.ToLower() == ".srm";

				CurrentFileStream.Position = 0;
				CurrentFileStream = ConvertSavestateStreamToSrm(CurrentFileStream);

				SramFile = new(CurrentFileStream, GameRegion);
				
				if(!IsSavestate)
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

			if (Options.CurrentFileSaveSlot > 0)
				CurrentFileSaveSlot = (MandatorySaveSlotId) base.CurrentFileSaveSlot;
			else
				CurrentFileSaveSlot = MandatorySaveSlotId.One;
		}

		protected internal override Task SaveOptionsAsync()
		{
			base.CurrentFileSaveSlot = (SaveSlotId)CurrentFileSaveSlot;

			return base.SaveOptionsAsync();
		}

		private Stream ConvertSavestateStreamToSrm(Stream stream)
		{
			stream.ThrowIfNull(nameof(stream));

			return !IsSavestate 
				? stream : 
				stream.ReadSramFromSavestate(GameRegion).ToStream();
		}

		public virtual Stream ConvertSrmStreamToSavestate(IOptions options, Stream srmStream, Stream savestateStream)
		{
			srmStream.ThrowIfNull(nameof(srmStream));

			if (!IsSavestate) return srmStream;

			savestateStream.ThrowIfNull(nameof(savestateStream));

			return srmStream.WriteSramToSavestate(GameRegion, savestateStream.GetBytes()).ToStream();
		}
	}
}
