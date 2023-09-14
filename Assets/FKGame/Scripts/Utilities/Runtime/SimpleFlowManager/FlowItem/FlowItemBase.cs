using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class FlowItemBase
    {
        public SimpleFlowManager flowManager;

        public bool Enable = true;
        public Action<FlowItemBase> OnStart;
        public Action<FlowItemBase, string> OnFinished; // 节点完成回调（节点名字（GetType().Name），error错误信息）
        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        public void Start(params object[] paras)
        {
            Debug.Log("FlowItemBase.start:" + Name);
            if (!Enable)
            {
                FinishCallBack(null);
                return;
            }
            OnFlowStart(paras);
            if (OnStart != null)
            {
                OnStart(this);
            }
            if (flowManager.OnStart != null)
            {
                flowManager.OnStart(this);
            }
        }

        protected virtual void OnFlowStart(params object[] paras){}


        public void Finish(string error)
        {
            Debug.Log("FlowItemBase.Finish:" + Name + " error:" + error);
            OnFlowFinished();
            FinishCallBack(error);
        }

        private void FinishCallBack(string error)
        {
            if (OnFinished != null)
            {
                OnFinished(this, error);
            }
            if (flowManager.OnFinished != null)
            {
                flowManager.OnFinished(this, error);
            }
        }
        protected virtual void OnFlowFinished(){}
    }
}