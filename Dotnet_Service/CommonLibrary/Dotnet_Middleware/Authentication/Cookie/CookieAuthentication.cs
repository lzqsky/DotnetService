using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Middleware.Authentication.Cookie
{
    public class CookieAuthentication: ICustomAuthentication
    {
        private const string CookieName = "lzqsky";
      
        List<ResourcePremission> _urls;
        List<UserPermission> _permission;

        public void ConfigureServices(IServiceCollection services, List<ResourcePremission> urls, List<UserPermission> permission)
        {
            AuthenticationBuilder builder = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            builder.AddCookie(options =>
            {
                options.Cookie.Name = CookieName;
                options.Cookie.Domain = CookieName;
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Login");
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/denied");
                options.ExpireTimeSpan = ExpireTime;       
            });

            _urls = urls;
            _permission = permission;
        }


        public void Configure(IApplicationBuilder builder)
        {
            //引入权限中间件
            builder.UseMiddleware<PermissionMiddleware>(new PermissionMiddlewareOption()
            {
                ResourcePerssions = _urls,
                UserPerssions = _permission
            });
        }


        public IActionResult Login(LoginData data, Controller c)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, data.Sid));
            identity.AddClaim(new Claim(ClaimTypes.Name, data.Name));
            identity.AddClaim(new Claim(ClaimTypes.Role, data.Role));

            c.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return c.RedirectToAction("Index", "Home");
        }

        public IActionResult Logout(Controller c)
        {
            c.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return c.RedirectToAction("Index", "Home");

        }

        public TimeSpan ExpireTime { get; set; } = new TimeSpan(0, 5, 0);
    }
}
