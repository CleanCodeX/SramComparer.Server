using System.Collections.Generic;

namespace WebApp.SoE.Extensions
{
	/// <summary>
	/// Provides safe access to dictionary values
	/// </summary>
	public static class DictionaryExtensions
	{
		public static string? GetValue(this IDictionary<string, string?> source, string key) => source.TryGetValue(key, out var value) ? value : null;
	}
}
