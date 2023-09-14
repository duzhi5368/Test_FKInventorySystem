using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [System.Serializable]
    public abstract class SDKInterfaceBase
    {
        [HideInInspector]
        public string m_SDKName;

        public virtual void Init() {}
        public virtual List<RuntimePlatform> GetPlatform()
        {
            return new List<RuntimePlatform>() { Application.platform };
        }
        // 额外初始化，当SDK需要特殊的初始化时机时使用
        public virtual void ExtraInit(string tag){}
    }
}