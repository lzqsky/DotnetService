using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_DataAccess;
using Dotnet_Middleware.Authentication;
using Dotnet_Middleware.Authentication.Jwt;
using Dotnet_Middleware.ServiceDiscover;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_JWT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AuthenticationFactory.CreateAuthentication(AuthenticationFactory.CustomAuthType.Jwt, new TimeSpan(0, 30, 0));
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJTokenBuild();
            services.AddMvc();
        }
         
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //初始化数据库连接 
            DbInitData initdata = new DbInitData
            {
                DbName = Configuration["DbConfig:DbName"],
                Ip = Configuration["DbConfig:Ip"],
                ProviderName = Configuration["DbConfig:ProviderName"],
                Pwd = Configuration["DbConfig:Pwd"],
                UserName = Configuration["DbConfig:UserName"]
            };
            DbFactory.DbInit = initdata;

            app.UseMvc();
            //consul
            app.RegisterWithConsul(lifetime, Configuration);
        }
    }
}
