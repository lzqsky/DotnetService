using Dotnet_Middleware.Authentication;
using Dotnet_Middleware.Authentication.Jwt;
using Dotnet_Util.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistBusiness.DAL; 

namespace Dotnet_JWT.Controllers
{ 
    public class LoginController : Controller
    {
        readonly PermissionRequirement _permissionRequirement;
        public LoginController(PermissionRequirement permissionRequirement)
        {
            _permissionRequirement = permissionRequirement;

        }
        [AllowAnonymous]
        [HttpGet("login")]
        public UnifyApiResult LoginResult(string username, string password)
        { 
            if (string.IsNullOrEmpty(username))
                return ApiResult.Error("用户名不允许为空。");

            if (string.IsNullOrEmpty(password))
                return ApiResult.Error("密码不允许为空。");

            OperatorDAL dal = new OperatorDAL();
            dynamic operatorData = dal.GetUserPwd(username, EncryptionHelper.UserMd5(password));

            if (operatorData != null)
            {  
                //固定角色的方法  角色只有 admin，system 2种，根据2种角色分别限制不同的Controller，不够灵活
                LoginData data = new LoginData()
                {
                    Sid = operatorData.OperatorID.ToString(),
                    Name = operatorData.LoginName,
                    Role = "admin"
                };
                
                return ApiResult.Sucess(new
                {
                    result = operatorData,
                    token = this.Login(data, _permissionRequirement)
                });
            }
            return ApiResult.Error("用户名密码输入不正确。");
        }

        [AllowAnonymous]
        [HttpGet("logout")]
        public IActionResult LogoutResult()
        {
            return this.Logout();
        }

        [AllowAnonymous]
        [HttpGet("denied")]
        public IActionResult DeniedAction()
        {
            return new ForbidResult();
        }


        [AllowAnonymous]
        [HttpGet("about")]
        public IActionResult About()
        {
            return Ok("JwtToken 认证中心");
        }
    }
     
}
