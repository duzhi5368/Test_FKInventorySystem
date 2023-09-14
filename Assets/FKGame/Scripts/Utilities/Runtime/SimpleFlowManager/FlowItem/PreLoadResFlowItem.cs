using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class PreLoadResFlowItem : FlowItemBase
    {
        public CallBack<int, int, bool> OnPreLoadProgressCallBack;  // 预加载进度显示 ：当前数量：最大数量，是否加载完毕
        private List<PreloadResourcesDataGenerate> otherResList = new List<PreloadResourcesDataGenerate>();

        // 添加其他需要预加载的配置
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
            Debug.Log("开始预加载");
            PreloadManager.progressCallBack += PreLoadProgress;
            PreloadManager.StartLoad(otherResList);
        }

        protected override void OnFlowFinished()
        {
            PreloadManager.progressCallBack -= PreLoadProgress;

        }
    }
}