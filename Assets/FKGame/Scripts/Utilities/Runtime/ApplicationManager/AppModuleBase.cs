using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class AppModuleBase
    {
        private bool enable = true;

        public bool Enable
        {
            get => enable;
            set
            {
                if (enable && !value)
                {
                    Debug.Log(GetType().Name + " ���ܹر�.");
                    OnDisable();
                }
                else if (!enable && value)
                {
                    Debug.Log(GetType().Name + " ���ܿ���.");
                    OnEnable();
                }
                enable = value;
            }
        }
        public virtual string GetModuleName() { return GetType().Name; }

        public virtual string GetModuleVersion() { return ""; }

        public virtual void OnCreate() { }

        public virtual void OnStart() { }

        public virtual void OnEnable() { }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnLateUpdate() { }

        public virtual void OnGUIUpdate() { }

        public virtual void OnDisable() { }

        public virtual void OnApplicationQuit() { }

        public virtual void OnDrawGizmosUpdate() { }

        // ��Ҫ��ģ�������棨���������ڴ�ʱ��
        public virtual void OnReleaseCache() { }

        public virtual void OnApplicationPause(bool pauseStatus){}

        public virtual void OnApplicationFocus(bool focusStatus){}

        public virtual void OnDestroy(){}
    }
}