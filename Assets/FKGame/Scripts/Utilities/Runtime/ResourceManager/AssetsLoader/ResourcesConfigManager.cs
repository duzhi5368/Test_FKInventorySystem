using System.Data;
using System;
using UnityEngine;
using System.IO;
//------------------------------------------------------------------------
namespace FKGame
{

    public static class ResourcesConfigManager
    {
        public const string c_ManifestFileName = "ResourcesManifest";
        public const string c_PathKey = "Path";

        static DataTable s_config;
        static bool s_isInit = false;

        static void Initialize()
        {
            s_isInit = true;
            LoadResourceConfig();
        }

        public static void ClearConfig()
        {
            s_isInit = false;
        }

        public static bool GetIsExitRes(string resName)
        {
            resName = resName.ToLower();

            if (!s_isInit || s_config == null)
            {
                Initialize();
            }
            return s_config.ContainsKey(resName);
        }

        public static string GetResourcePath(string bundleName)
        {
            bundleName = bundleName.ToLower();

            if (!s_isInit)
            {
                Initialize();
            }

            if (!s_config.ContainsKey(bundleName))
            {
                throw new Exception("RecourcesConfigManager can't find ->" + bundleName + "<-");
            }

            return s_config[bundleName].GetString(c_PathKey);
        }
        /// <summary>
        /// ����AssetsLoadType��ȡ����·��
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public static string GetLoadPath(AssetsLoadType assetsloadType, string name)
        {
            string path = GetResourcePath(name);
            if (assetsloadType == AssetsLoadType.Resources)
                return path;
            else
            {
                return GetLoadPathBase(assetsloadType, path);
            }
        }
        public static string GetLoadPathBase(AssetsLoadType assetsloadType, string path)
        {
            //#if !UNITY_WEBGL

            bool isLoadByPersistent = RecordManager.GetData(HotUpdateManager.c_HotUpdateRecordName).GetRecord(path, "null") == "null" ? false : true;
            ResLoadLocation loadType = ResLoadLocation.Streaming;

            //����·���� ���ظ�Ŀ¼ �� ���·�� �ϲ�����
            //���ظ�Ŀ¼�����þ���
            if (isLoadByPersistent)
            {
                loadType = ResLoadLocation.Persistent;
                return PathTool.GetAssetsBundlePersistentPath() + path;
            }
            else
            {
                loadType = ResLoadLocation.Streaming;
                return PathTool.GetAbsolutePath(loadType, path);
            }

            //#else
            //        return PathTool.GetLoadURL(config.path + "." + c_AssetsBundlesExpandName);
            //#endif
        }
        public static void LoadResourceConfig()
        {
#if !UNITY_WEBGL
            string data = "";

            if (ResourceManager.LoadType == AssetsLoadType.Resources)
            {
                data = ResourceIOTool.ReadStringByResource(c_ManifestFileName + "." + DataManager.c_expandName);
            }
            else
            {
                ResLoadLocation type = ResLoadLocation.Streaming;
                string r_path = null;
                if (RecordManager.GetData(HotUpdateManager.c_HotUpdateRecordName).GetRecord(c_ManifestFileName.ToLower(), "null") != "null")
                {
                    Debug.Log("LoadResourceConfig ��ȡɳ��·��");

                    type = ResLoadLocation.Persistent;
                    //������Դ�����Application.persistentDataPath+"/Resources/"Ŀ¼��
                    r_path = PathTool.GetAssetsBundlePersistentPath() + c_ManifestFileName.ToLower();

                }
                else
                {
                    Debug.Log("LoadResourceConfig ��ȡstream·��");

                    r_path = PathTool.GetAbsolutePath(type, c_ManifestFileName.ToLower());

                }
                AssetBundle ab = AssetBundle.LoadFromFile(r_path);

                TextAsset text = ab.LoadAsset<TextAsset>(c_ManifestFileName);
                data = text.text;

                ab.Unload(true);
            }

            s_config = DataTable.Analysis(data);

#else
        return WEBGLReadResourceConfigContent();
#endif
        }

#if UNITY_EDITOR

        public const string c_ResourceParentPath = "/Resources/";
        public const string c_MainKey = "Res";
        static int direIndex = 0;

        public static bool GetIsExistResources()
        {
            string resourcePath = Application.dataPath + c_ResourceParentPath;
            return Directory.Exists(resourcePath);
        }

        public static void CreateResourcesConfig()
        {
            string content = DataTable.Serialize(GenerateResourcesConfig());
            string path = PathTool.GetAbsolutePath(ResLoadLocation.Resource, c_ManifestFileName + "." + DataManager.c_expandName);

            ResourceIOTool.WriteStringByFile(path, content);
        }

        /// <summary>
        /// ������Դ�嵥·��������Editor�¿��Ե���
        /// </summary>
        /// <returns></returns>
        public static DataTable GenerateResourcesConfig()
        {
            DataTable data = new DataTable();

            data.tableKeyDict.Add(c_MainKey);

            data.tableKeyDict.Add(c_PathKey);
            data.SetDefault(c_PathKey, "��Դ���·��");
            data.SetFieldType(c_PathKey, FieldType.String, null);

            string resourcePath = Application.dataPath + c_ResourceParentPath;
            direIndex = resourcePath.LastIndexOf(c_ResourceParentPath);
            direIndex += c_ResourceParentPath.Length;

            RecursionAddResouces(data, resourcePath);

            return data;
        }

        static void RecursionAddResouces(DataTable data, string path)
        {
            if (!File.Exists(path))
            {
                FileTool.CreatPath(path);
            }

            string[] dires = Directory.GetDirectories(path);

            for (int i = 0; i < dires.Length; i++)
            {
                RecursionAddResouces(data, dires[i]);
            }

            string[] files = Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(files[i]));
                string relativePath = files[i].Substring(direIndex);
                if (relativePath.EndsWith(".meta") || relativePath.EndsWith(".DS_Store"))
                    continue;
                else
                {
                    relativePath = FileTool.RemoveExpandName(relativePath).Replace("\\", "/");

                    SingleData sd = new SingleData();
                    sd.Add(c_MainKey, fileName.ToLower());
                    sd.Add(c_PathKey, relativePath.ToLower());

                    if (fileName.EndsWith(" "))
                    {
                        Debug.LogError("�ļ���β�����пո� ->" + fileName + "<-");
                    }
                    else
                    {
                        if (!data.ContainsKey(fileName.ToLower()))
                        {
                            data.AddData(sd);
                        }
                        else
                        {
                            Debug.LogError("GenerateResourcesConfig error ���������ļ���" + relativePath);
                        }
                    }
                }
            }
        }

#endif
    }
}