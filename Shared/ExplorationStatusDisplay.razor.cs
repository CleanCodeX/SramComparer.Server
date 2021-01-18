using WebApp.SoE.Helpers;

namespace WebApp.SoE.Shared
{
	public partial class ExplorationStatusDisplay
	{
		public string? Status { get; private set; }

		protected override void OnInitialized() => Status = ExplorationStatusHelper.GetExplorationStatus();
	}
}
