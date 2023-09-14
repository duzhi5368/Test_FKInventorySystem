using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UnloadAssetInfo
    {
        public string assetsName;
        public int useTimes = 0;
        public float discardTime;           // 资源不再使用（引用为0）时的时间
        public bool isHaveDependencies;
        public AssetsData assets;
        public float unloadBundleTime;

        public UnloadAssetInfo(bool isHaveDependencies, AssetsData assets)
        {
            this.assetsName = assets.assetName;
            this.isHaveDependencies = isHaveDependencies;
            this.assets = assets;
        }

        public float GetFrequency()
        {
            return (Time.realtimeSinceStartup - discardTime) / useTimes;
        }
    }
}