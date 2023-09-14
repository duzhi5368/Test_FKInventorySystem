using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UnloadAssetInfo
    {
        public string assetsName;
        public int useTimes = 0;
        public float discardTime;           // ��Դ����ʹ�ã�����Ϊ0��ʱ��ʱ��
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