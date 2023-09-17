using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class IDataGenerateBase
    {
        public virtual void LoadData(string key) { }
        public virtual void LoadData(DataTable table, string key)
        {
            Debug.LogError("¡¾FK¡¿Default function can't load data.");
        }
    }
}