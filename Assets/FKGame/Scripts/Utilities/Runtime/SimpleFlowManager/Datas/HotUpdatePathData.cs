using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class HotUpdatePathData : IDataGenerateBase
    {
        public string m_key;
        public string m_HotupdatePath;  // 热更新地址
        public string m_Description;    // 描述

        public override void LoadData(string key)
        {
            DataTable table = DataManager.GetData("HotUpdatePathData");
            if (!table.ContainsKey(key))
            {
                throw new Exception("HotUpdatePathDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
            }

            SingleData data = table[key];
            m_key = key;
            m_HotupdatePath = data.GetString("HotupdatePath");
            m_Description = data.GetString("Description");
        }

        public override void LoadData(DataTable table, string key)
        {
            SingleData data = table[key];
            m_key = key;
            m_HotupdatePath = data.GetString("HotupdatePath");
            m_Description = data.GetString("Description");
        }
    }
}