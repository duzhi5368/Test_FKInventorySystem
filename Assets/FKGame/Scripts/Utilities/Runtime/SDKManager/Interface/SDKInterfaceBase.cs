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
        // �����ʼ������SDK��Ҫ����ĳ�ʼ��ʱ��ʱʹ��
        public virtual void ExtraInit(string tag){}
    }
}