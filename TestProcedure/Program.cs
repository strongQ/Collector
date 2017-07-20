using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            DbHelper help = new DbHelper(DbHelper.BuildConStr("172.17.99.125,1521", "orcl", "qw", "password", Collector.DataBase.DBHelpMe.DBTypeEnum.Oracle));
            string a = "2";
            int data = help.ExecuteSql_All<int>("select count(1) from T_SystemNotice_RECV where GGBH=:GGBH and SEND_STATION_ID=:id", new { GGBH = "2017061910112005", id = a }).FirstOrDefault();
        }
    }
}
