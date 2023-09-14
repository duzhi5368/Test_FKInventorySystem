namespace FKGame
{
    [System.Serializable]
    public class LoginInterface : SDKInterfaceBase
    {
        public virtual LoginPlatform GetLoginPlatform()
        {
            return LoginPlatform.Tourist;
        }
        public override void Init()
        {
            base.Init();
        }
        public virtual void Login(string tag){}
        public virtual void LoginOut(string tag){}

        protected void LoginCallBack(OnLoginInfo info)
        {
            info.loginPlatform = GetLoginPlatform();
            if (SDKManager.LoginCallBack != null)
                SDKManager.LoginCallBack(info);
        }
    }
}