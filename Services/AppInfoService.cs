using System;
using System.IO;
using System.Reflection;
using Common.Shared.Min.Extensions;

namespace WebApp.SoE.Services
{
    public class AppInfoService : IAppInfoService
    {
        public DateTime CompileTime { get; }
        public string PackageVersion { get; }
        public string? AppTitle { get; }

        public AppInfoService() :this(Assembly.GetEntryAssembly()!) {}
        public AppInfoService(Assembly assembly)
        {
            assembly.ThrowIfNull(nameof(assembly));

            CompileTime = new FileInfo(assembly.Location).LastWriteTime;

            AppTitle = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

            PackageVersion = GetAssemblyVersion(assembly);
        }

        public string GetAssemblyVersion(Assembly assembly)
        {
            var version = GetPackageVersion(assembly);
            if (version.EndsWith(".0"))
                version = PackageVersion.Replace(".0", string.Empty);

            return version;

            static string GetPackageVersion(Assembly assembly) =>
                assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion.SubstringBefore("-");
        }
    }
}
