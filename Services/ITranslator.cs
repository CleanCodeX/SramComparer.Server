using System.Threading.Tasks;

namespace WebApp.SoE.Services
{
	public interface ITranslator
	{
		Task<string?> TranslateTextAsync(string content, string fromLanguage, string toLanguage);
	}
}