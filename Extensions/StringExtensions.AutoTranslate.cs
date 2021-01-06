using Common.Shared.Min.Extensions;

namespace WebApp.SoE.Extensions
{
	/// <summary>Fixes common errors of auto translation</summary>
	public static partial class StringExtensions
	{
		public static string FixAutoTranslatedText(this string source) =>
			source
				.Replace("] (", "](")

				#region HTML link correction

					.Replace("> ", ">")
					.Replace(" </a>", "</a>")

				#endregion HTML link correction

				.Remove("</ span>")!;
	}
}
