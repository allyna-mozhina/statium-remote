using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statium.Alpha.Remote.Data.Models;
using Statium.Alpha.Remote.Data.Repositories;
using Renci.SshNet;
using System.IO;


namespace Statium.Alpha.Remote.Launcher.RemoteAccess
{
    public class PbsAccessor : IRemoteSystemAccessor
    {
        public JobStatus CheckJob(Job job, string remote_id)
        {
            if (string.IsNullOrEmpty(remote_id))
                return JobStatus.Processing;

            ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "username", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("username", "pass")
                });

            JobStatus result = JobStatus.Processing;

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand check = sshClient.CreateCommand(string.Format("qstat -a {0}", remote_id)))
                {
                    check.Execute();

                    if (check.ExitStatus == 0)
                        result = ParseQstat(check.Result);
                }

                sshClient.Disconnect();
            }

            return result;
        }

        public int[] FetchResults(Job job, string remote_id)
        {
            ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "username", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("username", "pass")
                });

            int[] result = new int[job.Method.OutGridCount];

            using (ScpClient scpClient = new ScpClient(connInfo))
            {
                using (IRepository<Grid> repository = new HardcodeRepository<Grid>(new LinkedList<Grid>()))
                {
                    scpClient.Connect();

                    for (int i = 0; i < job.Method.OutGridCount; ++i)
                    {
                        Grid nextResult = new Grid() { DomainName = "global", User = job.User, UserId = job.UserId };
                        repository.Add(nextResult);
                        result[i] = nextResult.Id;
                        scpClient.Download(string.Format("Statium.Remote/{0}/result{1}.csv", job.Id, i), 
                            new FileInfo(Path.Combine("C:\\Users\\username\\Desktop", "result.csv")));
                    }

                    scpClient.Disconnect();
                }
            }

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand remove = sshClient.CreateCommand(string.Format("rm -rf ~/Statium.Remote/{0}", job.Id)))
                {
                    remove.Execute();

                    if (remove.ExitStatus != 0)
                        return null;
                }

                sshClient.Disconnect();
            }

            return result;
        }

        public void KillJob(Job job, string remote_id)
        {
            ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "username", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("username", "pass")
                });

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand kill = sshClient.CreateCommand(string.Format("qdel {0}", remote_id)))
                {
                    kill.Execute();

                    if (kill.ExitStatus != 0)
                        return;
                }

                using (SshCommand remove = sshClient.CreateCommand(string.Format("rm -rf ~/Statium.Remote/{0}", job.Id)))
                {
                    remove.Execute();

                    if (remove.ExitStatus != 0)
                        return;
                }

                sshClient.Disconnect();
            }
        }

        public string SendJob(Job job)
        {
            /*ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "username", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("username", "pass")
                });*/

            MakeSubmitScript(job);
            string result = "";

            /*using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand firstDir = sshClient.CreateCommand("mkdir -p ~/Statium.Remote"))
                {
                    firstDir.Execute();

                    if (firstDir.ExitStatus != 0)
                        return null;
                }

                using (SshCommand secondDir = sshClient.CreateCommand(string.Format("mkdir -p ~/Statium.Remote/{0}", job.Id)))
                {
                    secondDir.Execute();

                    if (secondDir.ExitStatus != 0)
                        return null;
                }

                using (ScpClient scpClient = new ScpClient(connInfo))
                {
                    scpClient.Connect();

                    scpClient.Upload(new DirectoryInfo(Path.Combine("C:\\Users\\username\\Desktop\\From Debian With Love", "to_send")), string.Format("Statium.Remote/{0}", job.Id));

                    foreach (Grid g in job.Grids)
                        scpClient.Upload(new FileInfo(Path.Combine("C:\\Users\\username\\Desktop", "in.csv")), string.Format("Statium.Remote/{0}", job.Id));

                    scpClient.Disconnect();
                }

                using (SshCommand move = sshClient.CreateCommand(string.Format("mv ~/Statium.Remote/{0}/{0}/* ~/Statium.Remote/{0}", job.Id)))
                {
                    move.Execute();

                    if (move.ExitStatus != 0)
                        return null;
                }

                using (SshCommand start = sshClient.CreateCommand(string.Format("qsub {0}/submit.sh",
                    string.Format("~/Statium.Remote/{0}", job.Id))))
                {
                    start.Execute();

                    if (start.ExitStatus != 0)
                        return null;
                    else
                        result = start.Result;
                }
            }*/

            return result;
        }

        public bool TestAccess(ClusterProfile profile)
        {
            ConnectionInfo connInfo = new ConnectionInfo(profile.Cluster.Url,
                22, "username", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("username", "pass")
                });

            bool result = false;

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand test = sshClient.CreateCommand("qstat -q -f"))
                {
                    test.Execute();

                    if (test.ExitStatus == 0)
                    {
                        result = true;
                        UpdateProfile(profile, test.Result);
                    }
                }

                sshClient.Disconnect();
            }

            return result;
        }

        private JobStatus ParseQstat(string output)
        {
            string[] tokens = output.Split(new char[] { ' ' });
            string status = "";
            if(tokens.Length >= 10)
                status = tokens[9]; //such is output format, deal with it

            JobStatus result;

            switch (status)
            {
                case "W":
                case "Q":
                    {
                        result = JobStatus.Queued;
                        break;
                    }
                case "R":
                case "E":
                    {
                        result = JobStatus.Running;
                        break;
                    }
                case "C":
                    {
                        result = JobStatus.Completed;
                        break;
                    }
                case "S":
                case "H":
                    {
                        result = JobStatus.Aborted;
                        break;
                    }

                default:
                    {
                        result = JobStatus.Processing;
                        break;
                    }
            }

            return result;
        }

        private void MakeSubmitScript(Job job)
        {
            StringBuilder scriptToBe = new StringBuilder();

            scriptToBe.Append("#!/bin/sh\n");

            scriptToBe.Append($"#PBS -N Statium.Remote.{job.Id}\n");

            if(job.ClusterProfile.QueueName != null)
                scriptToBe.Append($"#PBS -q {job.ClusterProfile.QueueName}\n");

            string procType = job.ClusterProfile.Cluster.LaunchType == "GPU" ? "gpu" : "ppn";
            int nodeNum = 4;
            int procNum = 4;
            string walltime = ConvertToWalltime(job.EstimatedTime);
            string memoryReq = "2000m";
            
            scriptToBe.Append($"#PBS -l nodes={nodeNum}:{procType}={procNum}:mem={memoryReq}:walltime={walltime}\n");

            scriptToBe.Append($"#PBS -o $PBS_O_HOME/Statium.Remote/{job.Id}/output.txt\n");

            scriptToBe.Append($"#PBS -e $PBS_O_HOME/Statium.Remote/{job.Id}/errors.txt\n");

            scriptToBe.Append($"cd ~/Statium.Remote/{job.Id}\n");

            scriptToBe.Append($"make\n");

            scriptToBe.Append($"mpirun -hostfile $PBS_NODEFILE -np {nodeNum} ./launcher\n");

            using (FileStream fs = File.Create(Path.Combine("C:/Users/username/Desktop/", "submit.sh")))
            {
                string scriptString = scriptToBe.ToString();
                byte[] scriptBytes = new UTF8Encoding(true).GetBytes(scriptString);
                fs.Write(scriptBytes, 0, scriptBytes.Length);
            }
        }

        private string ConvertToWalltime(int minutes)
        {
            int hours = minutes / 60;
            int minutesLeft = minutes % 60;
            return $"{hours}:{minutesLeft}:00";
        }

        private void UpdateProfile(ClusterProfile profile, string qstatOut)
        {

        }
    }
}
