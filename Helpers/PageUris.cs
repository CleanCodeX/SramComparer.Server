namespace WebApp.SoE.Helpers
{
	internal static class PageUris
	{
		public const string Comparing = "/" + nameof(Comparing);

		public const string Offset = "/" + nameof(Offset);
		public const string OffsetEditing = Offset + "/editing";

		public const string About = "/" + nameof(About);
		public const string Contributors = "/" + nameof(Contributors);
		public const string Contributor = "/" + nameof(Contributor);
		public const string Localizing = "/" + nameof(Localizing);
		public const string Features = "/" + nameof(Features);

		public const string Guide = "/" + nameof(Guide);
		public const string GuideSrm = Guide + "/Srm";
		public const string GuideSavestate = Guide + "/Savestate";

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
		public const string SramDocu = "/" + nameof(SramDocu);
		public const string Discord = "https://discord.gg/s4wTHQgxae";
		
		public const string Page = "/p";
		public const string ContentPagePrefix = Page + "?c=";
	}
}
