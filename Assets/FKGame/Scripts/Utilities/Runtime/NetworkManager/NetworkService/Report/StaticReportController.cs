using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��һЩ�̶������ϱ�������
    public class StaticReportController
    {
        private const string ReportUserData = "ReportUserData";

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            LoginGameController.OnUserLogin += OnUserLogin;
        }

        private static void OnUserLogin(UserLogin2Client t)
        {
            if (t.reloginState)
                return;
            if (t.code != 0)
                return;
            SendDeviceInfo(t.user.userID);
        }

        private static void SendDeviceInfo(string userID)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("uuid", userID);
            string channel = "Windows";

#if UNITY_ANDROID && !UNITY_EDITOR
            channel = "Android";
#endif
#if UNITY_IOS && !UNITY_EDITOR
            channel = "IOS";
#endif
            string[] os = SystemInfo.operatingSystem.Split(' ');
            string[] deviceInfo = SystemInfo.deviceModel.Split(' ');
            string cc = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_ChannelName, channel);
            datas.Add("channel", cc);
            datas.Add("brand", deviceInfo[0]);
            datas.Add("deviceName", SystemInfo.deviceModel);
            datas.Add("version", ApplicationManager.Version);
            datas.Add("processorType", SystemInfo.processorType.ToString());
            datas.Add("processorCount", SystemInfo.processorCount.ToString());

            string net = "�ƶ�����";
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                net = "������";
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                net = "wifi";
            }

            datas.Add("net", net);
            datas.Add("systemLanguage", Application.systemLanguage.ToString());
            datas.Add("memorySize", SystemInfo.systemMemorySize.ToString());
            datas.Add("graphicMemorySize", SystemInfo.graphicsMemorySize.ToString());
            datas.Add("shaderLevel", SystemInfo.graphicsShaderLevel.ToString());
            datas.Add("graphicDeviceType", SystemInfo.graphicsDeviceType.ToString());
            datas.Add("os", os[0]);
            datas.Add("ov", SystemInfo.operatingSystem);

            int w = Screen.width;
            int h = Screen.height;
            if (w < h)
            {
                w = Screen.height;
                h = Screen.width;
            }
            datas.Add("resolution", w + "x" + h);

            SDKManager.Log(ReportUserData, datas);
        }
    }
}