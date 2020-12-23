using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using SramCommons.Extensions;
using SramComparer.Helpers;
using SramComparer.Properties;
using WebServer.SoE.Extensions;
using WebServer.SoE.ViewModels.Bases;

#pragma warning disable 8509

namespace WebServer.SoE.ViewModels
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public class GetOffsetValueViewModel : LoadViewModelBase
	{
		public int OffsetAddress { get; set; }
		public int OffsetValue { get; set; }

		public bool CanGet => IsLoaded && OffsetAddress > 0;
		public bool CanSet => IsLoaded && OffsetAddress > 0;

		protected internal override async Task LoadOptionsAsync()
		{
			await base.LoadOptionsAsync();
			OffsetAddress = (await LocalStorage.GetAsync<int>(nameof(OffsetAddress))).Value;
		}

		protected internal override async Task SaveOptionsAsync()
		{
			await base.SaveOptionsAsync();
			await LocalStorage.SetAsync(nameof(OffsetAddress), OffsetAddress);
		}

		public override async Task SetCurrentFileAsync(IBrowserFile file)
		{
			await base.SetCurrentFileAsync(file);

			if (OffsetAddress > 0)
				await GetOffsetValueAsync();
		}

		public async Task GetOffsetValueAsync()
		{
			try
			{
				SramFile.ThrowIfNull(nameof(SramFile));
				OffsetValue = SramFile.GetOffsetByte(Options.CurrentGame - 1, OffsetAddress);
				var valueDisplayText = NumberFormatter.GetByteValueRepresentations((byte)OffsetValue);

				OutputMessage = Resources.StatusGetOffsetValueTemplate.InsertArgs(OffsetAddress, valueDisplayText).ColorText(Color.Green).ToMarkup();

				await SaveOptionsAsync();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
