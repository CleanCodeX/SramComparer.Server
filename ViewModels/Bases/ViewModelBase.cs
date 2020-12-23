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

		protected internal virtual async Task LoadOptionsAsync()
		{
			Options = (await LocalStorage.GetAsync<Options>(nameof(Options))).Value ?? Options;
			Region = Options.Region;
			CurrentGame = (GameId)Options.CurrentGame;
		}

		protected internal virtual async Task SaveOptionsAsync()
		{
			Options.Region = Region;
			Options.CurrentGame = CurrentGame.ToInt();

			await LocalStorage.SetAsync(nameof(Options), Options);
		}

		public virtual async Task SetCurrentFileAsync(IBrowserFile file)
		{
			CurrentFileName = file.Name;
			CurrentFileStream = await file.OpenReadStream().CopyAsMemoryStreamAsync();
		}
	}
}
