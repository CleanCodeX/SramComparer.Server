using System.ComponentModel.DataAnnotations;
using WebApp.SoE.Properties;

namespace WebApp.SoE.Shared.Enums
{
	public enum SaveSlotId
	{
		[Display(Name = nameof(Resources.AllSaveSlots), ResourceType = typeof(Resources))]
		All,
		[Display(Name = nameof(Resources.FirstSaveSlot), ResourceType = typeof(Resources))]
		First,
		[Display(Name = nameof(Resources.SecondSaveSlot), ResourceType = typeof(Resources))]
		Second,
		[Display(Name = nameof(Resources.ThirdSaveSlot), ResourceType = typeof(Resources))]
		Third,
		[Display(Name = nameof(Resources.FourthSaveSlot), ResourceType = typeof(Resources))]
		Fourth
	}
}