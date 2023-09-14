using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public enum ADState
    {
        Play,
        Load,
        Click,
    }

    public static class ReportController
    {
        // �����ϱ�
        public static void ReportEvent(string eventName, Dictionary<string, string> datas)
        {
            SDKManager.Log(eventName, datas);
        }

        /// <summary>
        /// �����ϱ��������
        /// </summary>
        /// <param name="eventName">ʾ��AD_xxx</param>
        /// <param name="name">ֻ��Ϊ"Play"��"Load"����֮һ�����ִ�Сд��"Play"=��沥�š�"Load"=�����أ�</param>
        /// <param name="cause">ʲôԭ�򲥷ţ���Դ</param>
        /// <param name="result">ADState�Ƿ�ɹ�</param>
        /// <param name="source">�������Դ</param>
        public static Dictionary<string, string> BuildADEventData(string eventName, ADState name = ADState.Play, bool result = true, String source = "")
        {
            if (string.IsNullOrEmpty(eventName) || !eventName.ToLower().Contains("ad_"))
            {
                Debug.LogError("�ϱ���������Ϲ���" + eventName);
                return new Dictionary<string, string>();
            }
            string typeName = eventName.Substring(3);
            Debug.Log("AD TypeName:" + typeName);
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("ad_id", typeName);
            datas.Add("name", name.ToString());
            datas.Add("cause", typeName);
            datas.Add("result", result.ToString());
            datas.Add("source", source);
            return datas;
        }
    }
}