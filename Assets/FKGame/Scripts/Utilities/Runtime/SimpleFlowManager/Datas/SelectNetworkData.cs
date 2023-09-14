using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SelectNetworkData : IDataGenerateBase
    {
        public string m_key;
        public string m_serverIP;               // ������IP
        public string m_description;            // ����
        public int m_port;                      // �˿�
        public string[] m_androidVersion;       // ֧��ǰ��Android�İ汾
        public string[] m_iosVersion;           // ֧�ֵ�ǰ��IOS�İ汾
        public string[] m_channel;              // ��������
        public string[] m_standaloneVersion;    // ����Windows��mac��Linux

        public override void LoadData(string key)
        {
            DataTable table = DataManager.GetData("SelectNetworkData");
            if (!table.ContainsKey(key))
            {
                throw new Exception("SelectNetworkDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
            }
            SingleData data = table[key];
            m_key = key;
            m_serverIP = data.GetString("serverIP");
            m_description = data.GetString("description");
            m_port = data.GetInt("port");
            m_androidVersion = data.GetStringArray("androidVersion");
            m_iosVersion = data.GetStringArray("iosVersion");
            m_channel = data.GetStringArray("channel");
            try
            {
                m_standaloneVersion = data.GetStringArray("standaloneVersion");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override void LoadData(DataTable table, string key)
        {
            SingleData data = table[key];
            m_key = key;
            m_serverIP = data.GetString("serverIP");
            m_description = data.GetString("description");
            m_port = data.GetInt("port");
            m_androidVersion = data.GetStringArray("androidVersion");
            m_iosVersion = data.GetStringArray("iosVersion");
            m_channel = data.GetStringArray("channel");

            try
            {
                m_standaloneVersion = data.GetStringArray("standaloneVersion");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}