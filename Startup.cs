using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using WebApp.SoE.Helpers;
using WebApp.SoE.Services;
using WebApp.SoE.ViewModels;
using Westwind.AspNetCore.LiveReload;

namespace WebApp.SoE
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IAppInfoService, AppInfoService>();
			services.AddScoped<CompareViewModel>(sp => new() {LocalStorage = sp.GetRequiredService<ProtectedLocalStorage>()});
			services.AddHttpContextAccessor();

			services.AddScoped<SetOffsetValueViewModel>(sp => new()
			{
				JsRuntime = sp.GetRequiredService<IJSRuntime>(),
				LocalStorage = sp.GetRequiredService<ProtectedLocalStorage>()
			});

			services.Configure<RequestLocalizationOptions>(
				options =>
				{
					var uiCulture = CultureInfo.GetCultureInfo("en");
					var supportedCultures = new List<CultureInfo>
					{
						uiCulture,
						CultureInfo.GetCultureInfo("de"),
						CultureInfo.GetCultureInfo("fr"),
						CultureInfo.GetCultureInfo("it"),
						CultureInfo.GetCultureInfo("jp"),
					};

					CultureInfo.CurrentUICulture = uiCulture;

					options.DefaultRequestCulture = new RequestCulture(uiCulture);
					// Formatting numbers, dates, etc.
					options.SupportedCultures = supportedCultures;
					// UI strings that we have localized.
					options.SupportedUICultures = supportedCultures;

					options.AddInitialRequestCultureProvider(new QueryStringRequestCultureProvider());
					options.AddInitialRequestCultureProvider(new CookieRequestCultureProvider());
					options.AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());
				});

			services.AddOptions<Settings>().Bind(Configuration.GetSection(nameof(Settings)));
			services.AddSingleton(cfg => cfg.GetService<IOptionsMonitor<Settings>>()!.CurrentValue);

#if DEBUG
			services.AddLiveReload(config => {
				config.LiveReloadEnabled = true;
				config.ClientFileExtensions = ".css,.js,.htm,.html";
				config.FolderToMonitor = "~/../";
			});
#endif

			services.AddHttpClient();
			services.AddRazorPages();
			services.AddServerSideBlazor();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRequestLocalization();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

#if DEBUG
			app.UseLiveReload();
#endif

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
