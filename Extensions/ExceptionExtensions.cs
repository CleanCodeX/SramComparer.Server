using System;
using System.Drawing;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace WebApp.SoE.Extensions
{
	public static class ExceptionExtensions
	{
		public static MarkupString GetColoredMessage(this Exception source)
		{
			var message = source is TargetInvocationException ex ? ex.InnerException!.Message : source.Message;

			return message.ColorText(Color.Red).ToMarkup();
		}
	}
}
