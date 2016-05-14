using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Models;

namespace Statium.Alpha.Remote.Launcher.RemoteAccess
{
    //Interface for classes doing protocol-specific access to various types of remote systems
    interface RemoteSystemAccessor
    {
        void SendJob(Job job);

        JobStatus CheckJob(Job job);

        void KillJob(Job job);

        int[] FetchResults(Job job);
    }
}
