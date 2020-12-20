using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using SramComparer.SoE;
using SramFormat.SoE.Models.Enums;
using WebServer.SoE.Shared.Enums;

namespace WebServer.SoE.ViewModels.Bases
{
	public abstract class ViewModelBase
	{
		public Options Options { get; } = new();
		public FileRegion Region { get; set; } = FileRegion.UnitedStates;
		public GameId CurrentGame { get; set; }
		protected MemoryStream? CurrentFileStream { get; set; }
		public string? CurrentFileName { get; set; }

		public virtual async Task SetCurrentFileAsync(IBrowserFile file)
		{
			CurrentFileName = file.Name;
			CurrentFileStream = await file.OpenReadStream().CopyAsMemoryStreamAsync();
		}
	}
}
