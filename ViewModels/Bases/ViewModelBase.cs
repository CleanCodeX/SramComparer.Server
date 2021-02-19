using System;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SoE.Models.Enums;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.SoE;
using WebApp.SoE.Properties;
using WebApp.SoE.Shared.Enums;

namespace WebApp.SoE.ViewModels.Bases
{
	public abstract class ViewModelBase
	{
#nullable disable
		[Inject] public ProtectedLocalStorage LocalStorage { get; set; }
#nullable restore

		public Options Options { get; private set; } = new();
		public GameRegion GameRegion { get; set; }
		public virtual SaveSlotId CurrentFileSaveSlot { get; set; }
		protected Stream? CurrentFileStream { get; set; }
		public string? CurrentFileName { get; set; }

		protected virtual string StorageKeyPrefix => GetType().Name + "_" ;
		private string StorageKey => StorageKeyPrefix + nameof(Options);
		
		protected internal virtual async Task LoadOptionsAsync()
		{
			try
			{
				if(await LocalStorage.GetAsync<Options>(StorageKey) is {Success: true} settings)
					Options = settings.Value!;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				await LocalStorage.DeleteAsync(StorageKey);
			}
			
			GameRegion = Options.GameRegion;
			CurrentFileSaveSlot = (SaveSlotId)Options.CurrentFileSaveSlot;
		}

		protected internal virtual async Task SaveOptionsAsync()
		{
			Options.GameRegion = GameRegion;
			Options.CurrentFileSaveSlot = CurrentFileSaveSlot.ToInt();

			try
			{
				await LocalStorage.SetAsync(StorageKey, Options);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public virtual async Task SetCurrentFileAsync(IBrowserFile file)
		{
			CheckFileExtension(file.Name);

			CurrentFileName = file.Name;
			CurrentFileStream = await file.OpenReadStream().CopyAsMemoryStreamAsync();
		}

		protected static void CheckFileExtension(string fileName)
		{
			if (FilePathHelper.IsSaveFile(fileName)) return;

			throw new ArgumentException(Resources.ErrorUnsupportedFileExtensionTemplate.InsertArgs(fileName));
		}
	}
}
