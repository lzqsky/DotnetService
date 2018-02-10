using System.Collections.Generic;
using Dotnet_DataAccess;
using Dotnet_Middleware.Authentication;
using Dotnet_Middleware.Authentication.Jwt;
using Dotnet_Middleware.ServiceDiscover;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dotnet_Service
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
            List<ResourcePremission> urls = new List<ResourcePremission>()
            {
                new ResourcePremission { Url = "/base/Operator/FindPageList", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Operator/Add", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Operator/Edit", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Operator/Remove", PremissionType = "API" },

                new ResourcePremission { Url = "/base/Technician/FindPageList", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Technician/Add", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Technician/Edit", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Technician/Remove", PremissionType = "API" },

                new ResourcePremission { Url = "/base/Hospital/FindPageList", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Hospital/Add", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Hospital/Edit", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Hospital/Remove", PremissionType = "API" },

                new ResourcePremission { Url = "/base/ContactGroup/FindPageList", PremissionType = "API" },
                new ResourcePremission { Url = "/base/ContactGroup/Add", PremissionType = "API" },
                new ResourcePremission { Url = "/base/ContactGroup/Edit", PremissionType = "API" },
                new ResourcePremission { Url = "/base/ContactGroup/Remove", PremissionType = "API" },

                new ResourcePremission { Url = "/base/Register/FindPageList", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Register/ExamineVerify", PremissionType = "API" },
                new ResourcePremission { Url = "/base/Register/Remove", PremissionType = "API" }
            };
            List<UserPermission> permission = new List<UserPermission>()
            {
                new UserPermission
                {
                    UserName="buzzly",
                    Role = new RolePremission{ RoleName = "admin", Urls = urls}
                }
            };
            services.AddOcelotPolicyJwtBearer(permission, "lzqsky");


            services.AddMvc();
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //日志
            //loggerFactory.AddNLog();
            //env.ConfigureNLog("nlog.config");

            //验证中间件
            app.UseAuthentication();

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
            app.RegisterWithConsul(lifetime, Configuration);//here
        }
    }
}
