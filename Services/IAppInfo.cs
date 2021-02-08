using System;
using System.Reflection;

namespace WebApp.SoE.Services
{
	public interface IAppInfo
	{
		DateTime CompileTime { get; }
		public string PackageVersion { get; }
		string? AppTitle { get; }
		string GetAssemblyVersion(Assembly assembly);
	}
}