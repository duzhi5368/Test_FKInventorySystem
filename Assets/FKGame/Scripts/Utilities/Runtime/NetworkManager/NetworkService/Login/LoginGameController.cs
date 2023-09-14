using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class LoginGameController
    {
        public static CallBack<OnLoginInfo> OnSDKLoginCallBack;                         // 调用sdk登录返回
        public static CallBack<UserLogin2Client> OnUserLogin;                           // 服务器登录回调
        public static CallBack<UserLogout2Client> OnUserLogout;                         // 退出登录回调
        public static CallBack<AskUserUseActivationCode2Client> OnAskUseActivationCode; // 当服务器要求玩家使用兑换码
        private static bool isLogin = false;                                            // 是否已登录
        public static bool isClickLogin = false;                                        // 是否已点击登录按钮
        private static string activationCode;                                           // 激活码
        private const float c_reSendTimer = 3;                                          // 消息重发间隔
        private static float reSendTimer = 0;
        private static UserLogin2Server loginMsg;

        // 玩家登陆的SDk平台
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

        // 检查是否要直接登录，如果是那就直接登录（默认选择筛选后的第0个登录）
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
                        Debug.LogError("获取登录配置失败：" + item);
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

        /// 登录
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

        // 退出登录
        public static void Logout()
        {
            UserLogout2Server msg = new UserLogout2Server();
            JsonMessageProcessingController.SendMessage(msg);
        }

        // 按时重发登录消息
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
                GameDataMonitor.PushData("User", e.user, "玩家数据");
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

        // 用于自动重连后自动登录
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