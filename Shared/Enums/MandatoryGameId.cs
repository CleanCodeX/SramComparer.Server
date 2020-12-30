using System.ComponentModel.DataAnnotations;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared.Enums
{
	public enum MandatoryGameId
	{
		[Display(Name = nameof(Resources.SaveSlot1), ResourceType = typeof(Resources))]
		One = 1,
		[Display(Name = nameof(Resources.SaveSlot2), ResourceType = typeof(Resources))]
		Two,
		[Display(Name = nameof(Resources.SaveSlot3), ResourceType = typeof(Resources))]
		Three,
		[Display(Name = nameof(Resources.SaveSlot4), ResourceType = typeof(Resources))]
		Four
	}
}