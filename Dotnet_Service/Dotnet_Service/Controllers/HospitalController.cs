using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet_Middleware;
using Microsoft.AspNetCore.Mvc;
using RegistBusiness.DAL;
using RegistBusiness.Model; 

namespace Dotnet_Service.Controllers
{
    [Route("base/[controller]")]
    public class HospitalController: BaseController
    {
        /// <summary>
        /// 获取所有的医院
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult FindPageList(string name, int currentPageIndex, int pageSize)
        {
            HospitalDAL dal = new HospitalDAL();
            IEnumerable<dynamic> result = dal.FindPageList(name, currentPageIndex, pageSize, out var total);
            var enumerable = result as dynamic[] ?? result.ToArray();

            return UnifyApiResult.PageResult(enumerable, total);
        }

        /// <summary>
        /// 添加医院
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Add(HospitalModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");
            if (string.IsNullOrEmpty(model.HName))
                return UnifyApiResult.Error("医院名称不能为空。");
       

            HospitalDAL dal = new HospitalDAL();
            if (dal.GetData(model.HName) != null)
                return UnifyApiResult.Error("医院名称不允许重复。");

            model.ModifyDate = DateTime.Now;
            dynamic result = dal.Insert(model);
            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 编辑医院
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Edit(HospitalModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");

            if (string.IsNullOrEmpty(model.HName))
                return UnifyApiResult.Error("医院名称不能为空。");

            model.ModifyDate = DateTime.Now;
            HospitalDAL dal = new HospitalDAL();
            dynamic result = dal.Modify(model);

            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 删除医院
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                return UnifyApiResult.Error("参数不能为空。");

            HospitalDAL dal = new HospitalDAL();
            dynamic result = dal.Remove(id);
            return UnifyApiResult.Sucess(result);
        }
    }
}
