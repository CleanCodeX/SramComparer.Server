namespace WebServer.SoE.Helpers
{
	internal static class PageUris
	{
		public const string Compare = "/compare";
		public const string Offset = "/offset";

		public const string Embed = "/embed";
		public const string EmbedPrefix = Embed + "?p=";

		public const string Download = EmbedPrefix + nameof(Download);
		public const string Features = EmbedPrefix + nameof(Features);
		public const string HowToUse = EmbedPrefix + nameof(HowToUse);
		public const string Imagery = EmbedPrefix + nameof(Imagery);
		public const string WhatIsUnknown = EmbedPrefix + nameof(WhatIsUnknown);
		public const string Unknowns = EmbedPrefix + nameof(Unknowns);
		public const string HowCanIHelp = EmbedPrefix + nameof(HowCanIHelp);
		public const string Changelog = EmbedPrefix + nameof(Changelog);
		public const string Awesome = EmbedPrefix + nameof(Awesome);
		public const string GitHub = EmbedPrefix + nameof(GitHub);
		public const string Forums = EmbedPrefix + nameof(Forums);
		public const string SramDocu = EmbedPrefix + nameof(SramDocu);
	}
}
