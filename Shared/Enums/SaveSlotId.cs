using System.ComponentModel.DataAnnotations;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared.Enums
{
	public enum SaveSlotId
	{
		[Display(Name = nameof(Resources.AllSaveSlots), ResourceType = typeof(Resources))]
		All,
		[Display(Name = nameof(Resources.SaveSlot1Only), ResourceType = typeof(Resources))]
		First,
		[Display(Name = nameof(Resources.SaveSlot2Only), ResourceType = typeof(Resources))]
		Second,
		[Display(Name = nameof(Resources.SaveSlot3Only), ResourceType = typeof(Resources))]
		Third,
		[Display(Name = nameof(Resources.SaveSlot4Only), ResourceType = typeof(Resources))]
		Fourth
	}
}