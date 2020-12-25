using System;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SramComparer.SoE;
using SramFormat.SoE.Models.Enums;
using WebApp.SoE.Shared.Enums;

namespace WebApp.SoE.ViewModels.Bases
{
	public abstract class ViewModelBase
	{
#nullable disable
		[Inject] public ProtectedLocalStorage LocalStorage { get; set; }
#nullable restore

		public Options Options { get; private set; } = new();
		public FileRegion Region { get; set; } = FileRegion.UnitedStates;
		public GameId CurrentGame { get; set; }
		protected MemoryStream? CurrentFileStream { get; set; }
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
			}
			
			Region = Options.Region;
			CurrentGame = (GameId)Options.CurrentGame;
		}

		protected internal virtual async Task SaveOptionsAsync()
		{
			Options.Region = Region;
			Options.CurrentGame = CurrentGame.ToInt();

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
			CurrentFileName = file.Name;
			CurrentFileStream = await file.OpenReadStream().CopyAsMemoryStreamAsync();
		}
	}
}
