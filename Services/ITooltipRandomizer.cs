namespace WebApp.SoE.Services
{
	public interface ITooltipRandomizer
	{
		string NextTooltip(bool allowFreeze = true);
		string NextMenuTooltip(bool allowFreeze = true);
		string GetTooltip(int index, bool allowFreeze = true);
	}
}