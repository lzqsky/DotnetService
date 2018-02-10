using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_Middleware.Authentication
{
    public interface ICustomAuthentication
    {
        void ConfigureServices(IServiceCollection services, List<ResourcePremission> urls, List<UserPermission> permission);
        void Configure(IApplicationBuilder builder);
        IActionResult Login(LoginData data, Controller c);
        IActionResult Logout(Controller c);
        TimeSpan ExpireTime { get; set; }
    }
}
