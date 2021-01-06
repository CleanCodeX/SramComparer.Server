using System.ComponentModel.DataAnnotations;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared.Enums
{
	public enum SaveSlotId
	{
		[Display(Name = nameof(Resources.EnumAllSaveSlots), ResourceType = typeof(Resources))]
		All,
		[Display(Name = nameof(Resources.EnumSaveSlot1Only), ResourceType = typeof(Resources))]
		First,
		[Display(Name = nameof(Resources.EnumSaveSlot2Only), ResourceType = typeof(Resources))]
		Second,
		[Display(Name = nameof(Resources.EnumSaveSlot3Only), ResourceType = typeof(Resources))]
		Third,
		[Display(Name = nameof(Resources.EnumSaveSlot4Only), ResourceType = typeof(Resources))]
		Fourth
	}
}