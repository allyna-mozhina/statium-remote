using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statium.Alpha.Remote.Exchange
{
    public enum RequestType
    {
        New, Status, Kill
    }
    
    public class Request
    {
        public int JobId { get; set; }

        public RequestType Type { get; set; }
    }
}
