namespace WebApp.SoE.Helpers
{
	internal static class PageUris
	{
		public const string BrowserCheck = "/" + nameof(BrowserCheck);
		public const string Unsupported = "/" + nameof(Unsupported);
		public const string Index = "/";

		public const string Comparison = "/" + nameof(Comparison);

		public const string Offset = "/" + nameof(Offset);
		public const string OffsetEditing = Offset + "/editing";

		public const string SlotSummary = "/" + nameof(SlotSummary);
		public const string TerminalCodes = "/" + nameof(TerminalCodes);

		public const string About = "/" + nameof(About);
		public const string Contributors = "/" + nameof(Contributors);
		public const string Contributor = "/" + nameof(Contributor);
		public const string Localizing = "/" + nameof(Localizing);
		public const string Features = "/" + nameof(Features);

		public const string Tutorials = "/" + nameof(Tutorials);

		public const string Guides = "/" + nameof(Guides);
		public const string GuideSrm = Guides + "/Srm";
		public const string GuideSnes9x = Guides + "/Snes9x";

		public const string FAQ = "/" + nameof(FAQ);
		public const string Glossary = "/" + nameof(Glossary);
		public const string Imagery = "/" + nameof(Imagery);
		public const string Prerequisites = "/" + nameof(Prerequisites);
		public const string Goals = "/" + nameof(Goals);
		public const string Unknowns = "/" + nameof(Unknowns);
		public const string Exploring = "/" + nameof(Exploring);

		public const string ConsoleApp = "/console-app";
		public const string Changelog = "/" + nameof(Changelog);
		public const string ChangelogWeb = Changelog + "/web";
		public const string ChangelogConsole = Changelog + "/console";
		
		public const string Awesome = "/" + nameof(Awesome);
		public const string Sources = "/" + nameof(Sources);
		public const string Contributing = "/" + nameof(Contributing);
		public const string Community = "/" + nameof(Community);
		public const string RosettaStone = "/rosetta-stone";
		public const string Discord = "https://discord.gg/s4wTHQgxae";

		public const string LangCheck = "/lang-check";

		public const string Page = "/p";
		public const string ContentPagePrefix = Page + "?c=";
	}
}
