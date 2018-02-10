using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Middleware;
using Microsoft.AspNetCore.Mvc;
using RegistBusiness.DAL; 

namespace Dotnet_Service.Controllers
{
    [Route("base/[controller]")]
    public class RegisterController:BaseController
    {
        /// <summary>
        /// 获取所有的注册信息
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult FindPageList(string statusType,string rtype, int currentPageIndex, int pageSize)
        {
            RegisterDAL dal = new RegisterDAL(); 
            IEnumerable<dynamic> result = dal.FindPageList(statusType,rtype, currentPageIndex, pageSize, out var total);
            var enumerable = result as dynamic[] ?? result.ToArray();

            return UnifyApiResult.PageResult(enumerable, total);
        }

        

        /// <summary>
        /// 编辑注册信息
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult ExamineVerify(string id)
        { 
            if (string.IsNullOrEmpty(id))
                return UnifyApiResult.Error("编号不能为空。");
            
            RegisterDAL dal = new RegisterDAL();
            dynamic result = dal.ExamineVerify(id);

            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 删除注册信息
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                return UnifyApiResult.Error("编号不能为空。");

            RegisterDAL dal = new RegisterDAL();
            dynamic result = dal.Remove(id);
            return UnifyApiResult.Sucess(result);
        }
    }
}
