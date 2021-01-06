using System.ComponentModel.DataAnnotations;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared.Enums
{
	public enum MandatoryGameId
	{
		[Display(Name = nameof(Resources.EnumSaveSlot1), ResourceType = typeof(Resources))]
		One = 1,
		[Display(Name = nameof(Resources.EnumSaveSlot2), ResourceType = typeof(Resources))]
		Two,
		[Display(Name = nameof(Resources.EnumSaveSlot3), ResourceType = typeof(Resources))]
		Three,
		[Display(Name = nameof(Resources.EnumSaveSlot4), ResourceType = typeof(Resources))]
		Four
	}
}