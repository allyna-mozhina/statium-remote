using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statium.Alpha.Remote.Launcher.RemoteAccess
{
    public class RemoteAccessorFactory
    {
        public RemoteAccessorFactory(string configFile)
        {

        }

        public IRemoteSystemAccessor GetAccessor(string type)
        {
            if(type == "Linux")
                return new LinuxServerAccessor();

            if (type == "PBS")
                return new PbsAccessor();

            return null;
        }

        private Dictionary<string, IRemoteSystemAccessor> accessors;
    }
}
