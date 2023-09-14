namespace FKGame
{
    public struct OnLoginInfo
    {
        public bool isSuccess;
        public string accountId;
        public string nickName;
        public string headPortrait;
        public string password;
        public LoginPlatform loginPlatform;
        public LoginErrorEnum error;
        public string sdkError;
    }
}