using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_Middleware.Authentication.Default
{
    public class DefaultAuthentication : ICustomAuthentication
    {
        public TimeSpan ExpireTime { get; set; } = new TimeSpan(0, 5, 0);

        public void Configure(IApplicationBuilder builder)
        {
           
        }

        public void ConfigureServices(IServiceCollection services, List<ResourcePremission> urls, List<UserPermission> permission)
        {
           
        }

        public IActionResult Login(LoginData data, Controller c)
        {
            return c.Forbid();
        }

        public IActionResult Logout(Controller c)
        {
            return c.Forbid();
        }
    }
}
