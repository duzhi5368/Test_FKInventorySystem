namespace FKGame
{
    public class UserLogin2Client : CodeMessageBase
    {
        public User user;
        public bool newPlayerState;                     // 是否是新玩家登录
        public bool reloginState = false;               // 标记是否是重连
        public bool supportCompressMsg = false;         // 服务器是否支持消息压缩

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}