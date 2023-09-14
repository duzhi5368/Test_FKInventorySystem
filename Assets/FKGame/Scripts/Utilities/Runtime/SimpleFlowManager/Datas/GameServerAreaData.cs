using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GameServerAreaData : IDataGenerateBase
    {
        public string m_key;
        public string m_Description;            // ����������
        public string m_ClientHotUpdateURL;     // �ͻ����ȸ��µ�ַ
        public string m_SelectServerURL;        // �ô�����ѡ���б�
        public string[] m_CountryCode;          // ���Ӹô����Ĺ���code�� ISO 3166-1 alpha-2 ��
        public string m_SpecialServerHost;      // ѡȡ�����е�һ�������������ڲ��ܻ�ȡIP������ʱʹ��Pingѡ����
        public string[] m_ContinentName;        // ����Ӣ����д������ֱ��ʹ�ô��������ִ���������ѡ��Ϊ�գ���ʹ��Countrycode��ѡ�����([AF]����,
                                                // [EU]ŷ��, [AS]����, [OA]������, [NA]������, [SA]������, [AN]�ϼ���)

        public override void LoadData(string key)
        {
            DataTable table = DataManager.GetData("GameServerAreaData");
            if (!table.ContainsKey(key))
            {
                throw new Exception("GameServerAreaDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
            }

            SingleData data = table[key];
            m_key = key;
            m_Description = data.GetString("Description");
            m_ClientHotUpdateURL = data.GetString("ClientHotUpdateURL");
            m_SelectServerURL = data.GetString("SelectServerURL");
            m_CountryCode = data.GetStringArray("CountryCode");
            m_SpecialServerHost = data.GetString("SpecialServerHost");
            try
            {
                m_ContinentName = data.GetStringArray("ContinentName");
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
            m_Description = data.GetString("Description");
            m_ClientHotUpdateURL = data.GetString("ClientHotUpdateURL");
            m_SelectServerURL = data.GetString("SelectServerURL");
            m_CountryCode = data.GetStringArray("CountryCode");
            m_SpecialServerHost = data.GetString("SpecialServerHost");
            try
            {
                m_ContinentName = data.GetStringArray("ContinentName");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}