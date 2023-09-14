using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class MonoBehaviourExtend : MonoBehaviour
    {
        public int callLevel = 1;           // �������ȼ���Խ��������ȼ�Խ��

        public void UpdateEx()
        {
            OnUpdate();
        }
        protected virtual void OnUpdate() { }
        public void FixedUpdateEx()
        {
            OnFixedUpdate();
        }
        protected virtual void OnFixedUpdate() { }
        public void LateUpdateEx()
        {
            OnLateUpdate();
        }
        protected virtual void OnLateUpdate() { }

        public void OnGUIEx()
        {
            OnGUIUpdate();
        }
        protected virtual void OnGUIUpdate() { }
        void OnEnable()
        {
            MonoBehaviourManager.Add(this);
        }

        void OnDestroy()
        {
            MonoBehaviourManager.Remove(this);
        }
    }
}
