namespace WebApp.SoE.Services
{
	public class TooltipRandomizerOptions
	{
		public int TooltipOneOutOfChance { get; set; }  // 1 out of X chance
		public int FreezeTooltipWaitTimeInSeconds { get; set; } 
		public int FreezeTooltipOneOutOfChance { get; set; }  // 1 out of X chance
		public string? FreezeTooltipBeginMarker { get; set; } 
	}
}