using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Dotnet_Middleware.Authentication.Jwt
{
    /// <summary>
    /// 必要参数类
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; } = "/denied";

        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { internal get; set; } = ClaimTypes.Role;
        /// <summary>
        /// 请求路径
        /// </summary>
        public string LoginPath { get; set; } = "/Login";
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
     
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

       
        /// <summary>
        /// 构造
        /// </summary>  
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="signingCredentials">签名验证实体</param>
        public PermissionRequirement( string issuer, string audience, SigningCredentials signingCredentials)
        { 
            Issuer = issuer;
            Audience = audience; 
            SigningCredentials = signingCredentials;
        }
    }
}
