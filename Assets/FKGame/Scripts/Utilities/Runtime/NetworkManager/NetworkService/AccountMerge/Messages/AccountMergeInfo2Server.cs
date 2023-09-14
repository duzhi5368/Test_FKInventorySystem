using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AccountMergeInfo2Server
    {
        public LoginPlatform loginType;         // ��¼ƽ̨
        public String typeKey;                  // ��ƽ̨���ص�key�����ο͵�¼ʹ���豸��
        public String pw;                       // ����
        public RuntimePlatform platform;        // ����ƽ̨��Windows, Android, IOS, web�ȵȣ�
        public String deviceUniqueIdentifier;   // �豸Ψһ��ʶ
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