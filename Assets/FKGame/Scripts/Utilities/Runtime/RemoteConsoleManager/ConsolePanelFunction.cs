using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class ConsolePanelFunction
    {
        private const string MethodType = "Frame";


        [RemoteInvoking(name = "���ó�������ģʽ", methodType = MethodType)]
        [ParamsDescription(paramName = "m_AppMode", selectItemValues = new string[] { " Developing", "QA", "Release" })]
        private static void SetAppMode(string m_AppMode)
        {
            PlayerPrefs.SetString("AppMode", m_AppMode);
            ApplicationManager.Instance.m_AppMode = (AppMode)Enum.Parse(typeof(AppMode), m_AppMode);
        }


        [RemoteInvoking(name = "��������ģʽ����", methodType = MethodType)]
        [ParamsDescription(paramName = "testNetAreaURL", paramsDescriptionName = "���Դ�����ַ")]
        private static void SetServerTestMode(bool isTestMode, string testNetAreaURL)
        {
            GamePrepareFlowController.SetServerTestMode(isTestMode, testNetAreaURL);
        }

        [RemoteInvoking(name = "�����ȸ��²��Ե�ַ", methodType = MethodType)]
        [ParamsDescription(paramName = "testPath", paramsDescriptionName = "�ȸ��µ�ַ", getDefaultValueMethodName = "GetNowHotUpdateTestPath")]
        private static void SetHotUpdateTestPath(string testPath)
        {
            PlayerPrefs.SetString(HotupdateFlowItem.P_SelectHotUpdateTestPath, testPath);
        }

        private static string GetNowHotUpdateTestPath()
        {
            return PlayerPrefs.GetString(HotupdateFlowItem.P_SelectHotUpdateTestPath, "");
        }

        [RemoteInvoking(name = "���õ�¼����Country Code(IOS-3166)", methodType = MethodType)]
        [ParamsDescription(paramName = "countryCode", selectItemValues = new string[] { "CN", "TW", "US", "DE" }, paramsDescriptionName = "������", getDefaultValueMethodName = "GetCurrentCountryCode")]
        private static void SetDeviceLoginCountryCode(string countryCode)
        {
            PlayerPrefs.SetString(DownloadRegionServerListFlowItem.P_Country_Code, countryCode);
        }

        private static string GetCurrentCountryCode()
        {
            return PlayerPrefs.GetString(DownloadRegionServerListFlowItem.P_Country_Code, "");
        }
    }
}