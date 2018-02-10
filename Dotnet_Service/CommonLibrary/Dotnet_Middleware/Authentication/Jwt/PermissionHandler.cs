using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_Middleware.Authentication.Jwt
{
    /// <summary>
    /// 权限授权Handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        /// <summary>
        /// 自定义策略参数
        /// </summary>
        public PermissionRequirement Requirement
        { get; set; }

        public List<UserPermission> Permissions { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="permission"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, List<UserPermission> permission)
        {
            Schemes = schemes;
            Permissions = permission;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        { 
            ////赋值用户权限      
            Requirement = requirement;
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            AuthorizationFilterContext authorizationFilterContext = context.Resource as AuthorizationFilterContext;
            if (authorizationFilterContext != null)
            {
                var httpContext = authorizationFilterContext.HttpContext;
                //请求Url
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //判断请求是否停止
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
                {
                    var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                    if (handler != null && await handler.HandleRequestAsync())
                    {
                        httpContext.Response.Redirect(requirement.DeniedAction);
                        return;
                    }
                }
                //判断请求是否拥有凭据，即有没有登录
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    //result?.Principal不为空即登录成功
                    if (result?.Principal != null)
                    {
                        httpContext.User = result.Principal;
                        var name = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name).Value;
                        if (!Permissions.Any(g => g.UserName == name && g.Role?.Urls != null && g.Role.Urls.Any(w => w.Url.ToLower() == questUrl)))
                        {
                            //无权限跳转到拒绝页面 
                            context.Fail();
                            return;
                        } 
                   
                        //判断过期时间
                        if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.Now)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                        }
                        return;
                    } 
                }
            
                //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) 
                    && (!httpContext.Request.Method.Equals("POST")                                                                                
                        || !httpContext.Request.HasFormContentType))
                {
                    context.Fail();  
                    return;
                }
            }
            context.Succeed(requirement);
        }
        
    }
    
}

