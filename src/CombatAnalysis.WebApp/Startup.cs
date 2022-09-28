using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Settings;
using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CombatAnalysis.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisteringDependencies(services);

            var settings = Configuration.GetSection(nameof(TokenSettings));
            var scheme = settings.GetValue<string>(nameof(TokenSettings.AuthScheme));

            services.Configure<TokenSettings>(settings);

            Port.CombatParserApi = Configuration.GetValue<string>("CombatParserApiPort");
            Port.UserApi = Configuration.GetValue<string>("UserApiPort");

            services.AddControllersWithViews();

            IHttpClientHelper httpClient = new HttpClientHelper();
            services.AddSingleton(httpClient);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new IdentityMappingMapper());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void RegisteringDependencies(IServiceCollection services)
        {
            services.RegisterDependenciesBL(Configuration, "DefaultConnection");
            services.RegisterIdentityDependencies();
        }
    }
}
