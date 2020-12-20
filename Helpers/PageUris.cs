namespace SramComparer.Server.Helpers
{
	internal static class PageUris
	{
		public const string Compare = "/compare";
		public const string Offset = "/offset";
		
		private const string Embed = "/embed?p=";
		
		public const string Download = Embed + nameof(Download);
		public const string Features = Embed + nameof(Features);
		public const string HowToUse = Embed + nameof(HowToUse);
		public const string Imagery = Embed + nameof(Imagery);
		public const string WhatIsUnknown = Embed + nameof(WhatIsUnknown);
		public const string Unknowns = Embed + nameof(Unknowns);
		public const string HowCanIHelp = Embed + nameof(HowCanIHelp);
		public const string Changelog = Embed + nameof(Changelog);
		public const string Awesome = Embed + nameof(Awesome);
		public const string GitHub = Embed + nameof(GitHub);
		public const string Forums = Embed + nameof(Forums);
		public const string SramDocu = Embed + nameof(SramDocu);
	}
}
