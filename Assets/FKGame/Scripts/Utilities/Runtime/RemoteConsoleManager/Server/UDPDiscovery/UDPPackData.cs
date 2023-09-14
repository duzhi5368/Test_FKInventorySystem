using System.Net;
//------------------------------------------------------------------------
namespace FKGame
{
    public struct UDPPackData
    {
        public IPEndPoint iPEndPoint;
        public string data;

        public UDPPackData(IPEndPoint iPEndPoint, string data)
        {
            this.iPEndPoint = iPEndPoint;
            this.data = data;
        }
    }
}