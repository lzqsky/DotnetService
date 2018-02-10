using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServiceBusiness.DAL;
using ServiceBusiness.Model;

namespace Dotnet_Service.Controllers
{
    [Route("base/[controller]")]
    public class TechnicianController:BaseController
    {
        /// <summary>
        /// 获取所有的技术员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult FindPageList(string name, int currentPageIndex, int pageSize)
        {
            TechnicianDAL dal = new TechnicianDAL();
            IEnumerable<dynamic> result = dal.FindPageList(name, currentPageIndex, pageSize, out var total);
            var enumerable = result as dynamic[] ?? result.ToArray();

            return UnifyApiResult.PageResult(enumerable, total);
        }

        /// <summary>
        /// 添加技术员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Add(TechnicianModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");
            if (string.IsNullOrEmpty(model.Name))
                return UnifyApiResult.Error("姓名不能为空。");
            if (string.IsNullOrEmpty(model.Phone))
                return UnifyApiResult.Error("手机不能为空。");

            TechnicianDAL dal = new TechnicianDAL();
            if (dal.GetData(model.Name) != null)
                return UnifyApiResult.Error("姓名不允许重复。");
             
            dynamic result = dal.Insert(model);
            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 编辑技术员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Edit(TechnicianModel model)
        {
            if (model == null)
                return UnifyApiResult.Error("参数不能为空。");

            if (!string.IsNullOrEmpty(model.Name))
                return UnifyApiResult.Error("姓名不能为空。");
            if (string.IsNullOrEmpty(model.Phone))
                return UnifyApiResult.Error("手机不能为空。");

            TechnicianDAL dal = new TechnicianDAL();
            dynamic result = dal.Modify(model);

            return UnifyApiResult.Sucess(result);
        }

        /// <summary>
        /// 删除技术员
        /// </summary>  
        [HttpGet("[action]")]
        public UnifyApiResult Remove(string code)
        {
            if (string.IsNullOrEmpty(code))
                return UnifyApiResult.Error("参数不能为空。");

            TechnicianDAL dal = new TechnicianDAL();
            dynamic result = dal.Remove(code);
            return UnifyApiResult.Sucess(result);
        }
    }
}
