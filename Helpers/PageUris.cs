namespace WebServer.SoE.Helpers
{
	internal static class PageUris
	{
		public const string Compare = "/compare";
		public const string Offset = "/offset";

		public const string Embed = "/embed";
		private const string EmbedPrefix = Embed + "?p=";

		public const string Downloads = EmbedPrefix + nameof(Downloads);
		public const string Features = EmbedPrefix + nameof(Features);
		public const string HowToUse = EmbedPrefix + nameof(HowToUse);
		public const string Imagery = EmbedPrefix + nameof(Imagery);
		public const string Goal = EmbedPrefix + nameof(Goal);
		public const string Unknowns = EmbedPrefix + nameof(Unknowns);
		public const string HowCanIHelp = EmbedPrefix + nameof(HowCanIHelp);
		public const string ChangeLog = EmbedPrefix + nameof(ChangeLog);
		public const string Awesome = EmbedPrefix + nameof(Awesome);
		public const string GitHub = EmbedPrefix + nameof(GitHub);
		public const string Support = EmbedPrefix + nameof(Support);
		public const string Forums = EmbedPrefix + nameof(Forums);
		public const string SramDocu = EmbedPrefix + nameof(SramDocu);
	}
}
