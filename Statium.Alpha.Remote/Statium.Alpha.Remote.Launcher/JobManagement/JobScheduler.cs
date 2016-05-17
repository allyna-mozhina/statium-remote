using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data;
using Statium.Alpha.Remote.Data.Models;
using Statium.Alpha.Remote.Data.Repositories;

namespace Statium.Alpha.Remote.Launcher.JobManagement
{
    //Assigns a remote system to the submitted job
    public class JobScheduler
    {
        public void Schedule(Job job)
        {
            if (job.ClusterProfile != null)
                return;

            using (IRepository<ClusterProfile> repository = new HardcodeRepository<ClusterProfile>(new ClusterProfile[] { }))
            {
                IEnumerable<ClusterProfile> available = repository.GetAll().Where(cp => cp.UserId == job.UserId);

                if (!available.Any())
                    return;

                IEnumerable<ClusterProfile> profilePool = available;

                if (job.EstimatedTime == 0)
                    EstimateCompletionTime(job);

                if (job.MaxCost != 0)
                {
                    profilePool = available.Where(cp => cp.Cluster.ChargeRate * job.EstimatedTime <= job.MaxCost);

                    if (!profilePool.Any())
                        return;
                }

                IEnumerable<ClusterProfile> goodType = profilePool.Where(cp => cp.Cluster.ProcTypes == job.Method.GoodProc);
                
                if (goodType.Any())
                    profilePool = goodType;
                else
                    profilePool = available;

                double cheapest = profilePool.Min(cp => cp.Cluster.ChargeRate);
                ClusterProfile chosen = profilePool.Where(cp => cp.Cluster.ChargeRate <= cheapest).First();
                
                job.ClusterProfile = chosen;
                job.ClusterProfileId = chosen.Id;                
            }
        }

        private void EstimateCompletionTime(Job job)
        {
            job.EstimatedTime = 60;
        }
    }
}
