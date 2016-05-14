using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data;
using Statium.Alpha.Remote.Data.Models;

namespace Statium.Alpha.Remote.Launcher.JobManagement
{
    //Root class for resource management subsystem
    //Any and all usage of resource management subsystem from other subsystems is to be done through this class
    class ResourceManager
    {
        //Schedule and submit the job to a remote system
        public void SubmitJob(Job job)
        {

        }

        //Get job execution status from remote system
        public async Task<JobStatus> GetJobStatusAsync(int jobId)
        {
            return JobStatus.Queued;
        }

        //Interrupt job execution on a remote system
        //Upon this operation the job is removed from a remote system and will no longer be accesible
        public void KillJob(int jobId)
        {

        }

        //Fetch job execution results from a remote system
        //Upon this operation the job is removed from a remote system and will no longer be accesible
        public async Task<int[]> GetJobResultAsync(int jobId)
        {
            return new int[] { 3, 14, 15, 92, 6 };
        }

        //Try to access a remote system without submitting a job
        public async Task<bool> TestAccess(Cluster cluster, ClusterProfile profile)
        {
            return true;
        }
    }
}
