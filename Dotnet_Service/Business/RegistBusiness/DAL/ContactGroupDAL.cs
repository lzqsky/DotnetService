using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dotnet_DataAccess;
using RegistBusiness.Model; 

namespace RegistBusiness.DAL
{
    public class ContactGroupDAL
    {
        public dynamic GetData(string name)
        {
            dynamic result = null;
            string sql = "select ID,UserName,Contact,ContactGroupId,ModifyDatetime  from ContactGroup where UserName=@UserName";
            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                var list = conn.Query(sql, new { UserName = name });
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
            string sql = @"SELECT w1.ID,w1.UserName,w1.Contact,w1.ContactGroupId,w1.ModifyDatetime from ContactGroup w1,( 
                    SELECT row_number() OVER(ORDER BY ID) n, ID
                    FROM ContactGroup) w2 WHERE w1.ID = w2.ID {0} AND w2.n >= @currentPageIndex and w2.n <= @pageSize ORDER BY w2.n ASC ";


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
                    count = conn.ExecuteScalar<int>("select count(*) from ContactGroup");
                }
                else
                {
                    sql = string.Format(sql, " and w1.UserName like @Name");
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize,
                        Name = '%' + name + '%'
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from ContactGroup where UserName like @Name",
                        new { Name = '%' + name + '%' });
                }
            });
            total = count;
            return result;
        }


        public int Insert(ContactGroupModel model)
        {
            dynamic result = null;
            string sql = "insert into ContactGroup(UserName,Contact,ContactGroupId,ModifyDatetime) " +
                         "values(@UserName,@Contact,@ContactGroupId,@ModifyDatetime)";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, model);
            });
            return result;
        }

        public int Modify(ContactGroupModel model)
        {
            dynamic result = null;
            string sql = "update ContactGroup set  UserName=@UserName,Contact=@Contact,ModifyDatetime=@ModifyDatetime  where ID=@ID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, model);
            });
            return result;
        }

        public int Remove(string id)
        {
            dynamic result = null;
            string sql = "Delete ContactGroup where ID=@ID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new { ID = id });
            });
            return result;
        }
    }
}
