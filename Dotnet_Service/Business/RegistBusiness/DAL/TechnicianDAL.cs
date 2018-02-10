using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegistBusiness.Model;
using Dotnet_DataAccess;

namespace RegistBusiness.DAL
{
    public  class TechnicianDAL
    {
        public dynamic GetData(string name)
        {
            dynamic result = null;
            string sql = "select Code,Name,Phone, case when IsEnabled = 1 then '已激活' else '未激活' end EnableState,RCount,QQ,Area,License from Technician where Name=@Name";
            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                var list = conn.Query(sql, new { Name = name });
                var enumerable = list as dynamic[] ?? list.ToArray();
                if (enumerable.Any())
                    result = enumerable.FirstOrDefault();
            });
            return result;
        }

        public IEnumerable<dynamic> FindPageList(string name, int currentPageIndex, int pageSize, out int total)
        {
            IEnumerable<dynamic> result = null;
            int count = 0;
            string sql = @"SELECT w1.Code,w1.Name,w1.Phone,case when w1.IsEnabled = 1 then '已激活' else '未激活' end EnableState,w1.RCount,w1.QQ,w1.Area,w1.License from Technician w1,( 
                    SELECT row_number() OVER(ORDER BY Code) n, Code
                    FROM Technician) w2 WHERE w1.Code = w2.Code {0} AND w2.n >= @currentPageIndex and w2.n <= @pageSize ORDER BY w2.n ASC ";


            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                if (string.IsNullOrEmpty(name))
                {
                    sql = string.Format(sql, "");
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from Technician");
                }
                else
                {
                    sql = string.Format(sql, " and w1.Name like @Name");
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize,
                        Name = '%' + name + '%'
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from Technician where Name like @Name", 
                        new { Name = '%' + name + '%' });
                }
            });
            total = count;
            return result;
        }


        public int Insert(TechnicianModel model)
        {
            dynamic result = null;
            string sql = "insert into Technician(Name,Phone,IsEnabled,RCount,QQ,Area,License,ModifyDate) " +
                         "values(@Name,@Phone,@IsEnabled,@RCount,@QQ,@Area,@License,@ModifyDate)";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    IsEnabled = model.EnableState == "已激活" ? 1 : 0,
                    RCount = model.RCount,
                    QQ = model.QQ,
                    Area = model.Area,
                    License = model.License,
                    ModifyDate = DateTime.Now,
                });
            });
            return result;
        }

        public int Modify(TechnicianModel model)
        {
            dynamic result = null;
            string sql = "update Technician set Name=@Name,Phone=@Phone,IsEnabled=@IsEnabled,RCount=@RCount,QQ=@QQ,Area=@Area,License=@License,ModifyDate=@ModifyDate where Code=@Code";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new
                {
                    Code = model.Code,
                    Name = model.Name,
                    Phone = model.Phone,
                    IsEnabled = model.EnableState == "已激活" ? 1 : 0,
                    RCount = model.RCount,
                    QQ = model.QQ,
                    Area = model.Area,
                    License = model.License,
                    ModifyDate = DateTime.Now,
                });
            });
            return result;
        }

        public int Remove(string Code)
        {
            dynamic result = null;
            string sql = "Delete Technician where Code=@Code";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new { Code = Code });
            });
            return result;
        }
    }
}
