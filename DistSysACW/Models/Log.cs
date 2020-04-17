using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW
{
    public class Log
    {
        [Key]
        public string LogID { get; set; }
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }

        public Log () {}
    }
}
