using AutoMapper;
using CombatAnalysis.BL.Extensions;
using CombatAnalysis.BL.Mapping;
using CombatAnalysis.Identity.Extensions;
using CombatAnalysis.Identity.Mapping;
using CombatAnalysis.Identity.Settings;
using CombatAnalysis.UserApi.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CombatAnalysis.UserApi
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

            var loggerFactory = new LoggerFactory();
            var logger = new Logger<ILogger>(loggerFactory);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User API",
                    Version = "v1",
                });
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserApiMapper());
                mc.AddProfile(new BLMapper());
                mc.AddProfile(new IdentityMappingMapper());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<ILogger>(logger);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisteringDependencies(IServiceCollection services)
        {
            services.RegisterDependenciesBL(Configuration, "DefaultConnection");
            services.RegisterIdentityDependencies();
        }
    }
}
