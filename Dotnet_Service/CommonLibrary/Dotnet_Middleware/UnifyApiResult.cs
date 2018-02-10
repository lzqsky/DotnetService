using System;
using System.Collections.Generic;
using System.Text;

namespace Dotnet_Middleware
{
    public class UnifyApiResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        public dynamic Data { get; set; }

        public static UnifyApiResult Sucess(dynamic data)
        {
            return new UnifyApiResult { Code = 200, Data = data };
        }

        public static UnifyApiResult Error(string msg)
        {
            return new UnifyApiResult { Code = 500, Msg = msg };
        }

        public static ApiResult PageResult(IEnumerable<dynamic> list, int total)
        {
            return new ApiResult { Code = 200, Data = new PageResult { List = list, Total = total } };
        }
    }

    public class PageResult
    {
        public dynamic List { get; set; }
        public int Total { get; set; }
    }
}
