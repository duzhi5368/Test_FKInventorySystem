using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class PreLoadResFlowItem : FlowItemBase
    {
        public CallBack<int, int, bool> OnPreLoadProgressCallBack;  // Ԥ���ؽ�����ʾ ����ǰ����������������Ƿ�������
        private List<PreloadResourcesDataGenerate> otherResList = new List<PreloadResourcesDataGenerate>();

        // ���������ҪԤ���ص�����
        public void AddOtherPreLoadResources(List<PreloadResourcesDataGenerate> resList)
        {
            otherResList = resList;
        }

        private void PreLoadProgress(int currentNum, int count)
        {
            bool isFinish = false;
            if (count == 0 || currentNum >= count)
            {
                isFinish = true;
            }
            if (OnPreLoadProgressCallBack != null)
                OnPreLoadProgressCallBack(currentNum, count, isFinish);
            if (isFinish)
            {
                Finish(null);
            }
        }

        protected override void OnFlowStart(params object[] paras)
        {
            Debug.Log("��ʼԤ����");
            PreloadManager.progressCallBack += PreLoadProgress;
            PreloadManager.StartLoad(otherResList);
        }

        protected override void OnFlowFinished()
        {
            PreloadManager.progressCallBack -= PreLoadProgress;

        }
    }
}