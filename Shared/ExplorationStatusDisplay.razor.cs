using Microsoft.AspNetCore.Components;
using WebApp.SoE.Extensions;
using WebApp.SoE.Services;

namespace WebApp.SoE.Shared
{
	public partial class ExplorationStatusDisplay
	{
		[Inject] private ITooltipRandomizer TooltipRandomizer { get; set; } = default!;
		[Inject] private IExplorationStatus ExplorationStatus { get; set; } = default!;

		public MarkupString Status { get; private set; }
		
		protected override void OnInitialized() => Status = ExplorationStatus.GetStatus(false, false).ReplaceWithHtmlLineBreaks();
	}
}
