using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class LoginGameController
    {
        public static CallBack<OnLoginInfo> OnSDKLoginCallBack;                         // ����sdk��¼����
        public static CallBack<UserLogin2Client> OnUserLogin;                           // ��������¼�ص�
        public static CallBack<UserLogout2Client> OnUserLogout;                         // �˳���¼�ص�
        public static CallBack<AskUserUseActivationCode2Client> OnAskUseActivationCode; // ��������Ҫ�����ʹ�öһ���
        private static bool isLogin = false;                                            // �Ƿ��ѵ�¼
        public static bool isClickLogin = false;                                        // �Ƿ��ѵ����¼��ť
        private static string activationCode;                                           // ������
        private const float c_reSendTimer = 3;                                          // ��Ϣ�ط����
        private static float reSendTimer = 0;
        private static UserLogin2Server loginMsg;

        // ��ҵ�½��SDkƽ̨
        public static LoginPlatform PlayerLoginPlatform                         
        {
            get;
            private set;
        }
        public static string ActivationCode
        {
            get { return activationCode; }
            set
            {
                activationCode = value;
                if (loginMsg != null)
                {
                    loginMsg.activationCode = activationCode;
                }
            }
        }
        public static bool IsLogin
        {
            get
            {
                return isLogin;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            GlobalEvent.AddTypeEvent<UserLogin2Client>(OnUserLoginEvent);
            GlobalEvent.AddTypeEvent<UserLogout2Client>(OnUserLogoutEvent);
            GlobalEvent.AddTypeEvent<AskUserUseActivationCode2Client>(AskUserUseActivationCode);
            ResendMessageManager.Init();
            AutoReconnectController.EndReconnect += ReLogin;
            AutoReconnectController.Init();
            ApplicationManager.s_OnApplicationUpdate += LogonUpdate;
        }

        // ����Ƿ�Ҫֱ�ӵ�¼��������Ǿ�ֱ�ӵ�¼��Ĭ��ѡ��ɸѡ��ĵ�0����¼��
        public static bool CheckAutoLoginOrGetLoginPlatforms(out List<LoginConfigData> datas)
        {
            datas = new List<LoginConfigData>();
            List<LoginConfigData> allConfigs = DataGenerateManager<LoginConfigData>.GetAllDataList();
            string sdkStr = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_LoginPlatform, "");
            if (!string.IsNullOrEmpty(sdkStr))
            {
                string[] arrStr = sdkStr.Split('|');
                foreach (var item in arrStr)
                {
                    LoginConfigData con = DataGenerateManager<LoginConfigData>.GetData(item);
                    if (con != null)
                    {
                        datas.Add(con);
                    }
                    else
                    {
                        Debug.LogError("��ȡ��¼����ʧ�ܣ�" + item);
                    }
                }
            }
            else
            {
                foreach (var d in allConfigs)
                {
                    List<string> platforms = null;
                    if (d.m_SupportPlatform == null)
                        platforms = new List<string>();
                    else
                        platforms = new List<string>(d.m_SupportPlatform);
                    if (d.m_UseItem || platforms.Contains(Application.platform.ToString()))
                    {
                        datas.Add(d);
                    }
                }
            }
            string directlyLoginStr = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_DirectlyLogin, "false");
            bool directlyLogin = bool.Parse(directlyLoginStr);
            if (directlyLogin)
            {
                LoginConfigData d = datas[0];
                Login(d.m_loginName, "", "", d.m_CustomInfo);
                return true;
            }
            return false;
        }

        /// ��¼
        public static void Login(LoginPlatform loginPlatform, string accountID = "", string pw = "", string custom = "")
        {
            SDKManager.LoginCallBack += SDKLoginCallBack;
            string tag = "";
            accountID = accountID.Trim();
            pw = pw.Trim();
            string pwMd5 = MD5Utils.GetObjectMD5(pw);
            tag = accountID + "|" + pwMd5 + "|" + custom;
            SDKManager.LoginByPlatform(loginPlatform, tag);
        }

        public static void ReLogin()
        {
            if (loginMsg == null)
            {
                return;
            }
            SendLoginMsg();
        }

        // �˳���¼
        public static void Logout()
        {
            UserLogout2Server msg = new UserLogout2Server();
            JsonMessageProcessingController.SendMessage(msg);
        }

        // ��ʱ�ط���¼��Ϣ
        private static void LogonUpdate()
        {
            if (IsLogin)
                return;
            if (!isClickLogin)
                return;
            if (reSendTimer > 0)
            {
                reSendTimer -= Time.deltaTime;
                if (reSendTimer < 0)
                {
                    NetworkManager.DisConnect();
                }
            }
        }

        private static void AskUserUseActivationCode(AskUserUseActivationCode2Client e, object[] args)
        {
            ResendMessageManager.startResend = false;
            isClickLogin = false;
            if (OnAskUseActivationCode != null)
            {
                OnAskUseActivationCode(e);
            }
        }

        private static void OnUserLogoutEvent(UserLogout2Client e, object[] args)
        {
            isLogin = false;
            isClickLogin = false;
            ResendMessageManager.startResend = false;
            loginMsg = null;
            SDKManager.LoginOut(PlayerLoginPlatform.ToString());
            if (OnUserLogout != null)
            {
                OnUserLogout(e);
            }
        }

        private static void OnUserLoginEvent(UserLogin2Client e, object[] args)
        {
            activationCode = "";
            if (e.code == 0)
            {
                isLogin = true;
                GameDataMonitor.PushData("User", e.user, "�������");
                SDKManager.UserID = e.user.userID;
            }
            if (OnUserLogin != null)
            {
                OnUserLogin(e);
            }
            if (e.reloginState)
                return;
            isClickLogin = false;
            if (e.code != 0)
            {
                Debug.LogError("Login error code:" + e.code);
                return;
            }
            ResendMessageManager.startResend = true;
            loginMsg.typeKey = e.user.typeKey;
            SDKManager.LogLogin(e.user.userID);
        }

        private static void SDKLoginCallBack(OnLoginInfo info)
        {
            SDKManager.LoginCallBack -= SDKLoginCallBack;
            if (OnSDKLoginCallBack != null)
            {
                OnSDKLoginCallBack(info);
            }
            if (info.isSuccess)
            {
                isClickLogin = true;
                PlayerLoginPlatform = info.loginPlatform;
                UserLogin2Server msg = UserLogin2Server.GetLoginMessage(info.loginPlatform, info.accountId, info.password, activationCode);
                SendLoginMsg(msg);
            }
        }

        // �����Զ��������Զ���¼
        private static void SendLoginMsg(UserLogin2Server msg = null)
        {
            reSendTimer = c_reSendTimer;
            bool reLoginState = false;
            if (msg != null)
                loginMsg = msg;
            else
            {
                if (loginMsg == null)
                    return;
                else
                {
                    if (IsLogin)
                        reLoginState = true;
                }
            }
            loginMsg.reloginState = reLoginState;
            Debug.Log("SendLoginMsg -->" + reLoginState);
            JsonMessageProcessingController.SendMessage(loginMsg);
        }
    }
}