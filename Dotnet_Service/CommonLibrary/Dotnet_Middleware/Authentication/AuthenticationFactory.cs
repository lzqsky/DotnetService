using System;
using Dotnet_Middleware.Authentication.Cookie;
using Dotnet_Middleware.Authentication.Jwt;
using Dotnet_Middleware.Authentication.Default;

namespace Dotnet_Middleware.Authentication
{
    public static class AuthenticationFactory
    {
        public static ICustomAuthentication CustomAuthentication { get; set; }

        public static void CreateAuthentication(CustomAuthType key,TimeSpan expireTime)
        {
            switch(key)
            {
                case CustomAuthType.Cookie:
                    CustomAuthentication = new CookieAuthentication { ExpireTime = expireTime };
                    break;
                case CustomAuthType.Jwt:
                    CustomAuthentication = new JwtAuthentication { ExpireTime = expireTime }; 
                    break;
                default:
                    CustomAuthentication = new DefaultAuthentication { ExpireTime = expireTime };
                    break;
            }
        }

        public enum CustomAuthType
        {
            Cookie,
            Jwt,
            Default
        }
    }


}
