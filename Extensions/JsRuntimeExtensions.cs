using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using Microsoft.JSInterop;
using Res = WebApp.SoE.Properties.Resources;

namespace WebApp.SoE.Extensions
{
	public static class JsRuntimeExtensions
	{
		public static async Task<bool> StartDownloadAsync(this IJSRuntime source, string filename, byte[] content)
		{
			var result = await source.InvokeAsync<bool>("confirm", Res.DownloadConfirmationFileTemplate.InsertArgs(filename));
			if (!result) return false;

			await source.InvokeVoidAsync(
				"downloadFromByteArray",
				new
				{
					ByteArray = content,
					FileName = filename,
					ContentType = "application/octet-stream"
				});

			return true;
		}
	}
}
