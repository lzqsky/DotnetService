using System.Collections.Generic;
using System.Data;

namespace Dotnet_DataAccess
{
    public static class DataAccessExtensions
    {
        public static IEnumerable<dynamic> Query(this IDbConnection cnn, string sql, object param = null)
        {
            return Dapper.SqlMapper.Query(cnn, sql, param);
        }

        public static int Execute(this IDbConnection cnn, string sql, object param = null)
        {
            return Dapper.SqlMapper.Execute(cnn, sql, param);
        }

        public static T ExecuteScalar<T>(this IDbConnection cnn,string sql,object param=null)
        {
            return Dapper.SqlMapper.ExecuteScalar<T>(cnn, sql, param);
        }
    }
}
