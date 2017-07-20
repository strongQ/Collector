using Collector.DataBase;
using Collector.Interface;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;


namespace TestProcedure
{
    public class DbHelper:DBHelpMe
    {
       private static Log log=new Log();
       public DbHelper(string connstr) : base(log, connstr) { }
        public override System.Data.IDbConnection GetConnection()
        {
            OracleConnection conn = new OracleConnection(ConnStr);
            conn.Open();
            return conn;
        }
    }
}
