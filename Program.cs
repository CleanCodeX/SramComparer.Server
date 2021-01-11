using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebApp.SoE
{
	public class Program
	{
		public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
#if DEBUG
					webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
					webBuilder.CaptureStartupErrors(true);
#endif
					webBuilder.UseStaticWebAssets();
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					var env = hostingContext.HostingEnvironment;
					config.SetBasePath(env.ContentRootPath);
					config.AddJsonFile("appsettings.json", false, true);
					if(env.IsDevelopment())
						config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
					
					config.AddEnvironmentVariables();
				});
	}
}
