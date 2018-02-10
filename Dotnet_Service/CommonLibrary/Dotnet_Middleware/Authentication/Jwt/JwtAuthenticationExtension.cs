using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet_Middleware.Authentication.Jwt
{
    public static class JwtAuthenticationExtension
    {
        private const string Secret = "2s7rQxMvzGdcPwTQTNAqnt7Cz/YuLcXUpoQw9HJuODMMoS9dyJEW3xc8l+5abHxxKEHtCLIJEO1Vfu9wxFzZHp6KL/zq1CbeDVLVWpEtDvh6NBPRxNHTjgh8Ueg7gLXTZ3eCB7XomLJQnRwmb7YeGjadW6AQXdu93vUGz3GeWlgniBag4to7OPxPqsRbJ3a6/k/ZYVvyG07LqnXMLzPD/5kDEPHQY9DZL23r4W1qGZ05E2Bl4SgYMNcJoBIsj88/JmPhOuJiIzczWas6hLKzjpLAaasXS0iMQxEkZuee/1vYrce+Q4FJRLouPLr5q+r5Yk+jk0o55f0nLawMQnaxtw==";// "Y2F0Y2yhciUyMHdvbmclMFWfsaZlJTIwLm5ldA==";
        private const string Issuer = "lzqsky";       //发行人
        private const string Audience = "everone";     //接收者
        private static TimeSpan ExpireTime = new TimeSpan(0, 30, 0);     //TOKEN 过期时间

        /// <summary>
        /// 注入Gateway下JwtBearer，在ocelot网关的Startup的ConfigureServices中调用
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="defaultScheme">默认架构</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddOcelotJwtBearer(this IServiceCollection services, string defaultScheme)
        {
            var keyByteArray = Encoding.ASCII.GetBytes(Secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            return services.AddAuthentication(options =>
            {
                options.DefaultScheme = defaultScheme;
            })
             .AddJwtBearer(defaultScheme, opt =>
             {
                 //不使用https
                 opt.RequireHttpsMetadata = false;
                 opt.TokenValidationParameters = tokenValidationParameters;
             });
        }

        /// <summary>
        /// 注入Ocelot jwt策略，在业务API应用中的Startup的ConfigureServices调用
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="permission"></param>
        /// <param name="defaultScheme">默认架构</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddOcelotPolicyJwtBearer(this IServiceCollection services,
            List<UserPermission> permission, string defaultScheme)
        { 
            var keyByteArray = Encoding.ASCII.GetBytes(Secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人
                ValidateAudience = true,
                ValidAudience = Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(Issuer, Audience, signingCredentials);
            //注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
            services.AddSingleton(permission);

            return services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission",
                          policy => policy.Requirements.Add(permissionRequirement));

            })
             .AddAuthentication(options =>
             {
                 options.DefaultScheme = defaultScheme;
             })
             .AddJwtBearer(defaultScheme, o =>
             {
                 //不使用https
                 o.RequireHttpsMetadata = false;
                 o.TokenValidationParameters = tokenValidationParameters;
             });
        }

        /// <summary>
        /// JwtToken 注入Token生成器参数，在token生成项目的Startup的ConfigureServices中使用
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns></returns>
        public static IServiceCollection AddJTokenBuild(this IServiceCollection services)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)), SecurityAlgorithms.HmacSha256);
            //如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(Issuer, Audience,signingCredentials);
            return services.AddSingleton(permissionRequirement);

        } 

        public static string Login(this Controller c,LoginData data, PermissionRequirement permissionRequirement)
        {
            var claims = new Claim[] {
                new Claim(ClaimTypes.Sid, data.Sid),
                new Claim(ClaimTypes.Name, data.Name),
                new Claim(ClaimTypes.Role, data.Role),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(ExpireTime.TotalSeconds).ToString(CultureInfo.InvariantCulture))
            };
            //用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);

            string token = JwtToken.BuildJwtToken(claims, permissionRequirement, ExpireTime);
            return token.TrimStart('"').TrimEnd('"');
        }

        public static IActionResult Logout(this Controller c)
        {
            return c.Ok();
        }
    }
}
