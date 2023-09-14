using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // gameLoadType 为 Resource 时 ，所有资源从Resource读取
    // gameLoadType 不为 Resource时，资源读取方式从配置中读取
    public static class ResourceManager
    {
        private static AssetsLoadType loadType = AssetsLoadType.Resources;
        public static AssetsLoadType LoadType
        {
            get
            {
                return loadType;
            }
        }
        public static bool UseCache
        {
            get; private set;
        }
        private static AssetsLoadController loadAssetsController;
#if UNITY_EDITOR
        // UnityEditor模式下编译完成后自动初始化
        [UnityEditor.InitializeOnLoadMethod]
#endif
        private static void Initialize()
        {
            Initialize(AssetsLoadType.Resources, false);
        }

        // 初始化
        public static void Initialize(AssetsLoadType loadType, bool useCache)
        {
            if (loadType == AssetsLoadType.AssetBundle)
            {
                useCache = true;
            }
            if (!Application.isPlaying)
            {
                useCache = false;
            }
            UseCache = useCache;
            ResourceManager.loadType = loadType;
            ReleaseAll();
            loadAssetsController = new AssetsLoadController(loadType, useCache);
        }

        public static AssetsLoadController GetLoadAssetsController()
        {
            return loadAssetsController;
        }

        // 同步加载一个资源
        public static UnityEngine.Object Load(string name)
        {
            string path = ResourcesConfigManager.GetLoadPath(loadType, name);
            AssetsData assets = loadAssetsController.LoadAssets(path);
            if (assets != null)
            {
                return assets.Assets[0];
            }
            return null;
        }

        public static void LoadAsync(string name, CallBack<UnityEngine.Object> callBack)
        {
            string path = ResourcesConfigManager.GetLoadPath(loadType, name);
            loadAssetsController.LoadAsync(path, null, callBack);
        }
        public static void LoadAsync(string name, Type resType, CallBack<UnityEngine.Object> callBack)
        {
            string path = ResourcesConfigManager.GetLoadPath(loadType, name);
            loadAssetsController.LoadAsync(path, resType, callBack);
        }

        // 加载资源
        // 注意释放资源，方法： DestoryAssetsCounter
        public static T Load<T>(string name) where T : UnityEngine.Object
        {
            T res = null;
            string path = ResourcesConfigManager.GetLoadPath(loadType, name);
            AssetsData assets = loadAssetsController.LoadAssets<T>(path);
            if (assets != null)
            {
                res = assets.GetAssets<T>();
            }
            if (res == null)
            {
                Debug.LogError("Error => Load Name :" + name + "  Type:" + typeof(T).FullName + "\n" + " Load Object:" + res);
            }
            return res;
        }

        public static string LoadText(string name)
        {
            TextAsset tex = Load<TextAsset>(name);
            if (tex == null)
                return null;
            return tex.text;
        }

        // 释放资源 （通过 ResourceManager.Load<>() 加载出来的）
        public static void DestoryAssetsCounter(UnityEngine.Object unityObject, int times = 1)
        {
            DestoryAssetsCounter(unityObject.name, times);
        }

        public static void DestoryAssetsCounter(string name, int times = 1)
        {
            if (!ResourcesConfigManager.GetIsExitRes(name))
                return;
            string path = ResourcesConfigManager.GetLoadPath(loadType, name);
            if (times <= 0)
                times = 1;
            for (int i = 0; i < times; i++)
            {
                loadAssetsController.DestoryAssetsCounter(path);
            }
        }

        /// 卸载所有资源
        /// <param name="isForceAB">是否强制卸载bundle（true:bundle包和资源一起卸载；false：只卸载bundle包）</param>
        public static void ReleaseAll(bool isForceAB = true)
        {
            if (loadAssetsController != null)
                loadAssetsController.ReleaseAll(isForceAB);
            // ResourcesConfigManager.ClearConfig();
        }

        public static void Release(string name)
        {
            string path = ResourcesConfigManager.GetLoadPath(loadType, name);
            loadAssetsController.Release(path);
        }

        public static void ReleaseByPath(string path)
        {
            loadAssetsController.Release(path);
        }

        public static bool GetResourceIsExist(string name)
        {
            return ResourcesConfigManager.GetIsExitRes(name);
        }
    }
}