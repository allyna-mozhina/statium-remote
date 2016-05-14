using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Statium.Alpha.Remote.Models
{
    public enum JobStatus
    {
        Processing, Queued, Running, Completed, Aborted, Killed 
        //Aborted is for jobs aborted by remoted system itself 
        //Killed is for jobs killed by user
    }
}