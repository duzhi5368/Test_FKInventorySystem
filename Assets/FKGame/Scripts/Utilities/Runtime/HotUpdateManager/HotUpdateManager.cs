using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // �ȶ��ļ��汾�����������ļ�������Update����
    public class HotUpdateManager
    {
        public struct DownLoadData
        {
            public string name;
            public Hash128 md5;
        }

        public const string c_HotUpdateRecordName = "HotUpdateRecord";
        public const string c_HotUpdateConfigName = "HotUpdateConfig";
        public static string c_versionFileName = "Version";
        public static string c_largeVersionKey = "LargeVersion";
        public static string c_smallVersonKey = "SmallVerson";
        public static string c_downLoadPathKey = "DownLoadPath";
        public static string c_UseTestDownLoadPathKey = "UseTestDownLoadPath";
        public static string c_testDownLoadPathKey = "TestDownLoadPath";
        public const string c_useHotUpdateRecordKey = "UseHotUpdate";

        static Dictionary<string, object> s_versionConfig;
        static string downLoadServicePath;
        static string s_versionFileDownLoadPath;
        static string s_ManifestFileDownLoadPath;
        static string s_resourcesFileDownLoadPath;
        static HotUpdateCallBack s_UpdateCallBack;
        static string s_versionFileCache;
        static byte[] s_versionByteCache;
        static AssetBundleManifest s_ManifestCache;
        static byte[] s_ManifestByteCache;
        static List<DownLoadData> s_downLoadList = new List<DownLoadData>();

#pragma warning disable 0618
        public static void StartHotUpdate(string hotUpdateURL, HotUpdateCallBack CallBack)
        {
            downLoadServicePath = hotUpdateURL;
            s_UpdateCallBack = CallBack;

            Init();
            //��ʼ�ȸ���
            ApplicationManager.Instance.StartCoroutine(HotUpdateProgress());
        }

        // �ȸ�������
        static IEnumerator HotUpdateProgress()
        {
            yield return CheckVersion();    // �ȼ���ļ��汾
        }

        public static bool CheckLocalVersion()
        {
            try
            {
                string StreamPath = PathTool.GetAbsolutePath(ResLoadLocation.Streaming, c_versionFileName.ToLower());
                //�жϱ����ļ��Ƿ����
                if (!File.Exists(StreamPath))
                {
                    Debug.LogError("���� Version �ļ������ڣ����ȴ��������ļ���");
                    return false;
                }
                int s_bigVersion = 0;
                int s_smallVersion = 0;
                GetVersion(StreamPath, ref s_bigVersion, ref s_smallVersion);
                string persistentPath = PathTool.GetAssetsBundlePersistentPath() + c_versionFileName;
                // �ж�ɳ��·���Ƿ����
                if (!File.Exists(persistentPath))
                {
                    Debug.Log("ɳ�� Version �ļ������ڣ�");
                    return false;
                }

                int p_bigVersion = 0;
                int p_smallVersion = 0;
                GetVersion(persistentPath, ref p_bigVersion, ref p_smallVersion);

                Debug.Log("largeVersionKey Streaming " + s_bigVersion + " ���� " + p_bigVersion);
                Debug.Log("smallVersonKey Streaming  " + s_smallVersion + " ���� " + p_smallVersion);

                // Streaming�汾�����Persistent�汾��Ҫ�£������Persistent�汾
                if (s_bigVersion > p_bigVersion ||
                   (s_bigVersion == p_bigVersion && s_smallVersion > p_smallVersion) ||
                   (s_bigVersion == p_bigVersion && s_smallVersion == p_smallVersion)
                    )
                {
                    Debug.Log("Streaming�汾��Persistent�汾��Ҫ��");
                    MemoryManager.FreeMemory();
                    RecordManager.CleanRecord(c_HotUpdateRecordName);
                    AssetsManifestManager.LoadAssetsManifest();
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return false;
        }

        private static void GetVersion(string path, ref int bigVersion, ref int smallVersion)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            TextAsset text = ab.LoadAsset<TextAsset>(c_versionFileName);
            string StreamVersionContent = text.text;
            ab.Unload(true);

            Dictionary<string, object> StreamVersion = (Dictionary<string, object>)MiniJSON.Deserialize(StreamVersionContent);
            bigVersion = GetInt(StreamVersion[c_largeVersionKey]);
            smallVersion = GetInt(StreamVersion[c_smallVersonKey]);
        }

        public static string GetHotUpdateVersion()
        {
            if (s_versionConfig == null)
            {
                s_versionConfig = (Dictionary<string, object>)MiniJSON.Deserialize(ReadVersionContent());
            }
            return GetInt(s_versionConfig[c_largeVersionKey]) + "." + GetInt(s_versionConfig[c_smallVersonKey]);
        }

        static IEnumerator CheckVersion()
        {
            UpdateDateCallBack(HotUpdateStatusEnum.DownLoadingVersionFile, 0);
            // ȡ�÷������汾�ļ�
            WWW www = new WWW(s_versionFileDownLoadPath);
            Debug.Log("��������ȡ�汾�ļ� ��" + s_versionFileDownLoadPath);
            while (!www.isDone)
            {
                UpdateDateCallBack(HotUpdateStatusEnum.DownLoadingVersionFile, GetHotUpdateProgress(false, false, www.progress));
                yield return new WaitForEndOfFrame();
            }

            if (www.error != null && www.error != "")
            {
                // ����ʧ��
                Debug.LogError("Version File DownLoad Error URL:" + s_versionFileDownLoadPath + " error:" + www.error);
                UpdateDateCallBack(HotUpdateStatusEnum.VersionFileDownLoadFail, 0);
                yield break;
            }

            s_versionFileCache = www.assetBundle.LoadAsset<TextAsset>(c_versionFileName).text;
            s_versionByteCache = www.bytes;
            www.assetBundle.Unload(true);

            UpdateDateCallBack(HotUpdateStatusEnum.DownLoadingVersionFile, GetHotUpdateProgress(false, false, 1));
            Debug.Log("�������汾��" + s_versionFileCache);
            Dictionary<string, object> ServiceVersion = (Dictionary<string, object>)MiniJSON.Deserialize(s_versionFileCache);

            // ��������汾�Ƚϴ���Ҫ��������
            if (GetInt(s_versionConfig[c_largeVersionKey]) < GetInt(ServiceVersion[c_largeVersionKey]))
            {
                Debug.Log("��Ҫ��������");
                UpdateDateCallBack(HotUpdateStatusEnum.NeedUpdateApplication, GetHotUpdateProgress(true, false, 0));
            }
            //��������汾�Ƚ�С���������
            else if (GetInt(s_versionConfig[c_largeVersionKey]) > GetInt(ServiceVersion[c_largeVersionKey]))
            {
                Debug.Log("��������汾�Ƚ�С��������£�ֱ�ӽ�����Ϸ");
                UpdateDateCallBack(HotUpdateStatusEnum.NoUpdate, 1);
                yield break;
            }
            //������С�汾�Ƚϴ󣬸����ļ�
            else if (GetInt(s_versionConfig[c_smallVersonKey]) < GetInt(ServiceVersion[c_smallVersonKey]))
            {
                Debug.Log("������С�汾�Ƚϴ󣬸����ļ�");
                UpdateDateCallBack(HotUpdateStatusEnum.Updating, GetHotUpdateProgress(true, false, 0));
                yield return DownLoadFile();
            }
            //������С�汾�Ƚ�С���������
            else
            {
                Debug.Log("������С�汾�Ƚ�С������ͬ��������£�ֱ�ӽ�����Ϸ");
                UpdateDateCallBack(HotUpdateStatusEnum.NoUpdate, 1);
                yield break;
            }
        }

        static int GetInt(object obj)
        {
            return int.Parse(obj.ToString());
        }

        // �����ļ�
        static IEnumerator DownLoadFile()
        {
            UpdateDateCallBack(HotUpdateStatusEnum.DownLoadingManifestFile, GetHotUpdateProgress(true, false, 0));
            // ȡ�÷������汾�ļ�
            WWW www = new WWW(s_ManifestFileDownLoadPath);
            Debug.Log("��������ȡ�嵥�ļ� ��" + s_ManifestFileDownLoadPath);
            while (!www.isDone)
            {
                UpdateDateCallBack(HotUpdateStatusEnum.DownLoadingManifestFile, GetHotUpdateProgress(true, false, www.progress));
                yield return new WaitForEndOfFrame();
            }

            if (www.error != null && www.error != "")
            {
                //����ʧ��
                Debug.LogError("MD5 DownLoad Error " + www.error);

                UpdateDateCallBack(HotUpdateStatusEnum.Md5FileDownLoadFail, GetHotUpdateProgress(true, false, 0));
                yield break;
            }

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            s_ManifestCache = www.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            s_ManifestByteCache = www.bytes;

            www.assetBundle.Unload(false);
            UpdateDateCallBack(HotUpdateStatusEnum.DownLoadingManifestFile, GetHotUpdateProgress(true, false, 1));

            s_downLoadList = new List<DownLoadData>();
            CheckBundleList(s_ManifestCache, AssetsManifestManager.GetManifest());

            yield return StartDownLoad();
        }

        static void CheckBundleList(AssetBundleManifest service, AssetBundleManifest local)
        {
            string[] allServiceBundle = service.GetAllAssetBundles();
            for (int i = 0; i < allServiceBundle.Length; i++)
            {
                Hash128 sHash = service.GetAssetBundleHash(allServiceBundle[i]);
                Hash128 lHash = local.GetAssetBundleHash(allServiceBundle[i]);

                if (!sHash.Equals(lHash))
                {
                    DownLoadData data = new DownLoadData();
                    data.name = allServiceBundle[i];
                    data.md5 = sHash;

                    s_downLoadList.Add(data);
                }
            }
        }

        static IEnumerator StartDownLoad()
        {
            Debug.Log("���ط������ļ�������");
            UpdateDateCallBack(HotUpdateStatusEnum.Updating, GetHotUpdateProgress(true, true, GetDownLoadFileProgress(0)));
            RecordTable hotupdateData = RecordManager.GetData(c_HotUpdateRecordName);
            for (int i = 0; i < s_downLoadList.Count; i++)
            {
                Hash128 md5Tmp = Hash128.Parse(hotupdateData.GetRecord(s_downLoadList[i].name, "null"));
                if (md5Tmp.Equals(s_downLoadList[i].md5))
                {
                    Debug.Log("�ļ��Ѹ������ " + s_downLoadList[i].name);
                    // ���ļ��Ѹ������
                    UpdateDateCallBack(HotUpdateStatusEnum.Updating, GetHotUpdateProgress(true, true, GetDownLoadFileProgress(i)));
                }
                else
                {
                    string downloadPath = s_resourcesFileDownLoadPath + s_downLoadList[i].name;
                    WWW www = new WWW(downloadPath);
                    yield return www;

                    if (www.error != null && www.error != "")
                    {
                        Debug.LogError("���س��� " + downloadPath + " " + www.error);
                        UpdateDateCallBack(HotUpdateStatusEnum.UpdateFail, GetHotUpdateProgress(true, true, GetDownLoadFileProgress(i)));
                        yield break;
                    }
                    else
                    {
                        Debug.Log("���سɹ��� " + downloadPath);
                        ResourceIOTool.CreateFile(PathTool.GetAssetsBundlePersistentPath() + "/" + s_downLoadList[i].name, www.bytes);
                        RecordManager.SaveRecord(c_HotUpdateRecordName, s_downLoadList[i].name, s_downLoadList[i].md5.ToString());
                        UpdateDateCallBack(HotUpdateStatusEnum.Updating, GetHotUpdateProgress(true, true, GetDownLoadFileProgress(i)));
                    }
                }
            }

            //����汾��Ϣ
            ResourceIOTool.CreateFile(PathTool.GetAssetsBundlePersistentPath() + c_versionFileName, s_versionByteCache);
            //�����ļ���Ϣ
            ResourceIOTool.CreateFile(PathTool.GetAssetsBundlePersistentPath() + AssetsManifestManager.c_ManifestFileName, s_ManifestByteCache);
            //��stream��ȡ����
            RecordManager.SaveRecord(c_HotUpdateRecordName, c_useHotUpdateRecordKey, true);
            //����������Դ����
            ResourcesConfigManager.LoadResourceConfig();
            AssetsManifestManager.LoadAssetsManifest();
            //�ӳ�2��ж��Bundle���棬��ֹ���½����ͼ������ʱ���ʱ��ж�ع��������ͼ��
            //yield return new WaitForSeconds(2);
            ResourceManager.ReleaseAll(false);
            UpdateDateCallBack(HotUpdateStatusEnum.UpdateSuccess, 1);
        }

        static void Init()
        {
            s_versionConfig = (Dictionary<string, object>)MiniJSON.Deserialize(ReadVersionContent());
            string downLoadPath = downLoadServicePath + "/" + platform + "/" + Application.version + "/";
            s_versionFileDownLoadPath = downLoadPath + c_versionFileName.ToLower();
            s_ManifestFileDownLoadPath = downLoadPath + AssetsManifestManager.c_ManifestFileName;
            s_resourcesFileDownLoadPath = downLoadPath;
            Debug.Log("=====>" + s_versionFileDownLoadPath);
        }

        static void UpdateDateCallBack(HotUpdateStatusEnum status, float progress)
        {
            try
            {
                s_UpdateCallBack(HotUpdateStatusInfo.GetUpdateInfo(status, progress));
            }
            catch (Exception e)
            {
                Debug.LogError("UpdateDateCallBack Error :" + e.ToString());
            }
        }

        static float GetHotUpdateProgress(bool isDownLoadVersion, bool isDownLoadMd5, float progress)
        {
            progress = Mathf.Clamp01(progress);
            if (!isDownLoadVersion)
            {
                return 0.1f * progress;
            }
            else if (!isDownLoadMd5)
            {
                return 0.1f + (0.1f * progress);
            }
            else
            {
                return 0.2f + (0.8f * progress);
            }
        }

        static float GetDownLoadFileProgress(int index)
        {
            if (s_downLoadList.Count == 0)
            {
                Debug.Log("�����б�Ϊ 0");
                return 0.95f;
            }

            return ((float)(index + 1) / (float)(s_downLoadList.Count + 1));
        }

        static string platform
        {
            get
            {
                string Platform = "Win";
#if UNITY_ANDROID //��׿
                Platform = "Android";
#elif UNITY_IOS //iPhone
                Platform = "IOS";
#elif UNITY_STANDALONE_OSX
                Platform = "Mac";
#elif UNITY_STANDALONE_LINUX
                Platform = "Linux";
#elif UNITY_STANDALONE_WIN
                Platform = "Win";
#endif
                return Platform;
            }
        }

        public static string ReadVersionContent()
        {
            string dataJson = "";
            if (ResourceManager.LoadType == AssetsLoadType.Resources)
            {
                dataJson = ResourceIOTool.ReadStringByResource(
                    c_versionFileName + "." + ConfigManager.c_expandName);
            }
            else
            {
                ResLoadLocation type = ResLoadLocation.Streaming;
                if (RecordManager.GetData(c_HotUpdateRecordName).GetRecord(c_useHotUpdateRecordKey, false))
                {
                    type = ResLoadLocation.Persistent;
                    string persistentPath = PathTool.GetAssetsBundlePersistentPath() + c_versionFileName;

                    AssetBundle ab = AssetBundle.LoadFromFile(persistentPath);
                    TextAsset text = ab.LoadAsset<TextAsset>(c_versionFileName);
                    dataJson = text.text;
                    ab.Unload(true);
                    Debug.Log("ɳ��·���汾��" + dataJson);
                }
                else
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(PathTool.GetAbsolutePath(type, c_versionFileName.ToLower()));
                    TextAsset text = ab.LoadAsset<TextAsset>(c_versionFileName);
                    dataJson = text.text;
                    ab.Unload(true);
                    Debug.Log("Streaming·���汾��" + dataJson);
                }
            }
            return dataJson;
        }

        public static string ReadLocalVersionContent()
        {
            string dataJson = "";
            string persistentPath = PathTool.GetAssetsBundlePersistentPath() + c_versionFileName;

            AssetBundle ab = AssetBundle.LoadFromFile(persistentPath);
            TextAsset text = ab.LoadAsset<TextAsset>(c_versionFileName);
            dataJson = text.text;
            ab.Unload(true);
            Debug.Log("ɳ��·���汾��" + dataJson);
            return dataJson;
        }

#pragma warning restore 0618
    }

    public delegate void HotUpdateCallBack(HotUpdateStatusInfo info);
}
