using RegistBusiness.Model;
using Dotnet_DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace RegistBusiness.DAL
{
    public class OperatorDAL
    {
        public dynamic GetData(string username)
        {
            dynamic result = null;
            string sql = "select OperatorID,LoginName,Grade from Operator where LoginName=@LoginName";
            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                var list = conn.Query(sql, new { LoginName = username });
                var enumerable = list as dynamic[] ?? list.ToArray();
                if (enumerable.Any())
                    result = enumerable.FirstOrDefault();
            });
            return result;
        }
 
        public dynamic GetUserPwd(string username, string password)
        {
            dynamic result = null;
            string sql = "select OperatorID,LoginName,Grade from Operator where LoginName=@LoginName and Password=@Password";
            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                var list = conn.Query(sql, new { LoginName = username, Password = password });
                var enumerable = list as dynamic[] ?? list.ToArray();
                if (enumerable.Any())
                    result = enumerable.FirstOrDefault();
            });

            return result;
        }

        public IEnumerable<dynamic> FindPageList(string name, int currentPageIndex, int pageSize,out int total)
        {
            IEnumerable<dynamic> result = null;
            int count = 0;
            string sql = @"SELECT w1.OperatorID,w1.LoginName,w1.Grade FROM Operator w1,( 
                    SELECT row_number() OVER(ORDER BY OperatorID) n, OperatorID
                    FROM Operator) w2 WHERE w1.OperatorID = w2.OperatorID {0} AND w2.n >= @currentPageIndex and w2.n <= @pageSize ORDER BY w2.n ASC ";


            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                if (string.IsNullOrEmpty(name))
                {
                    sql = string.Format(sql, "");
                    result = conn.Query(sql,new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from Operator"); 
                }
                else
                {
                    sql = string.Format(sql," and w1.LoginName like @LoginName");
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize,
                        LoginName = '%' + name + '%'
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from Operator where LoginName like @LoginName",new { LoginName = '%' + name + '%' });
                }
            });
            total = count;
            return result;
        }

        public int Insert(OperatorModel model)
        {
            dynamic result = null;
            string sql = "insert into Operator(LoginName,Grade,Password) values(@LoginName,@Grade,@Password)";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, model);
            });
            return result;
        }

        public int Modify(OperatorModel model)
        {
            dynamic result = null;
            string sql = "update Operator set LoginName=@LoginName,Grade=@Grade,Password=@Password where OperatorID=@OperatorID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, model);
            });
            return result;
        }

        public int Remove(string id)
        {
            dynamic result = null;
            string sql = "Delete Operator where OperatorID=@OperatorID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new { OperatorID = id });
            });
            return result;
        }
    }
}