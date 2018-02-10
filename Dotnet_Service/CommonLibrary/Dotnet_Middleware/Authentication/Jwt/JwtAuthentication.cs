using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet_Middleware.Authentication.Jwt
{
    /// <summary>
    /// Authorization  Bearer XXXXX
    /// </summary>
    public class JwtAuthentication : ICustomAuthentication
    {
        private const string Secret = "2s7rQxMvzGdcPwTQTNAqnt7Cz/YuLcXUpoQw9HJuODMMoS9dyJEW3xc8l+5abHxxKEHtCLIJEO1Vfu9wxFzZHp6KL/zq1CbeDVLVWpEtDvh6NBPRxNHTjgh8Ueg7gLXTZ3eCB7XomLJQnRwmb7YeGjadW6AQXdu93vUGz3GeWlgniBag4to7OPxPqsRbJ3a6/k/ZYVvyG07LqnXMLzPD/5kDEPHQY9DZL23r4W1qGZ05E2Bl4SgYMNcJoBIsj88/JmPhOuJiIzczWas6hLKzjpLAaasXS0iMQxEkZuee/1vYrce+Q4FJRLouPLr5q+r5Yk+jk0o55f0nLawMQnaxtw==";// "Y2F0Y2yhciUyMHdvbmclMFWfsaZlJTIwLm5ldA==";
        private const string Issuer = "lzqsky";       //发行人
        private const string Audience = "everone";     //接收者
        private const string AuthenticateScheme = "lzqsky";
        public void Configure(IApplicationBuilder app)
        {
           
        }
        PermissionRequirement _permissionRequirement;
        public void ConfigureServices(IServiceCollection services, List<ResourcePremission> urls, List<UserPermission> permission)
        { 
            //读取配置文件 
            var keyByteArray = Encoding.ASCII.GetBytes(Secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = AuthenticateScheme;
                    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(AuthenticateScheme, o =>
                {
                    //不使用https
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = tokenValidationParameters;
                });


            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //这个集合模拟用户权限表,可从数据库中查询出来

            //如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            _permissionRequirement = new PermissionRequirement(Issuer, Audience, signingCredentials);
            //注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(_permissionRequirement);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission",
                          policy => policy.Requirements.Add(_permissionRequirement));
            });
           
        }

        public IActionResult Login(LoginData data, Controller c)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Sid, data.Sid),
                new Claim(ClaimTypes.Name, data.Name),
                new Claim(ClaimTypes.Role, data.Role),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(ExpireTime.TotalSeconds).ToString(CultureInfo.InvariantCulture)) 
            };
            //用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);

            var token = JwtToken.BuildJwtToken(claims, _permissionRequirement, ExpireTime);
            return new JsonResult(token);
        }

        public IActionResult Logout(Controller c)
        {
            return c.Ok();
        }

        public TimeSpan ExpireTime { get; set; } = new TimeSpan(0, 5, 0);
    }
}
