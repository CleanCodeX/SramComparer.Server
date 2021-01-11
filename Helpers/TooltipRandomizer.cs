using System;

namespace WebApp.SoE.Helpers
{
	public static class TooltipRandomizer
	{
		private static readonly string[] Tooltips = new
		{
			"Take heed and go no further",
			"Camp on the banks of the great green Limpopo River",
			"Souvenir Spoon Pounding Factory 5 Furlongs, Tours daily",
			"I'd turn back if I were you",
			"Visit beautiful Gruelville, 25 leagues west",
			"Come see Mr. Head at Perceval Plank's Exhibition of Cultural Oddities",
			"See the amazing Bearded Boy, next left  (meaning?),
			"Half Way",
			"City of Costagando, 30 leagues east",
			 "Mountains of Candy, 50 furlongs" 
		};

		private static readonly Random Random = new ();

		public static string NextTooltip() => Tooltips[Random.Next(0, Tooltips.Length)];
	}
}
