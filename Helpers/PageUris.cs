namespace WebApp.SoE.Helpers
{
	internal static class PageUris
	{
		public const string Compare = "/compare";
		public const string Offset = "/offset";

		public const string Page = "/_";
		private const string PagePrefix = Page + "?p=";

		public const string Downloads = PagePrefix + "downloads";
		public const string Features = PagePrefix + "features";
		public const string HowToUse = PagePrefix + "howtouse";
		public const string Imagery = PagePrefix + "imagery";
		public const string Goal = PagePrefix + "goal";
		public const string Unknowns = PagePrefix + "unknowns";
		public const string HowCanIHelp = PagePrefix + "howcanihelp";
		public const string ChangeLog = PagePrefix + "changelog";
		public const string Awesome = PagePrefix + "awesome";
		public const string GitHub = PagePrefix + "github";
		public const string Support = PagePrefix + "support";
		public const string Community = PagePrefix + "community";
		public const string SramDocu = PagePrefix + "sramdocu";
	}
}
