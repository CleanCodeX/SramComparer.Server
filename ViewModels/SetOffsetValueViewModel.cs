using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SramCommons.Extensions;
using SramComparer.Helpers;
using SramComparer.Properties;
using WebServer.SoE.Extensions;

#pragma warning disable 8509

namespace WebServer.SoE.ViewModels
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public class SetOffsetValueViewModel : GetOffsetValueViewModel
	{
#nullable disable
		[Inject] internal IJSRuntime JsRuntime { get; set; }
#nullable restore

		public bool CanSave { get; set; }

		public void SetOffsetValue()
		{
			try
			{
				CanSave = false;

				SramFile.ThrowIfNull(nameof(SramFile));
				SramFile.SetOffsetValue(Options.CurrentGame - 1, Offset, (byte)OffsetValue);
				var valueDisplayText = NumberFormatter.GetByteValueRepresentations((byte)OffsetValue);

				OutputMessage = Resources.StatusSetOffsetValueTemplate.InsertArgs(Offset, valueDisplayText).ColorText(Color.Green).ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}

			CanSave = true;
		}

		public async Task SaveAndDownloadAsync()
		{
			try
			{
				SramFile.ThrowIfNull(nameof(SramFile));
				CanSave = false;
				
				var bytes = new byte[8192];
				SramFile.Save(new MemoryStream(bytes));

				await JsRuntime.StartDownloadAsync(CurrentFileName!, bytes);
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}

			CanSave = true;
		}
	}
}
