namespace WebApp.SoE.Services
{
	public interface IRandomResTooltip
	{
		string this[string resText, int randomizedOneOutOfChance = 0, int tooltipIndex = -1] { get; }
	}
}