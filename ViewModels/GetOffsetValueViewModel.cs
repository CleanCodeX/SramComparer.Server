using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using IO.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Properties;
using WebApp.SoE.Extensions;
using WebApp.SoE.ViewModels.Bases;

#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Base Viewmodel for loading SoE S-RAM files</summary>
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
			try
			{
				OffsetAddress = (await LocalStorage.GetAsync<int>(StorageKey)).Value;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				await LocalStorage.DeleteAsync(StorageKey);
			}
		}

		protected internal override async Task SaveOptionsAsync()
		{
			await base.SaveOptionsAsync();

			try
			{
				await LocalStorage.SetAsync(StorageKey, OffsetAddress);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
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
				var valueDisplayText = NumberFormatter.FormatDecHexBin((byte)OffsetValue);

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
