using System;
using System.Drawing;
using Common.Shared.Min.Extensions;
using SramCommons.Extensions;
using SramComparer.Helpers;
using SramComparer.Properties;
using SramComparer.Server.Extensions;
using SramComparer.Server.ViewModels.Bases;

#pragma warning disable 8509

namespace SramComparer.Server.ViewModels
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public class GetOffsetValueViewModel : LoadViewModelBase
	{
		public int Offset { get; set; }
		public int OffsetValue { get; set; }

		public bool CanGet => IsLoaded && Offset > 0;
		public bool CanSet => IsLoaded && Offset > 0;

		public void GetOffsetValue()
		{
			try
			{
				SramFile.ThrowIfNull(nameof(SramFile));
				OffsetValue = SramFile.GetOffsetByte(Options.CurrentGame - 1, Offset);
				var valueDisplayText = NumberFormatter.GetByteValueRepresentations((byte)OffsetValue);

				OutputMessage = Resources.StatusGetOffsetValueTemplate.InsertArgs(Offset, valueDisplayText).ColorText(Color.Green).ToMarkup();
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}
		}
	}
}
