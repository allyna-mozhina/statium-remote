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
    public class LinuxServerAccessor : IRemoteSystemAccessor
    {
        public JobStatus CheckJob(Job job, string remote_id)
        {
            ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "prochkin", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("prochkin", "youcantfeeltheheat")
                });

            JobStatus result = JobStatus.Processing;

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand poll = sshClient.CreateCommand($"ls ~/Statium.Remote/{job.Id}/result.csv"))
                {
                    poll.Execute();
                    /*string*/int pollOut = poll.ExitStatus;//poll.Result;
                    result = ParsePoll(pollOut);
                }

                sshClient.Disconnect();
            }

            return result;
        }

        public int[] FetchResults(Job job, string remote_id)
        {
            ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "prochkin", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("prochkin", "youcantfeeltheheat")
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
                        scpClient.Download(string.Format("Statium.Remote/{0}/result.csv", job.Id), new FileInfo(Path.Combine("C:\\Users\\username\\Desktop", "result.csv")));
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
                22, "prochkin", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("prochkin", "youcantfeeltheheat")
                });

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand killCommand = sshClient.CreateCommand(string.Format("kill {0}", job.Id)))
                {
                    killCommand.Execute();

                    if (killCommand.ExitStatus != 0)
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
            ConnectionInfo connInfo = new ConnectionInfo(job.ClusterProfile.Cluster.Url,
                22, "prochkin", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("prochkin", "youcantfeeltheheat")
                });

            string result;
            
            using (SshClient sshClient = new SshClient(connInfo))
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

                using (SshCommand make = sshClient.CreateCommand(
                    string.Format("g++ {0}cm_launcher.cpp {0}cmodule_api.cpp {0}cmodule.cpp {0}csv_io.cpp {0}simple_containers.cpp -o {0}launcher",
                    string.Format("~/Statium.Remote/{0}/", job.Id))))
                {
                    make.Execute();

                    if (make.ExitStatus != 0)
                        return null;
                }

                using (SshCommand start = sshClient.CreateCommand(
                    string.Format("{0}launcher 14 > {0}result.csv",
                    string.Format("~/Statium.Remote/{0}/", job.Id))))
                {
                    start.Execute();

                    if (start.ExitStatus != 0)
                        return null;
                }

                sshClient.Disconnect();
            }

            result = job.Id.ToString();
            return result;
        }

        public bool TestAccess(ClusterProfile profile)
        {
            ConnectionInfo connInfo = new ConnectionInfo(profile.Cluster.Url,
                22, "prochkin", new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod("prochkin", "youcantfeeltheheat")
                });

            bool result = false;

            using (SshClient sshClient = new SshClient(connInfo))
            {
                sshClient.Connect();

                using (SshCommand echo = sshClient.CreateCommand("echo TOTALLY ALIVE"))
                {
                    echo.Execute();

                    if (echo.ExitStatus == 0 && echo.Result == "TOTALLY ALIVE\n")
                        result = true;
                }

                sshClient.Disconnect();
            }

            return result;
        }

        private JobStatus ParsePoll(/*string*/int pollOut)
        {
            if (pollOut == 0)
                return JobStatus.Completed;
            else
                return JobStatus.Queued;
        }
    }
}
