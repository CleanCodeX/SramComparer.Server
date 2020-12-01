using System.ComponentModel.DataAnnotations;
using SramComparer.Server.Properties;

namespace SramComparer.Server.ViewModels
{
	public enum GameId
	{
		[Display(Name = nameof(Resources.AllGames), ResourceType = typeof(Resources))]
		All,
		[Display(Name = nameof(Resources.Game1Only), ResourceType = typeof(Resources))]
		One,
		[Display(Name = nameof(Resources.Game2Only), ResourceType = typeof(Resources))]
		Two,
		[Display(Name = nameof(Resources.Game3Only), ResourceType = typeof(Resources))]
		Three,
		[Display(Name = nameof(Resources.Game4Only), ResourceType = typeof(Resources))]
		Four
	}

	public enum MandatoryGameId
	{
		[Display(Name = nameof(Resources.Game1), ResourceType = typeof(Resources))]
		One = 1,
		[Display(Name = nameof(Resources.Game2), ResourceType = typeof(Resources))]
		Two,
		[Display(Name = nameof(Resources.Game3), ResourceType = typeof(Resources))]
		Three,
		[Display(Name = nameof(Resources.Game4), ResourceType = typeof(Resources))]
		Four
	}
}