using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AssetsManifestManager
    {
        public const string c_ManifestFileName = "StreamingAssets";
        private static bool s_isInit = false;
        private static AssetBundleManifest s_manifest;
        private static Dictionary<string, string[]> dependenciePathsDic = new Dictionary<string, string[]>();
        private static List<string> hasDependenciesPathList = new List<string>();       // ���溬����������Դ��·���б�

        static void Initialize()
        {
            if (!s_isInit)
            {
                s_isInit = true;
                LoadAssetsManifest();
            }
        }

        public static void LoadAssetsManifest()
        {
            ResLoadLocation type = ResLoadLocation.Streaming;
            string path = null;

            if (RecordManager.GetData(HotUpdateManager.c_HotUpdateRecordName).GetRecord(HotUpdateManager.c_useHotUpdateRecordKey, false))
            {
                Debug.Log("LoadAssetsManifest ��ȡɳ��·��");
                type = ResLoadLocation.Persistent;
                //������Դ�����Application.persistentDataPath+"/Resources/"Ŀ¼��
                path = PathTool.GetAssetsBundlePersistentPath() + c_ManifestFileName;
            }
            else
            {
                Debug.Log("LoadAssetsManifest ��ȡstream·��");
                path = PathTool.GetAbsolutePath(type, c_ManifestFileName);
            }

            AssetBundle ab = AssetBundle.LoadFromFile(path);
            s_manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            ab.Unload(false);
            LoadDependenciePaths();
        }

        public static Hash128 GetHash(string bundleName)
        {
            Initialize();
            return s_manifest.GetAssetBundleHash(bundleName);
        }

        public static AssetBundleManifest GetManifest()
        {
            Initialize();
            return s_manifest;
        }

        public static Dictionary<string, string[]> GetDependencieNamesDic()
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            foreach (var item in dependenciePathsDic)
            {
                List<string> names = new List<string>();
                foreach (var pathArr in item.Value)
                {
                    string name = PathUtils.GetFileName(pathArr);
                    names.Add(name);
                }
                dic.Add(PathUtils.GetFileName(item.Key), names.ToArray());
            }
            return dic;
        }

        private static void LoadDependenciePaths()
        {
            dependenciePathsDic.Clear();
            string[] sArr = s_manifest.GetAllAssetBundles();
            for (int i = 0; i < sArr.Length; i++)
            {
                string assetPath = sArr[i];
                string[] dependenPaths = s_manifest.GetDirectDependencies(assetPath);
                string[] dependens = new string[dependenPaths.Length];
                for (int j = 0; j < dependenPaths.Length; j++)
                {
                    dependens[j] = ResourcesConfigManager.GetLoadPathBase(ResourceManager.LoadType, dependenPaths[j]);
                }
                dependenciePathsDic.Add(ResourcesConfigManager.GetLoadPathBase(ResourceManager.LoadType, assetPath), dependens);
            }

            hasDependenciesPathList.Clear();
            foreach (var assetPath in dependenciePathsDic.Keys)
            {
                bool hasDep = false;
                foreach (var depList in dependenciePathsDic.Values)
                {
                    foreach (var item in depList)
                    {
                        if (item == assetPath)
                        {
                            hasDep = true;
                            hasDependenciesPathList.Add(assetPath);
                            break;
                        }
                    }
                    if (hasDep)
                    {
                        break;
                    }
                }
            }
        }

        public static string[] GetAllDependenciesPaths(string path)
        {
            if (!s_isInit)
            {
                Initialize();
            }
            if (dependenciePathsDic.Count == 0)
                return new string[0];

            if (dependenciePathsDic.ContainsKey(path))
            {
                return dependenciePathsDic[path];
            }
            else
            {
                Debug.LogError("û�ҵ����� GetAllDependenciesName.Name :" + path + " dependencieNamesDic=>" + dependenciePathsDic.Count);
                return new string[0];
            }
        }

        // �Ƿ���������������
        public static bool IsHaveDependencies(string path)
        {
            if (!s_isInit)
            {
                Initialize();
            }
            if (hasDependenciesPathList.Contains(path))
            {
                return true;
            }
            return false;
        }
    }
}