using Dotnet_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegistBusiness.Model;

namespace RegistBusiness.DAL
{
    public  class RegisterDAL
    {
        public dynamic GetData(string RType)
        {
            dynamic result = null;
            string sql = @"select r.RID,r.HISoffice as OfficeName,r.ModifyDate,r.AtionCode,
                            case when r.Status = 1 then '已审核' else '未审核' end StatusStr ,h.HName as HospitalName,t.Name as TechnicianName
                            from Register r
                            left join Technician t on  r.Code = t.Code
                            left join Hospital h on r.HID = h.HID
                            where RType = @RType";
            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                var list = conn.Query(sql, new { RType = RType });
                var enumerable = list as dynamic[] ?? list.ToArray();
                if (enumerable.Any())
                    result = enumerable.FirstOrDefault();
            });
            return result;
        }

        public IEnumerable<dynamic> FindPageList(string statusType, string RType, int currentPageIndex, int pageSize, out int total)
        {
            IEnumerable<dynamic> result = null;
            int count = 0;
            string sql = @"SELECT w1.RID,w1.HISoffice as OfficeName,w1.ModifyDate,w1.AtionCode,
                            case when w1.Status = 1 then '已审核' else '未审核' end StatusStr ,h.HName as HospitalName,t.Name as TechnicianName 
                            from Register w1 
                            left join Technician t on  w1.Code = t.Code
                            left join Hospital h on w1.HID = h.HID ,( 
                            SELECT row_number() OVER(ModifyDate desc) n, RID
                            FROM Register {1}) w2 
                            WHERE w1.RID = w2.RID AND w2.n >= @currentPageIndex and w2.n <= @pageSize 
                            {0} 
                            ORDER BY w2.n ASC ";


            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                if (string.IsNullOrEmpty(statusType) && string.IsNullOrEmpty(RType))
                {
                    sql = string.Format(sql, "");
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize
                    });
                    count = conn.ExecuteScalar<int>("select count(*) from Register");
                }
                else
                {
                    if (statusType == "全部")
                    {
                        sql = string.Format(sql, " and w1.RType = @RType ", "where RType = @RType");
                    }
                    else
                    {
                        sql = string.Format(sql, " and w1.RType = @RType and w1.Status = @Status ", "where RType = @RType and Status = @Status");
                    }
                    result = conn.Query(sql, new
                    {
                        currentPageIndex = 1 + ((currentPageIndex - 1) * pageSize),
                        pageSize = currentPageIndex * pageSize,
                        RType = RType == "大屏" ? "NSIS" : "NSIS_DATACENTER",
                        Status = statusType == "已审核" ? 1 : 0
                    });
                    if (statusType == "全部")
                    {
                        count = conn.ExecuteScalar<int>("select count(*) from Register where RType like @RType",
                        new { RType = RType == "大屏" ? "NSIS" : "NSIS_DATACENTER" });
                    }
                    else
                    {
                        count = conn.ExecuteScalar<int>("select count(*) from Register where RType like @RType  and Status = @Status",
                            new
                            {
                                RType = RType == "大屏" ? "NSIS" : "NSIS_DATACENTER",
                                Status = statusType == "已审核" ? 1 : 0
                            });
                    }
                }
            });
            total = count;
            return result;
        }


        public int ExamineVerify(string id)
        {
            dynamic result = null;
            string sql = "update Register set Status=1   where RID=@RID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new { RID  = id});
            });
            return result;
        }

        public int Remove(string id)
        {
            dynamic result = null;
            string sql = "Delete Register where RID=@RID";

            DbFactory.InitDataBase().BeginConnection(conn =>
            {
                result = conn.Execute(sql, new { RID = id });
            });
            return result;
        }
    }
}
