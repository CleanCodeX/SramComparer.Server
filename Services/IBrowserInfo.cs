namespace WebApp.SoE.Services
{
	public interface IBrowserInfo
	{
		(string Name, int Version) ExtractBrowserInfo(in string userAgent);

		bool IsSupportedBrowser(string userAgent) => IsSupportedBrowser(ExtractBrowserInfo(userAgent));
		bool IsSupportedBrowser(string Name, int version);
		private bool IsSupportedBrowser((string Name, int Version) browserInfo) => IsSupportedBrowser(browserInfo.Name, browserInfo.Version);

		bool HasES5Support(string userAgent) => HasES5Support(ExtractBrowserInfo(userAgent));
		private bool HasES5Support((string Name, int Version) browserInfo) => HasES5Support(browserInfo.Name, browserInfo.Version);
		bool HasES5Support(string Name, int version);
	}
}