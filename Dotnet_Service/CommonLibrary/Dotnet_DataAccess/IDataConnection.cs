using System;
using System.Data;

namespace Dotnet_DataAccess
{
    public interface IDataConnection
    {
        void BeginConnection(Action<IDbConnection> action);
    }
}
