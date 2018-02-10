using System;
using System.Collections.Generic;
using System.Linq;
using Dotnet_Middleware;
using Microsoft.AspNetCore.Mvc;
using RegistBusiness.DAL;
using RegistBusiness.Model; 

namespace Dotnet_Service.Controllers
{
    [Route("base/[controller]")]
    public class ContactGroupController: BaseController
    {
        /// <summary>
        /// 获取所有的项目负责人
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult FindPageList(string name, int currentPageIndex, int pageSize)
        {
            
            ContactGroupDAL dal = new ContactGroupDAL();
            IEnumerable<dynamic> result = dal.FindPageList(name, currentPageIndex, pageSize, out var total);
            var enumerable = result as dynamic[] ?? result.ToArray();

            return UnifyApiResult.PageResult(enumerable, total);
        }

        /// <summary>
        /// 添加项目负责人
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Add(ContactGroupModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");
            if (string.IsNullOrEmpty(model.UserName))
                return UnifyApiResult.Error("姓名不能为空。");
            if (string.IsNullOrEmpty(model.Contact))
                return UnifyApiResult.Error("电话不能为空。");
            if (string.IsNullOrEmpty(model.ContactGroupId))
                return UnifyApiResult.Error("分组不能为空。");

            ContactGroupDAL dal = new ContactGroupDAL();
            if (dal.GetData(model.UserName) != null)
                return UnifyApiResult.Error("姓名不允许重复。");

            model.ModifyDatetime = DateTime.Now;
            dynamic result = dal.Insert(model);
            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 编辑项目负责人
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Edit(ContactGroupModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");

            if (string.IsNullOrEmpty(model.UserName))
                return UnifyApiResult.Error("姓名不能为空。");
            if (string.IsNullOrEmpty(model.Contact))
                return UnifyApiResult.Error("电话不能为空。");
            if (string.IsNullOrEmpty(model.ContactGroupId))
                return UnifyApiResult.Error("分组不能为空。");


            model.ModifyDatetime = DateTime.Now;
            ContactGroupDAL dal = new ContactGroupDAL();
            dynamic result = dal.Modify(model);

            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 删除项目负责人
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                return UnifyApiResult.Error("参数不能为空。");

            ContactGroupDAL dal = new ContactGroupDAL();
            dynamic result = dal.Remove(id);
            return UnifyApiResult.Sucess(result);
        }
    }
}
