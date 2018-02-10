using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Service.Controllers
{
    [Route("base/[controller]")]
    public class HomeController: BaseController
    {
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult About()
        {
            return Ok("JwtToken 认证中心");
        }
    }
}
