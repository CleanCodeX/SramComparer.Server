namespace SramComparer.Server.Helpers
{
	public record Settings(string? ReadMeUrl, string? GitHubUrl, string? ChangeLogUrl)
	{
		public Settings() :this(default, default, default) { }
	}
}
