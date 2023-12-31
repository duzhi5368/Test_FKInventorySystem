using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class PreloadResourcesDataGenerate : IDataGenerateBase
    {
        public string m_key;
        public int m_instantiateNum;            // 实例化数量，当然只支持预制
        public string m_description;            // 资源简单介绍
        public bool m_createInstanceActive;     // 预加载放在对象池的实例是否是激活状态
        public PreloadResType m_ResType;        // 资源类型
        public bool m_UseLoad = true;           // 是否启用该项加载

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
