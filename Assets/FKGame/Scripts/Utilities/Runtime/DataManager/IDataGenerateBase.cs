using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class IDataGenerateBase
    {
        public virtual void LoadData(string key) { }
        public virtual void LoadData(DataTable table, string key)
        {
            Debug.LogError("默认方法不能加载数据！");
        }
    }
}