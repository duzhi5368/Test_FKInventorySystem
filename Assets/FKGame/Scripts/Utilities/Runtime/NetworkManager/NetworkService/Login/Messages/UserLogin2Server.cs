using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UserLogin2Server
    {
        public LoginPlatform loginType;             // ��¼ƽ̨
        public String typeKey;                      // ��ƽ̨���ص�key�����ο͵�¼ʹ���豸��
        public String pw;                           // password
        public RuntimePlatform platform;
        public String deviceUniqueIdentifier;
        public SystemLanguage deviceSystemLanguage = SystemLanguage.Unknown;
        public SystemLanguage clientUseLanguage;    // �ͻ��˶�����ʹ�õ�����
        public String clientVersion = "";
        public string activationCode;               // ������
        public bool reloginState = false;           // ����Ƿ�������

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