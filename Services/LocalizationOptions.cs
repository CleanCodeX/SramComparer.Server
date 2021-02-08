namespace WebApp.SoE.Services
{
	public class LocalizationOptions
	{
		public string? TargetCultureInitText { get; set; }
		public string? TargetCulture { get; set; }
		public bool HideEnglish { get; set; }

		public static LocalizationOptions Create(string? targetCulture = null) => new() { TargetCulture = targetCulture};
	}
}