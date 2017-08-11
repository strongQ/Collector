using Dapper;
using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProcedure.Npoi;

namespace TestProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            FluentConfiguration();
            //DbHelper help = new DbHelper(DbHelper.BuildConStr("172.17.99.125,1521", "orcl", "qw", "password", Collector.DataBase.DBHelpMe.DBTypeEnum.Oracle));
            //string a = "2";
            //int data = help.ExecuteSql_All<int>("select count(1) from T_SystemNotice_RECV where GGBH=:GGBH and SEND_STATION_ID=:id", new { GGBH = "2017061910112005", id = a }).FirstOrDefault();
            Person p = new Person
            {
                Name = "张奇",
                Age = 18,
                Car="benci"
            };
            Person p1 = new Person
            {
                Name = "周菲",
                Age = 20,
                Car="naosi"
            };
            List<Person> datas = new List<Person>();
            datas.Add(p);
            datas.Add(p1);
            var excelFile = "e:\\a.xlsx";
            datas.ToExcel(excelFile,"zhoufei");
            Console.ReadKey();
        }
        static void FluentConfiguration()
        {
            var fc = Excel.Setting.For<Person>();
            fc.Property(r => r.Name).HasExcelIndex(0).HasExcelTitle("姓名");
            fc.Property(r => r.Age).HasExcelIndex(1).HasExcelTitle("年龄");
            fc.HasStatistics("合计", "SUM", 1).HasFilter(0, 0, 1, null).HasFreeze(0,1,0,1);
        }
    }
}
