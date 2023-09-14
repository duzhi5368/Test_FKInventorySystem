namespace FKGame
{
    public class NetHeartBeatPongPlugin : INetMsgProcessPluginBase
    {
        private byte[] sendBytes = null;

        public override byte GetNetProperty()
        {
            return (byte)NetProperty.HeartBeatClinetSend;
        }

        protected override void OnInit()
        {
            sendBytes = MsgPackest.Write2Bytes(networkCommon.Configuration.byteOrder, 0, 0, 0, (byte)NetProperty.HeartBeatServerSend, new byte[0]);
        }
        public override void ReceveProcess(MsgPackest packest)
        {
            networkCommon.Sendbytes(packest.session, sendBytes);
        }
        public override void Release()
        {

        }
    }
}