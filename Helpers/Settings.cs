using System.Collections.Generic;

#nullable disable

namespace WebApp.SoE.Helpers
{
	public record Settings
	{
		public IDictionary<string, string> Urls { get; set; }
		public IDictionary<string, string> Files { get; set; }
	}
}
