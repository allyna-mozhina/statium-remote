using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data;
using Statium.Alpha.Remote.Data.Models;
using Statium.Alpha.Remote.Data.Repositories;
using Statium.Alpha.Remote.Launcher.RemoteAccess;

namespace Statium.Alpha.Remote.Launcher.JobManagement
{
    //Root class for resource management subsystem
    //Any and all usage of resource management subsystem from other subsystems is to be done through this class
    public class ResourceManager
    {        
        //Schedule and submit the job to a remote system
        public void SubmitJob(Job job)
        {
            if (job.ClusterProfile == null)
                scheduler.Schedule(job);

            IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(job.ClusterProfile.Cluster.LaunchType);

            string newKey = accessor.SendJob(job);
            activeJobs.Add(job.Id, newKey);
        }

        //Get job execution status from remote system
        public async Task<JobStatus> GetJobStatusAsync(int jobId)
        {
            JobStatus result = JobStatus.Aborted;
            Job subject = new Job() { Id = jobId};//activeJobs.Where(j => j.Id == jobId).First();
            IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(subject.ClusterProfile.Cluster.LaunchType);
            string remoteKey;
            activeJobs.TryGetValue(jobId, out remoteKey);
            result = accessor.CheckJob(subject, remoteKey);

            if (result == JobStatus.Aborted || result == JobStatus.Killed)
                activeJobs.Remove(jobId);    

            return result;
        }

        //Interrupt job execution on a remote system
        //Upon this operation the job is removed from a remote system and will no longer be accesible
        public void KillJob(int jobId)
        {
            Job subject = new Job() { Id = jobId };
            IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(subject.ClusterProfile.Cluster.LaunchType);
            string remoteKey;
            activeJobs.TryGetValue(jobId, out remoteKey);
            accessor.KillJob(subject, remoteKey);
            activeJobs.Remove(jobId);
        }

        //Fetch job execution results from a remote system
        //Upon this operation the job is removed from a remote system and will no longer be accesible
        public async Task<int[]> GetJobResultAsync(int jobId)
        {
            int[] result;

            Job subject = new Job() { Id = jobId };//activeJobs.Where(j => j.Id == jobId).First();
            IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(subject.ClusterProfile.Cluster.LaunchType);
            string remoteKey;
            activeJobs.TryGetValue(jobId, out remoteKey);
            result = accessor.FetchResults(subject, remoteKey);
            activeJobs.Remove(jobId);
            return result;
        }

        //Try to access a remote system without submitting a job
        public async Task<bool> TestAccess(Cluster cluster, ClusterProfile profile)
        {
            IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(cluster.LaunchType);
            return accessor.TestAccess(profile);
        }

        private ResourceManager()
        {
            scheduler = new JobScheduler();
            accessorFactory = new RemoteAccessorFactory("jai");
            activeJobs = new Dictionary<int, string>();
        }

        public static ResourceManager GetInstance()
        {
            if (instance == null)
                instance = new ResourceManager();

            return instance;
        }

        private static ResourceManager instance = null;
        private JobScheduler scheduler;
        private RemoteAccessorFactory accessorFactory;
        private Dictionary<int, string> activeJobs;
    }
}
