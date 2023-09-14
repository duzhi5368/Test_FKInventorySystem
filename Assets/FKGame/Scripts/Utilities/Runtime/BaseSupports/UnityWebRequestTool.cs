using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UnityWebRequestTool
    {
        /// <param name="uri">不加端口时默认80端口 如：http://192.168.1.185:8181 或http://192.168.1.185</param>
        /// <param name="callBack">string：error， Dictionary<string,string> 返回的数据</param>
        public static void Get(string uri, CallBack<string, Dictionary<string, string>> callBack)
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(AsyGet(uri, callBack));
        }

        public static void Get(string uri, CallBack<string, string> callBack)
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(AsyGet(uri, callBack));
        }
        static IEnumerator AsyGet(string uri, CallBack<string, string> callBack)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            webRequest.method = UnityWebRequest.kHttpVerbGET;
            webRequest.timeout = 15;
            yield return webRequest.SendWebRequest();
            string error = null;
            
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
                error = webRequest.error;

            if (callBack != null)
            {
                callBack(error, webRequest.downloadHandler.text);
            }

        }
        static IEnumerator AsyGet(string uri, CallBack<string, Dictionary<string, string>> callBack)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            webRequest.method = UnityWebRequest.kHttpVerbGET;
            webRequest.timeout = 15;
            yield return webRequest.SendWebRequest();
            string error = null;
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
                error = webRequest.error;

            if (callBack != null)
            {
                callBack(error, ParseString(webRequest.downloadHandler.text));
            }

        }

        private static Dictionary<string, string> ParseString(string ss)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(ss))
            {
                return map;
            }
            try
            {
                string[] tempStr = ss.Split('&');
                if (tempStr.Length > 0)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        string[] pare = tempStr[i].Split('=');
                        map.Add(pare[0], pare[1]);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return map;
        }

        public static void Post(string uri, Dictionary<string, string> data, CallBack<string, Dictionary<string, string>> callBack)
        {
            MonoBehaviourRuntime.Instance.StartCoroutine(AsyPost(uri, data, callBack));
        }

        static IEnumerator AsyPost(string uri, Dictionary<string, string> data, CallBack<string, Dictionary<string, string>> callBack)
        {
            Debug.Log("Send Http Post ->" + uri);
            WWWForm form = new WWWForm();
            if (data != null)
            {
                foreach (var item in data)
                {
                    form.AddField(item.Key, item.Value);
                }
            }

            UnityWebRequest webRequest = UnityWebRequest.Post(uri, form);
            webRequest.timeout = 15;
            webRequest.method = UnityWebRequest.kHttpVerbPOST;
            yield return webRequest.SendWebRequest();
            string error = null;
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                error = webRequest.error;
                Debug.LogError(error);
            }

            if (callBack != null)
            {
                callBack(error, ParseString(webRequest.downloadHandler.text));
            }
        }
    }
}
