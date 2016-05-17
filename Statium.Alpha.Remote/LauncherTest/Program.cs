using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Launcher.JobManagement;
using Statium.Alpha.Remote.Launcher.RemoteAccess;
using Statium.Alpha.Remote.Data.Models;

namespace LauncherTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAsync();
        }

        static async void TestAsync()
        {
            Cluster ccfit = new Cluster
            {
                ChargeRate = 0,
                Id = 1,
                LaunchType = "Linux",
                Name = "Ccfit",
                ProcTypes = "CPU_SINGLE",
                Url = "ccfit.nsu.ru"
            };

            User ya = new User();

            ClusterProfile ccfitProfile = new ClusterProfile
            {
                Id = 1,
                Cluster = ccfit,
                ClusterId = ccfit.Id,
                QueueName = null,
                User = new User(),
                UserId = 1
            };

            Method testMethod = new Method
            {
                Id = 1,
                GoodProc = "CPU_SINGLE",
                Name = "Test",
                OutGridCount = 1
            };

            Grid inGrid = new Grid
            {
                Id = 1,
                DomainName = "global",
                Hash = "0",
                Name = "Test input",
                Jobs = null,
                User = ya,
                UserId = 1
            };

            Job testJob = new Job
            {
                Id = 1,
                ClusterProfile = ccfitProfile,
                ClusterProfileId = ccfitProfile.Id,
                Method = testMethod,
                MethodId = testMethod.Id,
                EstimatedTime = 1,
                MaxCost = 0,
                Grids = new List<Grid> { inGrid },
                User = ya,
                UserId = 1
            };

            ResourceManager manaja = ResourceManager.GetInstance();

            bool accessGranted = await manaja.TestAccess(ccfit, ccfitProfile);

            if (accessGranted)
            {
                manaja.SubmitJob(testJob);

                JobStatus status = await manaja.GetJobStatusAsync(testJob.Id);
                Console.WriteLine(status);

                int[] output = await manaja.GetJobResultAsync(testJob.Id);

                manaja.SubmitJob(testJob);

                manaja.KillJob(testJob.Id);
            }   
        }
    }
}
