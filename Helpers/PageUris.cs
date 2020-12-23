namespace WebServer.SoE.Helpers
{
	internal static class PageUris
	{
		public const string Compare = "/compare";
		public const string Offset = "/offset";

		public const string Embed = "/embed";
		private const string EmbedPrefix = Embed + "?p=";

		public const string Downloads = EmbedPrefix + "downloads";
		public const string Features = EmbedPrefix + "features";
		public const string HowToUse = EmbedPrefix + "howtouse";
		public const string Imagery = EmbedPrefix + "imagery";
		public const string Goal = EmbedPrefix + "goal";
		public const string Unknowns = EmbedPrefix + "unknowns";
		public const string HowCanIHelp = EmbedPrefix + "howcanihelp";
		public const string ChangeLog = EmbedPrefix + "changelog";
		public const string Awesome = EmbedPrefix + "awesome";
		public const string GitHub = EmbedPrefix + "github";
		public const string Support = EmbedPrefix + "support";
		public const string Community = EmbedPrefix + "community";
		public const string SramDocu = EmbedPrefix + "sramdocu";
	}
}
