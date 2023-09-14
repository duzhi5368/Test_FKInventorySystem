namespace FKGame
{
    // Ping����(�����)
    public class NetPongPlugin : INetMsgProcessPluginBase
    {
        public override byte GetNetProperty()
        {
            return (byte)NetProperty.Ping;
        }

        protected override void OnInit()
        {

        }
        public override void ReceveProcess(MsgPackest packest)
        {
            byte[] sendBytes = MsgPackest.Write2Bytes(networkCommon.Configuration.byteOrder, 0, 0, 0, (byte)NetProperty.Pong, packest.contents);
            networkCommon.Sendbytes(packest.session, sendBytes);
        }
        public override void Release()
        {

        }
    }
}