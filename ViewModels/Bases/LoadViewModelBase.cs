using System;
using System.Drawing;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SramComparer.Server.Extensions;
using SramComparer.Server.Shared;
using SramComparer.Server.Shared.Enums;
using SramFormat.SoE;

#pragma warning disable 8509

namespace SramComparer.Server.ViewModels.Bases
{
	/// <summary>Base Viewmodel for loading SoE SRAM files</summary>
	public abstract class LoadViewModelBase : ViewModelBase
	{
		protected SramFileSoE? SramFile { get; set; }

		public bool IsLoading { get; set; }
		public virtual bool CanLoad => !IsLoading && CurrentFileStream is not null;
		public MarkupString OutputMessage { get; set; }
		public new MandatoryGameId CurrentGame { get; set; } = MandatoryGameId.One;
		public bool IsLoaded => SramFile is not null;

		public override async Task SetCurrentFileAsync(IBrowserFile file)
		{
			await base.SetCurrentFileAsync(file);
			
			Load();
		}

		public void Load()
		{
			try
			{
				CanLoad.ThrowIfFalse(nameof(CanLoad));

				IsLoading = true;

				SetOptions();

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));

				CurrentFileStream.Position = 0;

				SramFile = new SramFileSoE(CurrentFileStream, Options.Region);
				CurrentFileStream = null;
			}
			catch (Exception ex)
			{
				OutputMessage = ex.Message.ColorText(Color.Red).ToMarkup();
			}

			IsLoading = false;
		}

		private void SetOptions()
		{
			Options.Region = Region;
			Options.CurrentGame = CurrentGame.ToInt();
		}
	}
}
