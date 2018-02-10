using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Service.Controllers
{
    [Authorize("Permission")]
    //[EnableCors("AllowSameDomain")]     //跨域
    public class BaseController : Controller
    {
        public BaseController()
        {
            //Logger = LogManager.GetCurrentClassLogger();
        }

        //protected Logger Logger;
    } 
}
