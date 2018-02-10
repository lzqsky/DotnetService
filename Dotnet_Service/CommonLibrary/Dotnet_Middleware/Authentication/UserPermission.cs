using System.Collections.Generic;

namespace Dotnet_Middleware.Authentication
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserPermission
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public RolePremission Role
        { get; set; }
    }

    public class ResourcePremission
    {
        /// <summary>
        /// 请求Url
        /// </summary>
        public string Url
        { get; set; }

        /// <summary>
        /// 请求类型 Page  API
        /// </summary>
        public string PremissionType
        { get; set; }
    }

    public class RolePremission
    {
        public string RoleName
        { get; set; }

        public List<ResourcePremission> Urls
        { get; set; }
    }


    public class LoginData
    {
        public string Sid { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
