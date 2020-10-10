using System;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SramComparer.SoE.Server.Extensions;
using SramComparer.SoE.Services;
using SramFormat.SoE.Models.Enums;

namespace SramComparer.SoE.Server.Pages
{
	public partial class CompareSoE
	{
		private Options Options { get; } = new Options();
		private FileRegion FileRegion { get; set; } = FileRegion.UnitedStates;
		private MemoryStream? CurrentFileStream { get; set; }
		private MemoryStream? ComparisonFileStream { get; set; }
		private MarkupString ComparisonResult { get; set; }
		private bool IsComparing { get; set; }
		private bool CompareButtonDisabled => !IsComparing && (CurrentFileStream is null || ComparisonFileStream is null);

		private void Compare()
		{
			IsComparing = true;

			try
			{
				Options.Region = FileRegion;

				var output = new StringWriter { NewLine = "<br>" };

				Requires.NotNull(CurrentFileStream, nameof(CurrentFileStream));
				Requires.NotNull(ComparisonFileStream, nameof(ComparisonFileStream));
				
				CurrentFileStream.Position = 0;
				ComparisonFileStream.Position = 0;
				
				new CommandHandlerSoE().Compare(CurrentFileStream, ComparisonFileStream, Options, output);

				ComparisonResult = (MarkupString)output.ToString().FormatComparisonText();
			}
			catch (Exception ex)
			{
				ComparisonResult = (MarkupString)ex.Message;
			}

			IsComparing = false;
		}

		private async Task OnCurrentFileChange(InputFileChangeEventArgs arg) => CurrentFileStream = await arg.File.OpenReadStream().CopyAsMemoryStreamAsync();

		private async Task OnComparisonFileChange(InputFileChangeEventArgs arg) => ComparisonFileStream = await arg.File.OpenReadStream().CopyAsMemoryStreamAsync();
	}
}
