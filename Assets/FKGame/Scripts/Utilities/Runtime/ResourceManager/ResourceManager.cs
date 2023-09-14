using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // gameLoadType Ϊ Resource ʱ ��������Դ��Resource��ȡ
    // gameLoadType ��Ϊ Resourceʱ����Դ��ȡ��ʽ�������ж�ȡ
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
        // UnityEditorģʽ�±�����ɺ��Զ���ʼ��
        [UnityEditor.InitializeOnLoadMethod]
#endif
        private static void Initialize()
        {
            Initialize(AssetsLoadType.Resources, false);
        }

        // ��ʼ��
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

        // ͬ������һ����Դ
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

        // ������Դ
        // ע���ͷ���Դ�������� DestoryAssetsCounter
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

        // �ͷ���Դ ��ͨ�� ResourceManager.Load<>() ���س����ģ�
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

        /// ж��������Դ
        /// <param name="isForceAB">�Ƿ�ǿ��ж��bundle��true:bundle������Դһ��ж�أ�false��ֻж��bundle����</param>
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