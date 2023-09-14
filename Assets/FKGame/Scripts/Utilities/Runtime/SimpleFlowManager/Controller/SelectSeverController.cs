using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SelectSeverController
    {
        private static CallBack<List<SelectNetworkData>> OnSelectServerComplete;
        private static string downLoadFilePath = "";
        private static string version;
        private static RuntimePlatform platform;
        private static string channel;
#pragma warning disable CS0618
        private static WWW www;
#pragma warning restore CS0618
        public static void Start(string url, string version, RuntimePlatform platform, string channel, CallBack<List<SelectNetworkData>> OnSelectServerComplete)
        {
            downLoadFilePath = url;

            SelectSeverController.version = version;
            SelectSeverController.platform = platform;
            SelectSeverController.channel = channel;
            SelectSeverController.OnSelectServerComplete = OnSelectServerComplete;
            Debug.Log("选服version：" + version);
            Debug.Log("选服platform：" + platform);
            Debug.Log("选服channel：" + channel);

            MonoBehaviourRuntime.Instance.StartCoroutine(Exqute());
        }
        
        static IEnumerator Exqute()
        {
            for (int i = 0; i < 10;)
            {
                if (www == null || www.isDone)
                {
                    Debug.Log("下载选服配置地址：" + downLoadFilePath);
#pragma warning disable CS0618
                    www = new WWW(downLoadFilePath);
                    yield return www;
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        i++;
                        Debug.LogError("SelectSeverController下载配置失败：" + downLoadFilePath);
                        yield return new WaitForEndOfFrame();
                    }
                    else
                    {
                        break;
                    }
#pragma warning restore CS0618
                }
            }
            if (string.IsNullOrEmpty(www.error))
            {
                List<SelectNetworkData> configs = DataTableExtend.GetTableDatas<SelectNetworkData>(www.text);
                Debug.Log("下载选服配置：" + www.text);
                Debug.Log("DataTableExtend.GetTableDatas：" + configs.Count);
                List<SelectNetworkData> selectConfig = new List<SelectNetworkData>();
                foreach (SelectNetworkData cc in configs)
                {
                    if (StringArrayHaveItem(cc.m_channel, channel))
                    {
                        if (platform == RuntimePlatform.Android)
                        {
                            if (StringArrayHaveItem(cc.m_androidVersion, version))
                            {
                                selectConfig.Add(cc);
                            }
                        }
                        else if (platform == (RuntimePlatform.IPhonePlayer))
                        {
                            if (StringArrayHaveItem(cc.m_iosVersion, version))
                            {
                                selectConfig.Add(cc);
                            }
                        }
                        else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer
                            || platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer
                            || platform == RuntimePlatform.LinuxEditor || platform == RuntimePlatform.LinuxPlayer
                            || platform == RuntimePlatform.WSAPlayerX86 || platform == RuntimePlatform.WSAPlayerX64 || platform == RuntimePlatform.WSAPlayerARM)
                        {
                            if (StringArrayHaveItem(cc.m_standaloneVersion, version))
                            {
                                selectConfig.Add(cc);
                            }
                        }
                    }
                }
                Debug.Log("选择服务器数目：" + selectConfig.Count);
                if (OnSelectServerComplete != null)
                {
                    OnSelectServerComplete(selectConfig);
                }
            }
            else
            {
                if (OnSelectServerComplete != null)
                {
                    OnSelectServerComplete(null);
                }
            }
        }
        private static bool StringArrayHaveItem(string[] arr, string item)
        {
            if (arr == null)
                return false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}