using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UserRegister2Client : CodeMessageBase
    {
        public LoginPlatform loginType;
        public String typeKey;

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}