using System.ComponentModel.DataAnnotations;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared.Enums
{
	public enum MandatoryGameId
	{
		[Display(Name = nameof(Resources.Game1), ResourceType = typeof(Resources))]
		One = 1,
		[Display(Name = nameof(Resources.Game2), ResourceType = typeof(Resources))]
		Two,
		[Display(Name = nameof(Resources.Game3), ResourceType = typeof(Resources))]
		Three,
		[Display(Name = nameof(Resources.Game4), ResourceType = typeof(Resources))]
		Four
	}
}