namespace FKGame
{
    public class TouristLoginSDKClass : LoginInterface
    {
        public override LoginPlatform GetLoginPlatform()
        {
            return LoginPlatform.Tourist;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Login(string tage)
        {
            OnLoginInfo info = new OnLoginInfo();
            info.accountId = SystemInfoManager.deviceUniqueIdentifier;
            info.isSuccess = true;
            LoginCallBack(info);
        }
    }
}