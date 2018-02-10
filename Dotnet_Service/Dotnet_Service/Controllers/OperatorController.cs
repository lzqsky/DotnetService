using System;
using System.Collections.Generic;
using System.Linq;
using Dotnet_Middleware; 
using Dotnet_Util.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegistBusiness.DAL;
using RegistBusiness.Model; 

namespace Dotnet_Service.Controllers
{
    [Route("base/[controller]")]
    public class OperatorController : BaseController
    { 
        /// <summary>
        /// 获取所有的操作员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult FindPageList(string name,int currentPageIndex,int pageSize)
        {
            OperatorDAL dal = new OperatorDAL();
            int total;
            IEnumerable<dynamic> result = dal.FindPageList(name, currentPageIndex,pageSize,out total);
            var enumerable = result as dynamic[] ?? result.ToArray();
            
            return UnifyApiResult.PageResult(enumerable, total);
        }

        /// <summary>
        /// 添加操作员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Add(OperatorModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");
            if (string.IsNullOrEmpty(model.LoginName))
                return UnifyApiResult.Error("用户名不能为空。");
            if (string.IsNullOrEmpty(model.Password))
                return UnifyApiResult.Error("密码不能为空。");

            OperatorDAL dal = new OperatorDAL();
            if (dal.GetData(model.LoginName) == null)
                return UnifyApiResult.Error("用户名不允许重复。");

            model.Password = EncryptionHelper.UserMd5(model.Password);
            dynamic result = dal.Insert(model); 
            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 编辑操作员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Edit(OperatorModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");

            if (!string.IsNullOrEmpty(model.Password))
            {
                model.Password = EncryptionHelper.UserMd5(model.Password);
            }
            OperatorDAL dal = new OperatorDAL();
            dynamic result = dal.Modify(model);
         
            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 删除操作员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Remove(string  id)
        {
            if (string.IsNullOrEmpty(id))
                return UnifyApiResult.Error("参数不能为空。");

            OperatorDAL dal = new OperatorDAL();
            dynamic result = dal.Remove(id); 
            return UnifyApiResult.Sucess(result);
        }
    }
}
