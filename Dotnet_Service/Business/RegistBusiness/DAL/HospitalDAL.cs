using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dotnet_DataAccess;
using RegistBusiness.Model; 

namespace RegistBusiness.DAL
{
    public  class HospitalDAL
    {
        public dynamic GetData(string name)
        {
            dynamic result = null;
            string sql = "select HID,HName,Description,ModifyDate,ContactGroupId  from Hospital where HName=@Name";
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
            string sql = @"SELECT w1.HID,w1.HName,w1.Description,w1.ModifyDate,w1.ContactGroupId from Hospital w1,( 
                    SELECT row_number() OVER(ORDER BY HID) n, HID
                    FROM Hospital) w2 WHERE w1.HID = w2.HID {0} AND w2.n >= @currentPageIndex and w2.n <= @pageSize ORDER BY w2.n ASC ";


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
                    count = conn.ExecuteScalar<int>("select count(*) from Hospital");
                }
                else
                {
                    sql = string.Format(sql, " and w1.HName like @Name");
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize,
                        Name = '%' + name + '%'
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from Hospital where HName like @Name",
                        new { Name = '%' + name + '%' });
                }
            });
            total = count;
            return result;
        }


        public int Insert(HospitalModel model)
        {
            dynamic result = null;
            string sql = "insert into Hospital(HName,Description,ModifyDate,ContactGroupId) " +
                         "values(@HName,@Description,@ModifyDate,@ContactGroupId)";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, model);
            });
            return result;
        }

        public int Modify(HospitalModel model)
        {
            dynamic result = null;
            string sql = "update Hospital set Description=@Description,ModifyDate=@ModifyDate,ContactGroupId=@ContactGroupId  where HID=@HID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, model);
            });
            return result;
        }

        public int Remove(string id)
        {
            dynamic result = null;
            string sql = "Delete Hospital where HID=@HID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new { HID = id });
            });
            return result;
        }
    }
}
