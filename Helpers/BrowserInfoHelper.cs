using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Shared.Min.Extensions;
using SramCommons.Extensions;

namespace WebApp.SoE.Helpers
{
	public enum BrowserMinVersion
	{
		Firefox = 78, 
		Chrome = 66, 
		Trident = 0, // No support at all
		Edge = 80, 
		Opera = 53
	}

	public enum BrowserES5FallbackMinVersion
	{
		Firefox = 30,
		Chrome = 30,
		Trident = 0, // No support at all
		Edge = 30,
		Opera = 30
	}

	public static class BrowserInfoHelper
	{
		private static readonly Dictionary<string, int> MinVersions = default(BrowserMinVersion).ToDictionary().ToDictionary(k => k.Key, v => v.Value.ToInt());

		private static readonly Dictionary<string, int> ES5FallbackMinVersions = default(BrowserES5FallbackMinVersion).ToDictionary().ToDictionary(k => k.Key, v => v.Value.ToInt());

		public static (string Name, int Version) ExtractBrowserInfo(in string userAgent)
		{
			var regEx = new Regex(@"(MSIE|Trident|(?!Gecko.+)Firefox|(?!AppleWebKit.+Chrome.+)Safari(?!.+Edge)|(?!AppleWebKit.+)Chrome(?!.+Edge)|(?!AppleWebKit.+Chrome.+Safari.+)Edge|AppleWebKit(?!.+Chrome|.+Safari)|Gecko(?!.+Firefox))(?: |\/)([\d\.apre]+)").Match(userAgent);

			var browserName = regEx.Groups[1].Value;

			string version;
			if (browserName == "Trident")
			{
				browserName = "MSIE";
				version = userAgent.SubstringAfter(" rv:").SubstringBefore(".");
			}
			else
				version = regEx.Groups[2].Value.SubstringBefore(".");

			return new(browserName, int.Parse(version));
		}

		
		public static bool IsSupportedBrowser(string userAgent) => IsSupportedBrowser(ExtractBrowserInfo(userAgent));

		private static bool IsSupportedBrowser((string Name, int Version) browserInfo) => IsSupportedBrowser(browserInfo.Name, browserInfo.Version);
		public static bool IsSupportedBrowser(string Name, int version)
		{
			if (!MinVersions.TryGetValue(Name, out var minVersion) || minVersion == 0)
				return false;

			return version >= minVersion;
		}

		public static bool HasES5Support(string userAgent) => HasES5Support(ExtractBrowserInfo(userAgent));
		private static bool HasES5Support((string Name, int Version) browserInfo) => HasES5Support(browserInfo.Name, browserInfo.Version);

		public static bool HasES5Support(string Name, int version)
		{
			if (!ES5FallbackMinVersions.TryGetValue(Name, out var minVersion) || minVersion == 0)
				return false;

			return version >= minVersion;
		}
	}
}
