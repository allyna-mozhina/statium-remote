using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statium.Alpha.Remote.Exchange
{
    public enum ResponseType
    {
        Queued, Running, Failed, Done
    }

    public class Response
    {
        public int JobId { get; set; }

        public ResponseType Type { get; set; }
    }
}
