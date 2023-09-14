using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AssetsLoadController
    {
        private Dictionary<string, AssetsData> assetsCaches = new Dictionary<string, AssetsData>();
        private ILoaderBase loader;
        private AssetsLoadType loadType;
        private bool useCache;
        public AssetsLoadController(AssetsLoadType loadType, bool useCache)
        {
            this.loadType = loadType;
            this.useCache = useCache;
            if (loadType == AssetsLoadType.Resources)
            {

                loader = new ResourcesLoader(this);
            }
            else
            {
                loader = new AssetBundleLoader(this);
            }
        }

        public Dictionary<string, AssetsData> GetLoadAssets()
        {
            return assetsCaches;
        }

        public AssetsData LoadAssets(string path)
        {
            return LoadAssetsLogic(path, () =>
                {
                    if (assetsCaches.ContainsKey(path))
                    {
                        return true;
                    }
                    return false;
                }
                , 
                (p) =>
                {
                    return loader.LoadAssets(p);

                });
        }
        public AssetsData LoadAssets<T>(string path) where T : UnityEngine.Object
        {
            return LoadAssetsLogic(path,
                () =>
                {
                    if (assetsCaches.ContainsKey(path))
                    {
                        T res = assetsCaches[path].GetAssets<T>();
                        if (res != null)
                            return true;
                    }
                    return false;
                }
                ,
                (p) =>
                {
                    return loader.LoadAssets<T>(p);

                });
        }
        public AssetsData LoadAssetsLogic(string path, CallBackR<bool> checkContainsAssets, CallBackR<AssetsData, string> loadMethod)
        {
            LoadAssetsDependencie(path);
            AssetsData assets = null;
            if (checkContainsAssets())
            {
                assets = assetsCaches[path];
            }
            else
            {
                assets = loadMethod(path);
                if (assets == null)
                {
                    Debug.LogError("��Դ����ʧ�ܣ�" + path);
                    return assets;
                }
                else
                {
                    if (assetsCaches.ContainsKey(path))
                    {
                        List<UnityEngine.Object> asList = new List<UnityEngine.Object>(assetsCaches[path].Assets);
                        foreach (var item in assets.Assets)
                        {
                            if (!asList.Contains(item))
                            {
                                asList.Add(item);
                            }
                        }
                        assetsCaches[path].Assets = asList.ToArray();
                        assets = assetsCaches[path];
                    }
                    else
                    {
                        if (useCache)
                        {
                            assetsCaches.Add(path, assets);
                        }
                    }
                }
            }
            if (useCache)
            {
                assets.refCount++;
                AssetsUnloadHandler.MarkUseAssets(assets, loader.IsHaveDependencies(path));
            }
            return assets;
        }

        private void LoadAssetsDependencie(string path)
        {
            string[] dependenciesNames = loader.GetAllDependenciesName(path);
            foreach (var item in dependenciesNames)
            {
                LoadAssets(item);
            }
        }

        public void LoadAsync(string path, Type assetType, CallBack<UnityEngine.Object> callBack)
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(LoadAssetsIEnumerator(path, assetType, callBack));
        }

        // �첽������Դ�߼�
        private IEnumerator LoadAssetsIEnumerator(string path, Type assetType, CallBack<UnityEngine.Object> callBack)
        {
            yield return LoadAssetsIDependencieEnumerator(path);

            if (assetsCaches.ContainsKey(path))
            {
                AssetsData assets = assetsCaches[path];
                if (useCache)
                {
                    assets.refCount++;
                    AssetsUnloadHandler.MarkUseAssets(assets, loader.IsHaveDependencies(path));
                }
                if (callBack != null)
                {
                    callBack(assets.Assets[0]);
                }
            }
            else
            {
                yield return loader.LoadAssetsIEnumerator(path, assetType, (assets) =>
                {
                    if (useCache)
                    {
                        assetsCaches.Add(path, assets);
                    }
                    if (useCache)
                    {
                        assets.refCount++;
                        AssetsUnloadHandler.MarkUseAssets(assets, loader.IsHaveDependencies(path));
                    }
                    if (callBack != null)
                    {
                        callBack(assets.Assets[0]);
                    }
                });
            }
            yield return 0;
        }

        // �첽����������
        private IEnumerator LoadAssetsIDependencieEnumerator(string path)
        {
            string[] dependenciesNames = loader.GetAllDependenciesName(path);
            foreach (var item in dependenciesNames)
            {
                yield return LoadAssetsIEnumerator(item, null, null);
            }
        }

        // ��Դ���������٣�����Դ������Ҳ����٣�
        public void DestoryAssetsCounter(string path)
        {
            if (assetsCaches.ContainsKey(path))
            {
                AssetsData assets = assetsCaches[path];
                assets.refCount--;
                if (assets.refCount < 0)
                {
                    Debug.LogError("��Դ���ü�������(" + assets.refCount + ") " + assets.assetPath);
                    assets.refCount = 0;
                }
                if (assets.refCount <= 0)
                {
                    AssetsUnloadHandler.DiscardAssets(assets);
                }

                string[] dependenciesNames = loader.GetAllDependenciesName(path);
                foreach (var item in dependenciesNames)
                {
                    DestoryAssetsCounter(item);
                }
            }
            else
            {
                if (useCache)
                {
                    Debug.LogError("δ������Դ������Destroy ��" + path);
                }
            }
        }

        public void ReleaseAll(bool isForceAB)
        {
            foreach (var item in assetsCaches)
            {
                UnloadAssets(item.Value, isForceAB);
            }
            assetsCaches.Clear();
        }

        // ֱ���ͷ���Դ��������Ϊ0ʱ�����ã�
        public void Release(string path)
        {
            AssetsData assets = null;
            if (assetsCaches.TryGetValue(path, out assets))
            {
                if (assets.refCount <= 0)
                {
                    UnloadAssets(assets, true);
                    assetsCaches.Remove(path);
                    Debug.LogWarning("�����ͷ�" + path);
                }
            }
        }

        public void UnloadAssets(AssetsData assets, bool isForceAB)
        {
            if (!useCache)
                return;
            if (assets.Assets != null && isForceAB)
            {
                foreach (var item in assets.Assets)
                {
                    Debug.LogWarning("�ͷ���Դ" + item);
                    UnloadObject(item);
                }
                assets.Assets = null;
            }
            if (assets.AssetBundle != null)
                assets.AssetBundle.Unload(isForceAB);
        }
        private void UnloadObject(UnityEngine.Object obj)
        {

            if (obj == null)
            {
                return;
            }
            if (obj is Shader)
            {
                return;
            }

            if (!(obj is GameObject)
                && !(obj is Component)
                && !(obj is AssetBundle)
                )
            {
                Resources.UnloadAsset(obj);
            }
            else if ((obj is GameObject)
                || (obj is Component))
            {
                if (loadType == AssetsLoadType.AssetBundle)
                    UnityEngine.Object.DestroyImmediate(obj, true);
            }
            else
            {
                AssetBundle ab = (AssetBundle)obj;
                ab.Unload(true);
            }
        }
    }
}