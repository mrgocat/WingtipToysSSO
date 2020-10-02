using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipSSO.MogoDBDataAccess
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
// https://developer.okta.com/blog/2020/06/29/aspnet-core-mongodb