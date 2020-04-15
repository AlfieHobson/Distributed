using System;
using System.Collections.Generic;
using System.Text;

namespace DistSysACWClient.Models
{
    public class Body
    {
        public string username { get; set; }
        public string role { get; set; }
        public Body (string name)
        {
            username = name;
        }
    }
}
