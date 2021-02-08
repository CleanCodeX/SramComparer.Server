using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.SoE.Services
{
	public class Translator : ITranslator
	{
		public async Task<string?> TranslateTextAsync(string content, string fromLanguage, string toLanguage)
		{
			// Set the language from/to in the url (or pass it into this function)
			string url = $"translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(content)}";
			using HttpClient httpClient = new() {BaseAddress = new("https://translate.googleapis.com")};

			try
			{
				var result = await httpClient.GetFromJsonAsync<List<dynamic>>(url);

				// Extract just the first array element (This is the only data we are interested in)
				var items = result?[0];
				if (items is null) return null;

				var jsonElement = (JsonElement) items;

				// Translation Data
				string translation = "";

				// Loop through the collection extracting the translated objects
				foreach (var item in jsonElement.EnumerateArray())
				{
					// Convert the item array to IEnumerable
					var translationLineObject = item.EnumerateArray();

					// Convert the IEnumerable translationLineObject to a IEnumerator
					using var translationLineString = translationLineObject.GetEnumerator();

					// Get first object in IEnumerator
					translationLineString.MoveNext();

					// Save its value (translated text)
					translation += $" {Convert.ToString(translationLineString.Current)}";
				}

				// Remove first blank character
				if (translation.Length > 1) 
					translation = translation[1..]; 

				// Return translation
				return translation;
			}
			// ReSharper disable once RedundantCatchClause
			catch (Exception ex)
			{
				throw;
			}
		}
    }
}
