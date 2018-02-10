using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dotnet_DataAccess
{
    public  class OdbcDataAccess : IDataConnection
    {
        readonly string _dbstr;
        public OdbcDataAccess(DbInitData data)
        {
            _dbstr =$"Uid={data.UserName};Pwd={data.Pwd};Initial Catalog={data.DbName};Data Source={data.Ip};Connect Timeout=1900";
        }
        public void BeginConnection(Action<IDbConnection> action)
        {
            using (IDbConnection conn = new System.Data.SqlClient.SqlConnection(_dbstr))
            {
                action(conn);
            }
        }
    }
}
