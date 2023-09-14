using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��Դж�ؿ�����
    public class AssetsUnloadHandler
    {
        public static Dictionary<string, UnloadAssetInfo> usedAssetsDic = new Dictionary<string, UnloadAssetInfo>();
        public static Dictionary<string, UnloadAssetInfo> noUsedAssetsDic = new Dictionary<string, UnloadAssetInfo>();
        public static List<UnloadAssetInfo> noUsedAssetsList = new List<UnloadAssetInfo>();
        private static Dictionary<string, UnloadAssetInfo> unloadBundleQue = new Dictionary<string, UnloadAssetInfo>();

        // ��¼��Դ�ļ���
        public static void MarkUseAssets(AssetsData assets, bool isHaveDependencies)
        {
            if (assets == null)
                return;
            UnloadAssetInfo info = MakeUseLogic(assets.assetName, assets, isHaveDependencies);

            // ����������Դ����ж��Bundleѹ�����Ķ���
            if (assets.AssetBundle != null && !isHaveDependencies)
            {
                info.unloadBundleTime = 5;
                if (!unloadBundleQue.ContainsKey(info.assetsName))
                    unloadBundleQue.Add(info.assetsName, info);
            }
        }

        // ��¼��Դ��ʹ�ã����������ظ�ʹ��һ�μ�¼һ�Σ�
        public static void MarkUseAssets(string assetsName)
        {
            if (!ResourceManager.UseCache)
                return;
            if (usedAssetsDic.ContainsKey(assetsName))
                MakeUseLogic(assetsName);
        }

        private static UnloadAssetInfo MakeUseLogic(string assetName, AssetsData assets = null, bool isHaveDependencies = true)
        {
            UnloadAssetInfo info = null;
            if (noUsedAssetsDic.ContainsKey(assetName))
            {
                info = noUsedAssetsDic[assetName];
                noUsedAssetsDic.Remove(assetName);
                if (noUsedAssetsList.Contains(info))
                    noUsedAssetsList.Remove(info);
                usedAssetsDic.Add(assetName, info);
            }
            else if (usedAssetsDic.ContainsKey(assetName))
            {
                info = usedAssetsDic[assetName];
            }
            else
            {
                info = new UnloadAssetInfo(isHaveDependencies, assets);
                usedAssetsDic.Add(assetName, info);
            }
            if (assets != null)
            {
                info.assets = assets;
            }
            info.useTimes++;
            return info;
        }

        // ����Դ����Ϸ��������ʱ����������¼
        public static void DiscardAssets(AssetsData assets)
        {
            if (!usedAssetsDic.ContainsKey(assets.assetName))
            {
                return;
            }
            UnloadAssetInfo info = null;
            info = usedAssetsDic[assets.assetName];
            usedAssetsDic.Remove(assets.assetName);
            info.discardTime = Time.realtimeSinceStartup;
            noUsedAssetsDic.Add(info.assetsName, info);
            noUsedAssetsList.Add(info);
            noUsedAssetsList.Sort(SortNoUsedAssetsInfo);
        }

        private static int SortNoUsedAssetsInfo(UnloadAssetInfo x, UnloadAssetInfo y)
        {
            // ����Դ���������ڵ�ʱ�� / ��Դʹ�ô���
            float freqX = x.GetFrequency();
            float freqY = y.GetFrequency();
            if (freqX < freqY)
            {
                return 1;
            }
            else if (freqX > freqY)
                return -1;
            return 0;
        }

        public static void UnloadAll()
        {
            foreach (var info in noUsedAssetsList)
            {
                if (unloadBundleQue.ContainsKey(info.assetsName))
                    unloadBundleQue.Remove(info.assetsName);
                ResourceManager.ReleaseByPath(info.assets.assetPath);
            }
            noUsedAssetsList.Clear();
        }

        public static void UnloadOne()
        {
            if (noUsedAssetsList.Count > 0)
            {
                UnloadAssetInfo info = noUsedAssetsList[0];
                noUsedAssetsList.RemoveAt(0);
                if (unloadBundleQue.ContainsKey(info.assetsName))
                    unloadBundleQue.Remove(info.assetsName);
                ResourceManager.ReleaseByPath(info.assets.assetPath);
            }
        }
        public static void LateUpdate()
        {
            if (unloadBundleQue.Count > 0)
            {
                foreach (var keyValue in unloadBundleQue)
                {
                    var item = keyValue.Value;
                    if (item != null)
                    {
                        item.unloadBundleTime -= Time.deltaTime;
                        if (item.unloadBundleTime <= 0)
                        {
                            if (item.assets != null && item.assets.AssetBundle != null)
                            {
                                item.assets.AssetBundle.Unload(false);
                                item.assets.AssetBundle = null;
                            }
                            unloadBundleQue.Remove(keyValue.Key);
                            break;
                        }
                    }
                    else
                    {
                        unloadBundleQue.Remove(keyValue.Key);
                        Debug.LogError("Unload res is null .name:" + keyValue.Key);
                        break;
                    }
                }
            }
        }
    }
}