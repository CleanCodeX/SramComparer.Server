namespace WebApp.SoE.Services
{
	public interface IExplorationStatus
	{
		string GetStatus();
		string GetStatus(bool showWramStatus, bool showRomStatus);
	}
}