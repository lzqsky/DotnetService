namespace Dotnet_DataAccess
{
    public static class DbFactory
    { 
        public static DbInitData DbInit { get; set; }

        public static IDataConnection InitDataBase(DbInitData data = null)
        {
            if (data == null)
                data = DbInit;
             switch(data.ProviderName)
             {
                case "MsSql":
                    DataConnection = new MsSqlDataAccess(data);
                    break;
             }
            return DataConnection;
        }

        public static IDataConnection DataConnection { get; set; }
    }

    public class DbInitData
    {
        public string ProviderName { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string DbName { get; set; }
        public string Ip { get; set; }
    }
}
