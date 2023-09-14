using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UserLogin2Server
    {
        public LoginPlatform loginType;             // 登录平台
        public String typeKey;                      // 该平台返回的key，如游客登录使用设备号
        public String pw;                           // password
        public RuntimePlatform platform;
        public String deviceUniqueIdentifier;
        public SystemLanguage deviceSystemLanguage = SystemLanguage.Unknown;
        public SystemLanguage clientUseLanguage;    // 客户端多语言使用的语言
        public String clientVersion = "";
        public string activationCode;               // 激活码
        public bool reloginState = false;           // 标记是否是重连

        public static UserLogin2Server GetLoginMessage(LoginPlatform loginType, String typeKey, String pw, string activationCode)
        {
            UserLogin2Server msg = new UserLogin2Server();
            msg.loginType = loginType;
            msg.typeKey = typeKey;
            msg.pw = pw;
            msg.activationCode = activationCode;
            msg.deviceUniqueIdentifier = SystemInfoManager.deviceUniqueIdentifier;
            msg.platform = Application.platform;
            msg.deviceSystemLanguage = Application.systemLanguage;
            msg.clientVersion = ApplicationManager.Version;
            msg.clientUseLanguage = LanguageManager.CurrentLanguage;
            return msg;
        }
    }
}