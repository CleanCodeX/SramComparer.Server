using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using SramCommons.Extensions;
using SramComparer.Helpers;
using SramComparer.Properties;
using WebApp.SoE.Extensions;
using WebApp.SoE.ViewModels.Bases;

#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public class GetOffsetValueViewModel : LoadViewModelBase
	{
		public int OffsetAddress { get; set; }
		public int OffsetValue { get; set; }

		public bool CanGet => IsLoaded && OffsetAddress > 0;
		public bool CanSet => IsLoaded && OffsetAddress > 0;
		
		private string StorageKey => StorageKeyPrefix + nameof(OffsetAddress);

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();
			OffsetAddress = (await LocalStorage.GetAsync<int>(StorageKey)).Value;
		}

		protected internal override async Task SaveOptionsAsync()
		{
			await base.SaveOptionsAsync();
			await LocalStorage.SetAsync(StorageKey, OffsetAddress);
		}

		public override async Task SetCurrentFileAsync(IBrowserFile file)
		{
			await base.SetCurrentFileAsync(file);

			if (OffsetAddress > 0)
				await GetOffsetValueAsync();
		}

		public async Task GetOffsetValueAsync()
		{
			InternalGetOffsetValue();
			await SaveOptionsAsync();
		}

		private void InternalGetOffsetValue()
		{
			try
			{
				CurrentFileSaveSlot.ThrowIfDefault(nameof(CurrentFileSaveSlot));
				SramFile.ThrowIfNull(nameof(SramFile));

				IsError = false;
				OffsetValue = SramFile.GetOffsetByte(CurrentFileSaveSlot.ToInt() - 1, OffsetAddress);
				var valueDisplayText = NumberFormatter.GetByteValueRepresentations((byte) OffsetValue);

				OutputMessage = Resources.StatusGetOffsetValueTemplate.InsertArgs(OffsetAddress, valueDisplayText)
					.ColorText(Color.Green).ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}
		}
	}
}
