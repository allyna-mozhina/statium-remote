using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Statium.Alpha.Remote.Models
{
    public class Cluster : Computer
    {
        public string LaunchType { get; set; }

        public string ProcTypes { get; set; }
    }
}