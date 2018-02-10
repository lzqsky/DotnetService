using System.Collections.Generic;

namespace Dotnet_Middleware.Authentication
{
    /// <summary>
    /// 权限中间件选项
    /// </summary>
    public class PermissionMiddlewareOption
    {
        public List<ResourcePremission> ResourcePerssions
        { get; set; } = new List<ResourcePremission>();
        /// <summary>
        /// 用户权限集合
        /// </summary>
        public List<UserPermission> UserPerssions
        { get; set; } = new List<UserPermission>();

        /// <summary>
        /// 无权限跳转的页面
        /// </summary>
        public string NoPermissionUrl { get; set; }
    }
}
