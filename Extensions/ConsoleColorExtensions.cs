using System;
using System.Drawing;

namespace WebServer.SoE.Extensions
{
	public static  class ConsoleColorExtensions
	{
		/// <summary>
		/// Gets the rgb color of a console color
		/// </summary>
		/// <returns>RGB color</returns>
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
