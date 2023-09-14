using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AccountMergeInfo2Server
    {
        public LoginPlatform loginType;         // 登录平台
        public String typeKey;                  // 该平台返回的key，如游客登录使用设备号
        public String pw;                       // 密码
        public RuntimePlatform platform;        // 运行平台（Windows, Android, IOS, web等等）
        public String deviceUniqueIdentifier;   // 设备唯一标识
        public SystemLanguage deviceSystemLanguage = SystemLanguage.Unknown;

        public static AccountMergeInfo2Server GetMessage(LoginPlatform loginType, String typeKey, string pw)
        {
            AccountMergeInfo2Server msg = new AccountMergeInfo2Server();
            msg.loginType = loginType;
            msg.typeKey = typeKey;
            msg.pw = pw;
            msg.deviceUniqueIdentifier = SystemInfoManager.deviceUniqueIdentifier;
            msg.platform = Application.platform;
            msg.deviceSystemLanguage = Application.systemLanguage;
            return msg;
        }
    }
}