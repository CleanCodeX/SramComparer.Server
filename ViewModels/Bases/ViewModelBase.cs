using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.AspNetCore.Components.Forms;
using SramComparer.Server.Shared;
using SramComparer.Server.Shared.Enums;
using SramComparer.SoE;
using SramFormat.SoE.Models.Enums;

namespace SramComparer.Server.ViewModels.Bases
{
	public abstract class ViewModelBase
	{
		public Options Options { get; } = new();
		public FileRegion Region { get; set; } = FileRegion.UnitedStates;
		public GameId CurrentGame { get; set; }
		public IEnumerable<FileRegion> FileRegions { get; } = default(FileRegion).GetValues();
		protected MemoryStream? CurrentFileStream { get; set; }
		public string? CurrentFileName { get; set; }

		public virtual async Task SetCurrentFileAsync(IBrowserFile file)
		{
			CurrentFileName = file.Name;
			CurrentFileStream = await file.OpenReadStream().CopyAsMemoryStreamAsync();
		}
	}
}
