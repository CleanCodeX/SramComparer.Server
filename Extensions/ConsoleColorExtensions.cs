using System;
using System.Drawing;

namespace SramComparer.Server.Extensions
{
	public static  class ConsoleColorExtensions
	{
		/// <summary>
		/// Gets the rgb color for console color
		/// </summary>
		/// <returns>Color.</returns>
		public static Color ToColor(this ConsoleColor color)
		{
			var c = Color.FromName(Enum.Parse(typeof(ConsoleColor), color.ToString()!).ToString()!);

			return c.IsKnownColor switch
			{
				false => Color.FromArgb(255, 128, 128, 0),
				_ => c,
			};
		}
	}
}
