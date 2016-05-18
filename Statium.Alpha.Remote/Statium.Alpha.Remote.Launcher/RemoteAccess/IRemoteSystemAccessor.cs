using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Launcher.RemoteAccess
{
    //Interface for classes doing protocol-specific access to various types of remote systems
    public interface IRemoteSystemAccessor
    {
        string SendJob(Job job);

        JobStatus CheckJob(Job job, string remote_id);

        void KillJob(Job job, string remote_id);

        int[] FetchResults(Job job, string remote_id);

        bool TestAccess(ClusterProfile profile);
    }
}
