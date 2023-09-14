using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class INetPluginBase
    {
        protected NetClientManager s_network;
        public void SetNetwork(NetClientManager s_network)
        {
            this.s_network = s_network;
        }
        public virtual void Init(params object[] paramArray) { }
        public virtual void Update() { }
        public virtual void OnConnectStateChange(NetworkState status) { }
        public virtual void OnReceiveMsg(NetworkMessage message) { }
        public virtual void OnSendMsg(string messageType, Dictionary<string, object> data) { }
        public virtual void OnDispose() { }
    }
}