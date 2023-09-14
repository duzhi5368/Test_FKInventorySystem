using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LoginConfigData : IDataGenerateBase
    {
        public string m_key;
        public LoginPlatform m_loginName;           // ��¼��������
        public string m_UIIcon;                     // UI����ʾ��ͼ��
        public bool m_UseItem;                      // �Ƿ�����ƽ̨�����õ�ǰ��¼
        public string m_Description;                // ����
        public string m_LoginClassName;             // �Խ�SDK��class Name
        public string[] m_SupportPlatform;          // ֧��ƽ̨(ʹ��UnityEngine.RuntimePlatform)
        public string m_CustomInfo;                 // ������Զ����ı�

        public override void LoadData(string key)
        {
            DataTable table = DataManager.GetData("LoginConfigData");

            if (!table.ContainsKey(key))
            {
                throw new Exception("LoginConfigDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
            }

            SingleData data = table[key];

            m_key = key;
            m_loginName = data.GetEnum<LoginPlatform>("loginName");
            m_UIIcon = data.GetString("UIIcon");
            m_UseItem = data.GetBool("UseItem");
            m_Description = data.GetString("Description");
            m_LoginClassName = data.GetString("LoginClassName");
            m_SupportPlatform = data.GetStringArray("SupportPlatform");
            m_CustomInfo = data.GetString("CustomInfo");
        }
    }
}