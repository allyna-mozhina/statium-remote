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

            accessor.SendJob(job);
            activeJobs.AddLast(job);
        }

        //Get job execution status from remote system
        public async Task<JobStatus> GetJobStatusAsync(int jobId)
        {
            JobStatus result = JobStatus.Aborted;
            Job subject = activeJobs.Where(j => j.Id == jobId).First();
            IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(subject.ClusterProfile.Cluster.LaunchType);
            result = accessor.CheckJob(subject);    

            return result;
        }

        //Interrupt job execution on a remote system
        //Upon this operation the job is removed from a remote system and will no longer be accesible
        public void KillJob(int jobId)
        {
            using (IRepository<Job> repository = new HardcodeRepository<Job>(new Job[] { }))
            {
                Job subject = repository.Find(jobId);
                IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(subject.ClusterProfile.Cluster.LaunchType);
                accessor.KillJob(subject);
            }
        }

        //Fetch job execution results from a remote system
        //Upon this operation the job is removed from a remote system and will no longer be accesible
        public async Task<int[]> GetJobResultAsync(int jobId)
        {
            int[] result;

            using (IRepository<Job> repository = new HardcodeRepository<Job>(new Job[] { }))
            {
                Job subject = repository.Find(jobId);
                IRemoteSystemAccessor accessor = accessorFactory.GetAccessor(subject.ClusterProfile.Cluster.LaunchType);
                result = accessor.FetchResults(subject);
            }

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
            activeJobs = new LinkedList<Job>();
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
        private LinkedList<Job> activeJobs;
    }
}
