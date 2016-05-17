using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Launcher.RemoteAccess
{
    public class TorqueAccessor : IRemoteSystemAccessor
    {
        public JobStatus CheckJob(Job job)
        {
            throw new NotImplementedException();
        }

        public int[] FetchResults(Job job)
        {
            throw new NotImplementedException();
        }

        public void KillJob(Job job)
        {
            throw new NotImplementedException();
        }

        public void SendJob(Job job)
        {
            throw new NotImplementedException();
        }

        public bool TestAccess(ClusterProfile profile)
        {
            throw new NotImplementedException();
        }
    }
}
