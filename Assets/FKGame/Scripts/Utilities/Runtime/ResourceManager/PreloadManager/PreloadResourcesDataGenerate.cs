using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class PreloadResourcesDataGenerate : IDataGenerateBase
    {
        public string m_key;
        public int m_instantiateNum;            // ʵ������������Ȼֻ֧��Ԥ��
        public string m_description;            // ��Դ�򵥽���
        public bool m_createInstanceActive;     // Ԥ���ط��ڶ���ص�ʵ���Ƿ��Ǽ���״̬
        public PreloadResType m_ResType;        // ��Դ����
        public bool m_UseLoad = true;           // �Ƿ����ø������

        public override void LoadData(string key)
        {
            DataTable table = DataManager.GetData("PreloadResourcesData");
            if (!table.ContainsKey(key))
            {
                throw new Exception("PreloadResourcesDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
            }

            SingleData data = table[key];
            m_key = key;
            m_instantiateNum = data.GetInt("instantiateNum");
            m_description = data.GetString("description");
            m_createInstanceActive = data.GetBool("createInstanceActive");
            m_ResType = data.GetEnum<PreloadResType>("ResType");
            m_UseLoad = data.GetBool("UseLoad");
        }
    }
}
