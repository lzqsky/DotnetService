using Dotnet_Middleware.Authentication.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Dotnet_Ocelot
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("configuration.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            string defaultScheme = "lzqsky";
            services.AddOcelotJwtBearer(defaultScheme);

            services.AddOcelot(Configuration)
                .AddStoreOcelotConfigurationInConsul();
        }
         

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 

            #region 跨域
            var urls = Configuration["AppConfig:AllowSameDomainUrl"].Split(',');
            app.UseCors(builder =>
                builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials()
            );
            #endregion
            app.UseOcelot().Wait();
        }
    }
}
