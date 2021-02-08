using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Blazor.Polyfill.Server;
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
			services.AddHttpContextAccessor();

			services.AddScoped<CompareViewModel>(sp => new() {LocalStorage = sp.GetRequiredService<ProtectedLocalStorage>()});
			services.AddScoped<SetOffsetValueViewModel>(sp => new()
			{
				JsRuntime = sp.GetRequiredService<IJSRuntime>(),
				LocalStorage = sp.GetRequiredService<ProtectedLocalStorage>()
			});

			services.AddOptions<Settings>().Bind(Configuration.GetSection(nameof(Settings)));
			services.AddSingleton(cfg => cfg.GetService<IOptionsMonitor<Settings>>()!.CurrentValue);

			var supportedCulturesSection = Configuration.GetSection(nameof(SupportedCultures));
			services.AddOptions<SupportedCultures>().Bind(supportedCulturesSection);
			var supportedCultureIds = supportedCulturesSection.GetSection("Cultures").GetChildren().Select(e => e.Value).ToList();

			services.AddOptions<TooltipRandomizerOptions>().Bind(Configuration.GetSection("TooltipRandomizer"));
			services.AddSingleton(cfg => cfg.GetService<IOptionsMonitor<TooltipRandomizerOptions>>()!.CurrentValue);

			services.AddSingleton<ITranslator, Translator>();
			services.AddSingleton<ILocalizationCollector, LocalizationCollector>();
			services.AddSingleton<IMarkdownBuilder, MarkdownBuilder>();
			services.AddSingleton<IExplorationStatus, ExplorationStatus>();
			services.AddSingleton<ITooltipRandomizer, TooltipRandomizer>();
			services.AddSingleton<CrypticTooltipRandomizer>();
			services.AddSingleton<IAppInfo, AppInfo>();
			services.AddSingleton<IBrowserInfo, BrowserInfo>();
			services.AddSingleton<IRandomResTooltip, RandomResTooltip>();

			services.Configure<RequestLocalizationOptions>(
				options =>
				{
					var defaultCulture = CultureInfo.GetCultureInfo("en");
					CultureInfo.CurrentCulture = defaultCulture;
					CultureInfo.CurrentUICulture = defaultCulture;
					
					var supportedCultures = new List<CultureInfo> { defaultCulture };
					foreach (var supportedCultureId in supportedCultureIds)
						supportedCultures.Add(CultureInfo.GetCultureInfo(supportedCultureId));

					options.FallBackToParentUICultures = true;
					options.FallBackToParentCultures = true;

					options.SetDefaultCulture(defaultCulture.TwoLetterISOLanguageName);
					// Formatting numbers, dates, etc.
					options.SupportedCultures = supportedCultures;
					// UI strings that we have localized.
					options.SupportedUICultures = supportedCultures;

					options.AddInitialRequestCultureProvider(new QueryStringRequestCultureProvider());
					options.AddInitialRequestCultureProvider(new CookieRequestCultureProvider());
					options.AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());
				});

			services.AddTransient(cfg => cfg.GetService<IOptionsMonitor<SupportedCultures>>()!.CurrentValue);

#if DEBUG
			services.AddLiveReload(config =>
			{
				config.LiveReloadEnabled = true;
				config.ClientFileExtensions = ".css,.js,.htm,.html";
				config.FolderToMonitor = "~/../";
			});
#endif

			services.AddHttpClient();
			services.AddRazorPages().AddDataAnnotationsLocalization();
			services.AddServerSideBlazor();
			services.AddBlazorPolyfill();
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
			app.UseBlazorPolyfill(o =>
			{
				o.ES5FallbackValidation = request =>
				{
					var userAgent = request.Headers["User-Agent"];
					var browserInfo = request.HttpContext.RequestServices.GetRequiredService<IBrowserInfo>();

					return !browserInfo.IsSupportedBrowser(userAgent) && browserInfo.HasES5Support(userAgent);
				};
			});
			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapBlazorHub();
				endpoints.MapRazorPages();
				endpoints.MapFallbackToPage(PageUris.BrowserCheck);
			});
		}
	}
}
