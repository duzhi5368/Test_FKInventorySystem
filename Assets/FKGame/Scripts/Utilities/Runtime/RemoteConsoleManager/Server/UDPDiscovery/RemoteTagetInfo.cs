using System.Net;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RemoteTagetInfo
    {
        public RemoteDeviceInfo info;
        public IPAddress address;
        public IPAddress addressV6;
        public int port;
        public float timeOut = 1.5f;

        public RemoteTagetInfo(RemoteDeviceInfo info)
        {
            this.info = info;
        }

        public IPAddress GetIPAddress()
        {
            if (address != null)
                return address;
            else
                return addressV6;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is RemoteTagetInfo)
            {
                RemoteTagetInfo other = (RemoteTagetInfo)obj;
                if (info.Equals(other.info) && port == other.port)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "Name:" + info.appName + " Ip:" + address + "," + addressV6 + " port:" + port;
        }
    }
}