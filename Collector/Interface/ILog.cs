using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collector.Interface
{
    public interface ILog
    {
        void WriteError(string content, Exception e);
    }
}
