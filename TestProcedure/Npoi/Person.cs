using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProcedure.Npoi
{
    public class Person
    {
        //[Column(Index =0,Title ="名称")]
        public string Name { get; set; }
        //[Column(Index =2,Title ="年龄")]
        public int Age { get; set; }
        //[Column(IsIgnored =true)]
        public string  Car { get; set; }

    }
    
}
