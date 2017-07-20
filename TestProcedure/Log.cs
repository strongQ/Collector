using Collector.Helper;
using Collector.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProcedure
{
    public class Log:ILog
    {
        public void WirteLog(string content, Exception e)
        {
            LogHelper.WriteLog(content, LogHelper.LogEnum.错误);
        }
    }
}
