using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dotnet_Middleware.Authentication.Jwt
{
    public class JwtToken
    {
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary> 
        /// <returns></returns>
        public static string BuildJwtToken(Claim[] claims, PermissionRequirement permissionRequirement, TimeSpan expireTime)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: permissionRequirement.Issuer,
                audience: permissionRequirement.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expireTime),
                signingCredentials: permissionRequirement.SigningCredentials
            ); 
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
