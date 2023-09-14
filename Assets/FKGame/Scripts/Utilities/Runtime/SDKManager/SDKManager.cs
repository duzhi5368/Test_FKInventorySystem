using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class SDKManager
    {
#if UNITY_ANDROID
        public const string c_ConfigName = "SDKConfig_Android";
#elif UNITY_IOS
        public const string c_ConfigName = "SDKConfig_IOS";
#else
        public const string c_ConfigName = "SDKConfig";
#endif
        public const string c_KeyName = "SDKInfo";

        public static string UserID;
        private static bool isInit = false;
        private static List<LoginInterface> s_loginServiceList = null;
        private static List<PayInterface> s_payServiceList = null;
        private static List<ADInterface> s_ADServiceList = null;
        private static List<LogInterface> s_logServiceList = null;
        private static List<RealNameInterface> s_shareServiceList = null;
        private static List<OtherSDKInterface> s_otherServiceList = null;
        private static List<RealNameInterface> s_realNameServiceList = null;
        private static PayCallBack s_payCallBack;
        private static ADCallBack s_adCallBack;
        private static GoodsInfoCallBack s_goodsInfoCallBack;
        private static RealNameCallBack s_realNameCallBack;
        private static PayLimitCallBack s_payLimitCallBack;
        private static RealNameLogoutCallBack s_RealNameLogoutCallBack;
        private static Dictionary<string, OtherCallBack> s_callBackDict = new Dictionary<string, OtherCallBack>();
        private static bool s_useNewSDKManager = false;                                                     // �Ƿ�ʹ���°汾SDKManager
        private static List<PayPlatformInfo> allPayPlatformInfos;
        public static Dictionary<string, string> propertiesNoRepackage = new Dictionary<string, string>();  // �Ǿ����ش�� �������õ���������

        public static LoginCallBack LoginCallBack { get; set; }

        public static PayCallBack PayCallBack
        {
            get
            {
                return s_payCallBack;
            }

            set
            {
                s_payCallBack = value;
            }
        }
        public static ADCallBack ADCallBack
        {
            get
            {
                return s_adCallBack;
            }

            set
            {
                s_adCallBack = value;
            }
        }
        public static GoodsInfoCallBack GoodsInfoCallBack
        {
            get
            {
                return s_goodsInfoCallBack;
            }

            set
            {
                s_goodsInfoCallBack = value;
            }
        }
        public static RealNameCallBack RealNameCallBack
        {
            get
            {
                return s_realNameCallBack;
            }
            set
            {
                s_realNameCallBack = value;
            }
        }
        public static PayLimitCallBack PayLimitCallBack
        {
            get
            {
                return s_payLimitCallBack;
            }

            set
            {
                s_payLimitCallBack = value;
            }
        }
        public static RealNameLogoutCallBack RealNameLogoutCallBack
        {
            get
            {
                return s_RealNameLogoutCallBack;
            }

            set
            {
                s_RealNameLogoutCallBack = value;
            }
        }

        // ��ʼ��
        public static void Init()
        {
            if (!isInit)
            {
                isInit = true;
                try
                {
                    if (ConfigManager.GetIsExistConfig(c_ConfigName))
                    {
                        SchemeData data = LoadGameSchemeConfig();
                        s_useNewSDKManager = data.UseNewSDKManager;
                        if (s_useNewSDKManager)
                        {
                            SDKManagerNew.Init();
                        }
                        Debug.Log("SDKManager Init");
                        LoadService(data);
                        InitSDK();
                        AutoListenerInit();
                        RealNameManager.GetInstance().Init();     //��ʼ��ʵ����ϵͳ
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Init Exception: " + e.ToString());
                }
            }
        }

        // �����ʼ����SDKName == nullʱ��ʼ��ȫ��
        public static void ExtraInit(SDKType type, string sdkName = null, string tag = null)
        {
            if (sdkName != null)
            {
                switch (type)
                {
                    case SDKType.AD:
                        GetADService(sdkName).ExtraInit(tag); break;
                    case SDKType.Log:
                        GetLogService(sdkName).ExtraInit(tag); break;
                    case SDKType.Login:
                        GetLoginService(sdkName).ExtraInit(tag); break;
                    case SDKType.Other:
                        GetOtherService(sdkName).ExtraInit(tag); break;
                    case SDKType.Pay:
                        GetPayService(sdkName).ExtraInit(tag); break;
                    case SDKType.RealName:
                        GetPayService(sdkName).ExtraInit(tag); break;
                }
            }
            else
            {
                switch (type)
                {
                    case SDKType.AD:
                        AllExtraInit(s_ADServiceList, tag); break;
                    case SDKType.Log:
                        AllExtraInit(s_logServiceList, tag); break;
                    case SDKType.Login:
                        AllExtraInit(s_loginServiceList, tag); break;
                    case SDKType.Other:
                        AllExtraInit(s_otherServiceList, tag); break;
                    case SDKType.Pay:
                        AllExtraInit(s_payServiceList, tag); break;
                    case SDKType.RealName:
                        AllExtraInit(s_realNameServiceList, tag); break;
                }
            }
        }

        static void AllExtraInit<T>(List<T> list, string tag = null) where T : SDKInterfaceBase
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i].ExtraInit(tag);
                }
                catch (Exception e)
                {
                    Debug.LogError("AllExtraInit Exception " + list[i].m_SDKName + " " + e.ToString());
                }
            }
        }

        static void CheckInit()
        {
            if (!isInit)
            {
                throw new Exception("SDKManager not init !");
            }
        }

        public static LoginInterface GetLoginService<T>() where T : LoginInterface
        {
            return GetLoginService(typeof(T).Name);
        }

        public static LoginInterface GetLoginService(string SDKName)
        {
            return GetSDKService(s_loginServiceList, SDKName);
        }

        public static LoginInterface GetLoginService(int index)
        {
            if (s_loginServiceList.Count <= index)
            {
                throw new Exception("GetLoginService error index->" + index + " count->" + s_loginServiceList.Count);
            }
            return s_loginServiceList[index];
        }

        public static PayInterface GetPayService<T>() where T : PayInterface
        {
            return GetPayService(typeof(T).Name);
        }

        public static PayInterface GetPayService(string SDKName)
        {
            return GetSDKService(s_payServiceList, SDKName);
        }

        public static bool GetHasPayService(string SDKName)
        {
            return GetHasSDKService(s_payServiceList, SDKName);
        }

        public static PayInterface GetPayService(int index)
        {
            if (s_payServiceList.Count <= index)
            {
                throw new Exception("GetPayService error index->" + index + " count->" + s_payServiceList.Count);
            }
            return s_payServiceList[index];
        }

        public static RealNameInterface GetRealNameService(int index)
        {
            if (s_realNameServiceList.Count <= index)
            {
                throw new Exception("GetRealNameService error index->" + index + " count->" + s_realNameServiceList.Count);
            }
            return s_realNameServiceList[index];
        }

        public static ADInterface GetADService<T>() where T : ADInterface
        {
            return GetADService(typeof(T).Name);
        }

        public static ADInterface GetADService(string SDKName)
        {
            return GetSDKService(s_ADServiceList, SDKName);
        }

        public static ADInterface GetADService(int index)
        {
            if (s_ADServiceList.Count <= index)
            {
                throw new Exception("GetADService error index->" + index + " count->" + s_ADServiceList.Count);
            }
            return s_ADServiceList[index];
        }

        public static LogInterface GetLogService<T>() where T : LogInterface
        {
            return GetLogService(typeof(T).Name);
        }

        public static LogInterface GetLogService(string SDKName)
        {
            return GetSDKService(s_logServiceList, SDKName);
        }

        public static LogInterface GetLogService(int index)
        {
            if (s_logServiceList.Count <= index)
            {
                throw new Exception("GetLogService error index->" + index + " count->" + s_logServiceList.Count);
            }
            return s_logServiceList[index];
        }

        public static OtherSDKInterface GetOtherService<T>() where T : OtherSDKInterface
        {
            return GetOtherService(typeof(T).Name);
        }

        public static OtherSDKInterface GetOtherService(string SDKName)
        {
            return GetSDKService(s_otherServiceList, SDKName);
        }

        public static OtherSDKInterface GetOtherService(int index)
        {
            if (s_otherServiceList.Count <= index)
            {
                throw new Exception("GetOtherService error index->" + index + " count->" + s_otherServiceList.Count);
            }

            return s_otherServiceList[index];
        }

        static void InitLogin(List<LoginInterface> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i].Init();
                }
                catch (Exception e)
                {
                    Debug.LogError("Init LoginInterface SDK Exception:\n" + e.ToString());
                }
            }
        }

        // ��½,Ĭ�Ϸ��ʵ�һ���ӿ�
        public static void Login(string tag = "")
        {
            if (s_useNewSDKManager)
            {
                SDKManagerNew.Login();
            }
            else
            {
                try
                {
                    GetLoginService(0).Login(tag);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Login Exception: " + e.ToString());
                }
            }
        }

        public static void LoginOut(string SDKName = null, String tag = null)
        {
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LoginOut(SDKName, tag);
            }
            else
            {
                try
                {
                    GetLoginService(SDKName).LoginOut(tag);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Login Exception: " + SDKName + "===" + e.ToString());
                }
            }
        }

        // ��½
        public static void LoginBySDKName(string SDKName, string tag = "")
        {
            if (s_useNewSDKManager)
            {
                SDKManagerNew.Login(SDKName, tag);
            }
            else
            {
                try
                {
                    GetLoginService(SDKName).Login(tag);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Login Exception: " + SDKName + "===" + e.ToString());
                }
            }
        }

        // ��½
        public static void LoginByPlatform(LoginPlatform loginPlatform, string tag = "")
        {
            try
            {
                foreach (var item in s_loginServiceList)
                {
                    if (item.GetPlatform().Contains(Application.platform) && item.GetLoginPlatform() == loginPlatform)
                    {
                        item.Login(tag);
                        return;
                    }
                }
                if (s_useNewSDKManager)
                {
                    Debug.LogWarning(loginPlatform);
                    SDKManagerNew.Login(loginPlatform.ToString(), tag);
                }
                else
                {
                    Debug.LogError("SDKManager Login dont find class by platform:" + Application.platform + " loginPlatform:" + loginPlatform);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager Login Exception: " + e.ToString());
            }
        }
        public static List<LoginPlatform> GetSupportLoginPlatform()
        {
            List<LoginPlatform> platforms = new List<LoginPlatform>();
            try
            {
                foreach (var item in s_loginServiceList)
                {
                    if (item.GetPlatform().Contains(Application.platform))
                    {
                        platforms.Add(item.GetLoginPlatform());
                    }
                }
                if (s_useNewSDKManager)
                {
                    List<LoginPlatform> newList = SDKManagerNew.GetSupportLoginPlatform();
                    for (int i = 0; i < newList.Count; i++)
                    {
                        if (!platforms.Contains(newList[i]))
                        {
                            platforms.Add(newList[i]);
                        }
                    }
                }
                if (platforms.Count == 0)
                {
                    Debug.LogError("SDKManager Login dont find class by platform:" + Application.platform + " please check config");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager Login Exception: " + e.ToString());
            }
            return platforms;
        }

        // ��ȡ֧�ֵĵ�¼ƽ̨
        public static List<string> GetLoginPlatformsBySDKTool(List<string> defaultValue)
        {
            if (!Application.isEditor && Application.platform == RuntimePlatform.Android)
            {
                List<string> result = new List<string>();
                string sdkStr = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_LoginPlatform, "");
                if (string.IsNullOrEmpty(sdkStr))
                {
                    return defaultValue;
                }
                else
                {
                    string[] arrStr = sdkStr.Split('|');
                    for (int i = 0; i < arrStr.Length; i++)
                    {
                        result.Add(arrStr[i]);
                    }
                    return result;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        static void InitPay(List<PayInterface> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i].Init();
                }
                catch (Exception e)
                {
                    Debug.LogError("Init PayInterface SDK Exception:\n" + e.ToString() + " " + list[i].m_SDKName);
                }
            }
            if (s_useNewSDKManager)
            {
                PublicPayClass publicPayInterface = new PublicPayClass();
                publicPayInterface.Init();
                list.Add(publicPayInterface);
            }
        }

        // ֧��,Ĭ�Ϸ��ʵ�һ���ӿ�
        public static void Pay(PayInfo payInfo)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_payServiceList.Count > 0)
            {
                try
                {
                    GetPayService(0).Pay(payInfo);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Pay Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                if (GetPrePay(payInfo.storeName.ToString()))
                {
                    try
                    {
                        GetPayService("PublicPayClass").Pay(payInfo);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("SDKManager PublicPayClass Exception: " + e.ToString());
                    }
                }
                else
                {
                    SDKManagerNew.Pay(payInfo);
                }
            }
            else
            {
                Debug.Log("֧��SDK û�����ã� ");
            }
        }

        // ֧��
        public static void Pay(string SDKName, PayInfo payInfo)
        {
            Debug.Log("Pay  SDKname " + SDKName + " GetHasSDKService " + GetHasSDKService(s_payServiceList, SDKName));
            // ����ʹ�ñ������õ�SDK
            if (GetHasSDKService(s_payServiceList, SDKName))
            {
                try
                {
                    GetPayService(SDKName).Pay(payInfo);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Pay Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                if (GetPrePay(SDKName))
                {
                    try
                    {
                        GetPayService("PublicPayClass").Pay(payInfo);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("SDKManager PublicPayClass Exception: " + e.ToString());
                    }
                }
                else
                {
                    SDKManagerNew.Pay(payInfo);
                }
            }
            else
            {
                Debug.LogError("֧��SDK û�����ã� ");
            }
        }

        // ֧��
        public static void ConfirmPay(string SDKName, string goodID, string tag = "")
        {
            try
            {
                if (GetHasPayService(SDKName))
                {
                    GetPayService(SDKName).ConfirmPay(goodID, tag, SDKName);
                }
                else
                {
                    GetPayService<PublicPayClass>().ConfirmPay(goodID, tag, SDKName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager Pay Exception: " + e.ToString());
            }
        }

        public static LocalizedGoodsInfo GetGoodsInfo(string goodsID, string tag = "")
        {
            try
            {
                return GetPayService(0).GetGoodsInfo(goodsID);
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager GetGoodsInfo Exception: " + e.ToString());
            }
            return null;
        }

        public static LocalizedGoodsInfo GetGoodsInfo(string SDKName, string goodsID, string tag = "")
        {
            // ����ʹ�ñ������õ�SDK
            if (GetHasSDKService(s_payServiceList, SDKName))
            {
                try
                {
                    return GetPayService(SDKName).GetGoodsInfo(goodsID);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager GetGoodsInfo Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                try
                {
                    GetPayService("PublicPayClass").GetGoodsInfo(goodsID);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager GetGoodsInfo Exception: " + e.ToString());
                }
            }
            else
            {
                Debug.LogError("֧��SDK û�����ã� ");
            }
            return null;
        }

        public static List<LocalizedGoodsInfo> GetAllGoodsInfo(string tag = "")
        {
            try
            {
                return GetPayService(0).GetAllGoodsInfo();
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager GetGoodsInfo Exception: " + e.ToString());
            }
            return null;
        }

        public static List<LocalizedGoodsInfo> GetAllGoodsInfo(string SDKName, string tag = "")
        {
            try
            {
                return GetPayService(SDKName).GetAllGoodsInfo();
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager GetGoodsInfo Exception: " + e.ToString());
            }
            return null;
        }

        public static bool GetPrePay(String SDKName)
        {
            return SDKManagerNew.GetPrePay(SDKName);
        }

        public static bool GetReSendPay(String SDKName)
        {
            return SDKManagerNew.GetReSendPay(SDKName);
        }

        // ��ȡ����֧��ƽ̨��Ϣ
        public static List<PayPlatformInfo> GetAllPayPlatformInfos()
        {
            if (allPayPlatformInfos == null)
            {
                string defaultValue = "None";
                if (s_payServiceList.Count > 0)
                {
                    defaultValue = "";
                    for (int i = 0; i < s_payServiceList.Count; i++)
                    {
                        if (s_payServiceList[i].GetType() == typeof(PublicPayClass))
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            defaultValue += "|";
                        }
                        defaultValue += s_payServiceList[i].m_SDKName;
                        Debug.LogWarning(defaultValue + s_payServiceList[i].GetType());
                    }
                }
                string payPlatformValueFromSDK = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_StoreName, defaultValue);
                Debug.Log("payPlatformValueFromSDK:" + payPlatformValueFromSDK + " defaultValue:" + defaultValue + "==" + s_payServiceList.Count);
                allPayPlatformInfos = PayPlatformInfo.GetAllPayPlatform(payPlatformValueFromSDK);
            }
            return allPayPlatformInfos;
        }

        // �жϵ�ǰ֧�������Ƿ����ĳ֧����ʽ
        public static bool IncludeThePayPlatform(StoreName storeName)
        {
            List<PayPlatformInfo> allPayPlatforms = GetAllPayPlatformInfos();
            for (int i = 0; i < allPayPlatforms.Count; i++)
            {
                if (allPayPlatforms[i].SDKName == storeName.ToString())
                {
                    Debug.Log("IncludeThePayPlatform:" + storeName);
                    return true;
                }
            }
            return false;
        }

        // ��ȡ������õĹ��sdkName
        public static string GetNowADSDKName()
        {
            string sdkName = GetProperties(SDKInterfaceDefine.PropertiesKey_ADPlatform, null);
            if (string.IsNullOrEmpty(sdkName) && s_ADServiceList.Count > 0)
            {
                return s_ADServiceList[0].m_SDKName;
            }
            return sdkName;
        }

        // ���ع��,Ĭ�Ϸ��ʵ�һ���ӿ�
        public static void LoadAD(ADType adType, string tag = "")
        {
            // ��ȡע������
            string sdkName = GetProperties(SDKInterfaceDefine.PropertiesKey_ADPlatform, null);
            LoadAD(sdkName, adType, tag);
        }

        // ���ع��
        public static void LoadAD(string SDKName, ADType adType, string tag = "")
        {
            if (string.IsNullOrEmpty(SDKName) && s_ADServiceList.Count > 0)
            {
                s_ADServiceList[0].LoadAD(adType, tag);
            }
            else if (GetHasSDKService(s_ADServiceList, SDKName))
            {
                try
                {
                    GetADService(SDKName).LoadAD(adType, tag);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Pay Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                SDKManagerNew.LoadAD(SDKName, adType, tag);
            }
            else
            {
                Debug.LogError("���SDK û�����ã� ");
            }
        }

        // ����Ѽ��سɹ�,Ĭ�Ϸ��ʵ�һ���ӿ�
        public static bool ADIsLoaded(ADType adType, string tag = "")
        {
            //��ȡע������
            string sdkName = GetProperties(SDKInterfaceDefine.PropertiesKey_ADPlatform, null);
            return ADIsLoaded(sdkName, adType, tag);
        }

        // ���ع��ɹ�
        public static bool ADIsLoaded(string SDKName, ADType adType, string tag = "")
        {
            try
            {
                if (string.IsNullOrEmpty(SDKName) && s_ADServiceList.Count > 0)
                {
                    return s_ADServiceList[0].IsLoaded(adType, tag);
                }
                else if (GetHasSDKService(s_ADServiceList, SDKName))
                {
                    return GetADService(SDKName).IsLoaded(adType, tag);
                }
                else
                {
                    if (s_useNewSDKManager)
                    {
                        return SDKManagerNew.ADIsLoaded(SDKName, adType, tag); ;
                    }
                    else
                    {
                        Debug.LogError("SDKManager ADIsLoaded Not find: " + SDKName);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager ADIsLoaded Exception: " + e.ToString());
                return false;
            }
        }

        // ��ʾ���
        public static void PlayAD(ADType adType, string tag = "")
        {
            string sdkName = GetProperties(SDKInterfaceDefine.PropertiesKey_ADPlatform, null);
            PlayAD(sdkName, adType, tag);
        }

        // ��ʾ���
        public static void PlayAD(string SDKName, ADType adType, string tag = "")
        {
            Debug.LogWarning("======PlayAD==" + SDKName + "===adType==" + adType + "===" + tag);
            try
            {
                if (string.IsNullOrEmpty(SDKName) && s_ADServiceList.Count > 0)
                {
                    s_ADServiceList[0].PlayAD(adType, tag);
                }
                else if (GetHasSDKService(s_ADServiceList, SDKName))
                {
                    GetADService(SDKName).PlayAD(adType, tag);
                }
                else
                {
                    if (s_useNewSDKManager)
                    {
                        SDKManagerNew.PlayAD(SDKName, adType, tag);
                    }
                    else
                    {
                        Debug.LogError("SDKManager PlayAD Not find: " + SDKName);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SDKManager PlayAD Exception: " + e.ToString());
            }
        }

        // ���ع��
        public static void CloseAD(ADType adType, string tag = "")
        {
            string sdkName = GetProperties(SDKInterfaceDefine.PropertiesKey_ADPlatform, null);
            CloseAD(sdkName, adType, tag);
        }

        // ���ع��
        public static void CloseAD(string SDKName, ADType adType, string tag = "")
        {
            if (string.IsNullOrEmpty(SDKName) && s_ADServiceList.Count > 0)
            {
                s_ADServiceList[0].CloseAD(adType, tag);
            }
            else if (GetHasSDKService(s_ADServiceList, SDKName))
            {
                GetADService(SDKName).CloseAD(adType, tag);
            }
            else
            {
                if (s_useNewSDKManager)
                {
                    SDKManagerNew.CloseAD(SDKName, adType, tag);
                }
                else
                {
                    Debug.LogError("SDKManager CloseAD Not find: " + SDKName);
                }
            }
        }

        // �����ϱ�
        public static void Log(string eventID, Dictionary<string, string> data)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.Log(eventID, data);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].Log(eventID, data);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager Log Exception: " + e.ToString());
                }
            }
        }

        public static void LogLogin(string accountID, Dictionary<string, string> data = null)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogLogin(accountID, data);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogLogin(accountID, data);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager LogLogin Exception: " + e.ToString());
                }
            }
        }

        public static void LogLoginOut(string accountID)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogLoginOut(accountID);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogLoginOut(accountID);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager LogLoginOut Exception: " + e.ToString());
                }
            }
        }

        public static void LogPay(string orderID, string goodsID, int count, float price, string currency, string payment)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogPay(orderID, goodsID, count, price, currency, payment);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogPay(orderID, goodsID, count, price, currency, payment);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager LogPay Exception: " + e.ToString());
                }
            }
        }

        public static void LogPaySuccess(string orderID)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogPaySuccess(orderID);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogPaySuccess(orderID);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager LogPaySuccess Exception: " + e.ToString());
                }
            }
        }

        // �����������ʹ�ã�����׷��������Ʒ�Ĳ�������
        public static void LogRewardVirtualCurrency(float count, string reason)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogRewardVirtualCurrency(count, reason);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogRewardVirtualCurrency(count, reason);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager RewardVirtualCurrency Exception: " + e.ToString());
                }
            }
        }

        public static void LogPurchaseVirtualCurrency(string goodsID, int num, float price)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogPurchaseVirtualCurrency(goodsID, num, price);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogPurchaseVirtualCurrency(goodsID, num, price);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager PurchaseVirtualCurrency Exception: " + e.ToString());
                }
            }
        }

        public static void LogUseItem(string goodsID, int num)
        {
            CheckInit();
            if (s_useNewSDKManager)
            {
                SDKManagerNew.LogUseItem(goodsID, num);
            }
            for (int i = 0; i < s_logServiceList.Count; i++)
            {
                try
                {
                    s_logServiceList[i].LogUseItem(goodsID, num);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager LogUseItem Exception: " + e.ToString());
                }
            }
        }

        // ʵ���Ƶ�¼
        static public void RealNameLogin(string userID, string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    GetRealNameService(0).OnLogin(userID);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager RealNameLogin Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                SDKManagerNew.RealNameOnLogin(userID, SDKName, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
            }
        }

        // �ǳ�
        static public void RealNameLogout(string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    GetRealNameService(0).OnLogout();
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager RealNameLogout Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                SDKManagerNew.RealNameOnLogout(SDKName, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
            }
        }

        // ʵ����״̬
        static public RealNameStatus GetRealNameType(string SDKName = null, string tag = null)
        {
            //����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    return GetRealNameService(0).GetRealNameType();
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager GetRealNameType Exception: " + e.ToString());
                    return RealNameStatus.NotNeed;
                }
            }
            else if (s_useNewSDKManager)
            {
                return SDKManagerNew.GetRealNameType(SDKName, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
                return RealNameStatus.NotNeed;
            }
        }

        // �Ƿ����
        static public bool IsAdult(string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    return GetRealNameService(0).IsAdult();
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager IsAdult Exception: " + e.ToString());
                    return true;
                }
            }

            else if (s_useNewSDKManager)
            {
                return SDKManagerNew.IsAdult(SDKName, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
                return true;
            }
        }

        // ��������ʱ��
        static public int GetTodayOnlineTime(string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    return GetRealNameService(0).GetTodayOnlineTime();
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager GetTodayOnlineTime Exception: " + e.ToString());
                    return -1;
                }
            }
            else if (s_useNewSDKManager)
            {
                return SDKManagerNew.GetTodayOnlineTime(SDKName, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
                return -1;
            }
        }

        // ��ʼʵ����
        static public void StartRealNameAttestation(string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    GetRealNameService(0).StartRealNameAttestation();
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager StartRealNameAttestation Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                SDKManagerNew.StartRealNameAttestation(SDKName, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
            }
        }

        // ���֧���Ƿ����� ��SDKManager.PayLimitCallBack �ص������
        static public void CheckPayLimit(int payAmount, string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    GetRealNameService(0).CheckPayLimit(payAmount);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager CheckPayLimit Exception: " + e.ToString());
                    return;
                }
            }
            else if (s_useNewSDKManager)
            {
                SDKManagerNew.CheckPayLimit(SDKName, payAmount, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
                return;
            }
        }

        // �ϱ�֧�����
        static public void LogPayAmount(int payAmount, string SDKName = null, string tag = null)
        {
            // ����ʹ�ñ������õ�SDK
            if (s_realNameServiceList.Count > 0)
            {
                try
                {
                    GetRealNameService(0).LogPayAmount(payAmount);
                }
                catch (Exception e)
                {
                    Debug.LogError("SDKManager CheckPayLimit Exception: " + e.ToString());
                }
            }
            else if (s_useNewSDKManager)
            {
                SDKManagerNew.LogPayAmount(SDKName, payAmount, tag);
            }
            else
            {
                Debug.Log("realName SDK û�����ã� ");
            }
        }

        // ����
        static void InitShare(List<ShareInterface> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i].Init();
                }
                catch (Exception e)
                {
                    Debug.LogError("Init LoginInterface SDK Exception:\n" + e.ToString());
                }
            }
        }

        public static void Share(string SDKName, string content)
        {
            if (s_useNewSDKManager)
            {
                //SDKManagerNew.Share
            }
        }

        // ����SDK
        public static void ToClipboard(string content)
        {
#if UNITY_EDITOR
            GUIUtility.systemCopyBuffer = content;
#elif UNITY_ANDROID
            SDKManagerNew.ToClipboard(content);
#elif UNITY_IOS
            ClipboardManager.ToClipboard(content);
#endif
        }

        public static void DownloadApk(string url = null)
        {
#if UNITY_ANDROID
            SDKManagerNew.DownloadApk(url);
#endif
        }

        public static void GetAPKSize(string url = null)
        {
#if UNITY_ANDROID
            SDKManagerNew.GetAPKSize(url);
#endif
        }

        // ����ע������
        public static void SetProperties(string key, string value)
        {
            SetPropertiesFromLocal(key, value);
        }

        // ��ȡע������
        public static string GetProperties(string key, string defaultValue = "")
        {
            defaultValue = GetPropertiesFromLocal(key, defaultValue);
            return GetProperties(SDKInterfaceDefine.FileName_ChannelProperties, key, defaultValue);
        }

        // ��ȡע������
        public static string GetProperties(string properties, string key, string defaultValue)
        {
            // pcƽ̨�¶�ȡ \Configs\{properties}.ini�����ļ�
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_WIN||UNITY_EDITOR
            string path = Environment.CurrentDirectory + "/Configs/" + properties + ".ini";
            path = path.Replace('\\', '/');
            if (File.Exists(path))
            {
                IniConfigTool tool = new IniConfigTool(path);
                string res = tool.GetString(key, defaultValue);
                return res;
            }
            else
            {
                return defaultValue;
            }
#else
            return SDKManagerNew.GetProperties(properties, key, defaultValue);
#endif
        }

        // ��Ϸ�ر�
        public static void QuitApplication()
        {
            Debug.Log("QuitApplication");
            if (Application.platform == RuntimePlatform.Android)
            {
                SDKManagerNew.QuitApplication();
            }
            else
            {
                Application.Quit();
            }
        }

        public static bool IsSDKExist(string SDKName)
        {
            bool result = false;
            if (s_useNewSDKManager)
            {
                SDKManagerNew.IsSDKExist(SDKName);
            }
            result |= GetHasSDKService(s_loginServiceList, SDKName);
            result |= GetHasSDKService(s_ADServiceList, SDKName);
            result |= GetHasSDKService(s_payServiceList, SDKName);
            result |= GetHasSDKService(s_logServiceList, SDKName);
            result |= GetHasSDKService(s_otherServiceList, SDKName);
            return result;
        }

        // �ӻ�ȡ���� ���õ���������
        private static string GetPropertiesFromLocal(string key, string defaultValue)
        {
            if (propertiesNoRepackage.ContainsKey(key))
            {
                return propertiesNoRepackage[key];
            }
            else
            {
                return defaultValue;
            }
        }

        // ���ñ��ػ������������
        private static void SetPropertiesFromLocal(string key, string value)
        {
            if (propertiesNoRepackage.ContainsKey(key))
            {
                propertiesNoRepackage[key] = value;
            }
            else
            {
                propertiesNoRepackage.Add(key, value);
            }
        }

        public static void AddOtherCallBackListener(string functionName, OtherCallBack callBack)
        {
            if (s_callBackDict.ContainsKey(functionName))
            {
                s_callBackDict[functionName] += callBack;
            }
            else
            {
                s_callBackDict.Add(functionName, callBack);
            }
            if (s_useNewSDKManager)
            {
                SDKManagerNew.AddOtherCallBackListener(functionName, callBack);
            }
        }

        public static void RemoveOtherCallBackListener(string functionName, OtherCallBack callBack)
        {
            if (s_callBackDict.ContainsKey(functionName))
            {
                s_callBackDict[functionName] -= callBack;
            }
            else
            {
                Debug.LogError("RemoveOtherCallBackListener �����ڵ� function Name ->" + functionName + "<-");
            }
            if (s_useNewSDKManager)
            {
                SDKManagerNew.RemoveOtherCallBackListener(functionName, callBack);
            }
        }

        // ��ȡ��ǰ��Ϸ�ڵ�SDK����,�Ҳ������߽���ʧ�ܻ᷵��Null
        public static SchemeData LoadGameSchemeConfig()
        {
            if (ConfigManager.GetIsExistConfig(c_ConfigName))
            {
                try
                {
                    Dictionary<string, SingleField> configData = ConfigManager.GetData(c_ConfigName);
                    return JsonUtility.FromJson<SchemeData>(configData[c_KeyName].GetString());
                }
                catch (Exception e)
                {
                    Debug.LogError("LoadGameSchemeConfig error " + e.ToString());
                    return null;
                }
            }
            else
            {
                Debug.Log("LoadGameSchemeConfig null");
                return null;
            }
        }

        public static void AnalyzeSchemeData(
            SchemeData schemeData,
            out List<LoginInterface> loginScheme,
            out List<ADInterface> ADScheme,
            out List<PayInterface> payScheme,
            out List<LogInterface> logScheme,
            out List<RealNameInterface> realNameScheme,
            out List<OtherSDKInterface> otherScheme
            )
        {
            if (schemeData != null)
            {
                loginScheme = new List<LoginInterface>();
                for (int i = 0; i < schemeData.LoginScheme.Count; i++)
                {
                    loginScheme.Add((LoginInterface)AnalysisConfig(schemeData.LoginScheme[i]));
                }
                ADScheme = new List<ADInterface>();
                for (int i = 0; i < schemeData.ADScheme.Count; i++)
                {
                    ADScheme.Add((ADInterface)AnalysisConfig(schemeData.ADScheme[i]));
                }
                payScheme = new List<PayInterface>();
                for (int i = 0; i < schemeData.PayScheme.Count; i++)
                {
                    payScheme.Add((PayInterface)AnalysisConfig(schemeData.PayScheme[i]));
                }
                logScheme = new List<LogInterface>();
                for (int i = 0; i < schemeData.LogScheme.Count; i++)
                {
                    logScheme.Add((LogInterface)AnalysisConfig(schemeData.LogScheme[i]));
                }
                realNameScheme = new List<RealNameInterface>();
                for (int i = 0; i < schemeData.RealNameScheme.Count; i++)
                {
                    realNameScheme.Add((RealNameInterface)AnalysisConfig(schemeData.RealNameScheme[i]));
                }
                otherScheme = new List<OtherSDKInterface>();
                for (int i = 0; i < schemeData.OtherScheme.Count; i++)
                {
                    otherScheme.Add((OtherSDKInterface)AnalysisConfig(schemeData.OtherScheme[i]));
                }
            }
            else
            {
                loginScheme = new List<LoginInterface>();
                ADScheme = new List<ADInterface>();
                payScheme = new List<PayInterface>();
                logScheme = new List<LogInterface>();
                realNameScheme = new List<RealNameInterface>();
                otherScheme = new List<OtherSDKInterface>();
            }
        }

        static SDKInterfaceBase AnalysisConfig(SDKConfigData data)
        {
            if (data == null || string.IsNullOrEmpty(data.SDKName))
            {
                return new NullSDKInterface();
            }
            else
            {
                return (SDKInterfaceBase)JsonUtility.FromJson(data.SDKContent, Assembly.Load("Assembly-CSharp").GetType(data.SDKName));
            }
        }

        static void LoadService(SchemeData data)
        {
            AnalyzeSchemeData(
                data,
                out s_loginServiceList,
                out s_ADServiceList,
                out s_payServiceList,
                out s_logServiceList,
                out s_realNameServiceList,
                out s_otherServiceList
                );
        }

        static void InitSDK()
        {
            InitLogin(s_loginServiceList);
            InitSDKList(s_ADServiceList);
            InitPay(s_payServiceList);
            InitSDKList(s_logServiceList);
            InitSDKList(s_otherServiceList);
            InitSDKList(s_realNameServiceList);
            InitSDKList(s_shareServiceList);
        }

        static T GetSDKService<T>(List<T> list, string name) where T : SDKInterfaceBase
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].m_SDKName == name)
                {
                    return list[i];
                }
            }
            throw new Exception("GetSDKService " + typeof(T).Name + " Exception dont find ->" + name + "<-");
        }

        static bool GetHasSDKService<T>(List<T> list, string name) where T : SDKInterfaceBase
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].m_SDKName == name)
                {
                    return true;
                }
            }
            return false;
        }

        static void InitSDKList<T>(List<T> list) where T : SDKInterfaceBase
        {
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i].m_SDKName = list[i].GetType().Name;
                    list[i].Init();
                }
                catch (Exception e)
                {
                    Debug.LogError("Init " + typeof(T).Name + " SDK Exception:\n" + e.ToString());
                }
            }
        }

        // �Զ��ϱ�������ʼ��
        static void AutoListenerInit()
        {
            PayCallBack += OnPayCallBackListener;
        }

        // �Զ��ϱ�֧��
        static void OnPayCallBackListener(OnPayInfo info)
        {
            Debug.Log("�Զ��ϱ�֧�� success >" + info.isSuccess + "<");
            if (info.isSuccess)
            {
                LogPay(info.orderID, info.goodsId, 1, info.price, info.currency, info.storeName.ToString());
            }
        }
    }
}