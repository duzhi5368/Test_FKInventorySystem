using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
//------------------------------------------------------------------------
namespace FKGame
{
    /// <summary>
    /// 资源读取器，负责从不同路径读取资源
    /// </summary>
    public class ResourceIOTool : MonoBehaviour
    {
        static ResourceIOTool instance;
        public static ResourceIOTool GetInstance()
        {
            if (instance == null)
            {
                GameObject resourceIOTool = new GameObject();
                resourceIOTool.name = "ResourceIO";
                DontDestroyOnLoad(resourceIOTool);

                instance = resourceIOTool.AddComponent<ResourceIOTool>();
            }
            return instance;
        }

        public static string ReadStringByFile(string path)
        {
            StringBuilder line = new StringBuilder();
            try
            {
                if (!File.Exists(path))
                {
                    Debug.Log("path dont exists ! : " + path);
                    return "";
                }

                StreamReader sr = File.OpenText(path);
                line.Append(sr.ReadToEnd());

                sr.Close();
                sr.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log("Load text fail ! message:" + e.Message);
            }
            return line.ToString();
        }

#pragma warning disable CS0618
        public static string ReadStringByBundle(string path)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            TextAsset ta = (TextAsset)ab.mainAsset;
            string content = ta.ToString();
            ab.Unload(true);
            return content;
        }
#pragma warning restore CS0618

        public static string ReadStringByResource(string path)
        {
            path = FileTool.RemoveExpandName(path);
            TextAsset text = (TextAsset)Resources.Load(path);

            if (text == null)
            {
                return "";
            }
            else
            {
                return text.text;
            }
        }

        public static Texture2D ReadTextureByFile(string path, int width, int height)
        {
            //创建文件读取流
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //创建Texture
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            return texture;
        }

        public static void ResourceLoadAsync(string path, Type resType, LoadCallBack callback)
        {
            GetInstance().MonoLoadMethod(path, resType, callback);
        }

        public void MonoLoadMethod(string path, Type resType, LoadCallBack callback)
        {
            StartCoroutine(MonoLoadByResourcesAsync(path, resType, callback));
        }

        LoadState m_loadState = new LoadState();
        public IEnumerator MonoLoadByResourcesAsync(string path, Type resType, LoadCallBack callback)
        {
            ResourceRequest status = null;
            try
            {
                if (resType == null)
                    status = Resources.LoadAsync(path);
                else
                    status = Resources.LoadAsync(path, resType);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                m_loadState.isDone = true;
                m_loadState.progress = 1;
                callback(m_loadState, null);
                yield break;
            }

            while (!status.isDone)
            {
                m_loadState.UpdateProgress(status);
                callback(m_loadState, null);

                yield return 0;
            }

            m_loadState.UpdateProgress(status);
            callback(m_loadState, status.asset);

        }

        /// <summary>
        /// 异步加载单个assetsbundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public static void AssetsBundleLoadAsync(string path, AssetBundleLoadCallBack callback)
        {
            GetInstance().MonoLoadAssetsBundleMethod(path, callback);
        }

        public void MonoLoadAssetsBundleMethod(string path, AssetBundleLoadCallBack callback)
        {
            StartCoroutine(MonoLoadByAssetsBundleAsync(path, callback));
        }

        public IEnumerator MonoLoadByAssetsBundleAsync(string path, AssetBundleLoadCallBack callback)
        {
#if !UNITY_WEBGL
            AssetBundleCreateRequest status = AssetBundle.LoadFromFileAsync(path);
            LoadState loadState = new LoadState();

            while (!status.isDone)
            {
                loadState.UpdateProgress(status);
                callback(loadState, null);

                yield return 0;
            }
            if (status.assetBundle != null)
            {
                status.assetBundle.name = path;
            }

            loadState.UpdateProgress(status);
            callback(loadState, status.assetBundle);
#else
        WWW www = new WWW(path);
        LoadState loadState = new LoadState();

        while (!www.isDone)
        {
            loadState.UpdateProgress(www);
            callback(loadState,resType, null);

            yield return 0;
        }
        if (www.assetBundle != null)
        {
            www.assetBundle.name = path;
        }

        loadState.UpdateProgress(www);
        callback(loadState,resType, www.assetBundle);
#endif
        }

        /// <summary>
        /// 异步加载WWW
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public static void WWWLoadAsync(string url, WWWLoadCallBack callback)
        {
            GetInstance().MonoLoadWWWethod(url, callback);
        }

        public void MonoLoadWWWethod(string url, WWWLoadCallBack callback)
        {
            StartCoroutine(MonoLoadByWWWAsync(url, callback));
        }

#pragma warning disable CS0618
        public IEnumerator MonoLoadByWWWAsync(string url, WWWLoadCallBack callback)
        {
            WWW www = new WWW(url);
            LoadState loadState = new LoadState();

            while (!www.isDone)
            {

                loadState.UpdateProgress(www);
                callback(loadState, www);

                yield return 0;
            }

            loadState.UpdateProgress(www);
            callback(loadState, www);
        }
#pragma warning restore CS0618

#if !UNITY_WEBGL || UNITY_EDITOR
        //web Player 不支持写操作
        public static void WriteStringByFile(string path, string content)
        {
            byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(content);
            CreateFile(path, dataByte);
        }

        public static void WriteTexture2DByFile(string path, Texture2D texture)
        {
            byte[] dataByte = texture.EncodeToPNG();
            CreateFile(path, dataByte);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.Log("File:[" + path + "] dont exists");
            }
        }

        public static void CreateFile(string path, byte[] byt)
        {
            try
            {
                FileTool.CreatFilePath(path);
                File.WriteAllBytes(path, byt);
            }
            catch (Exception e)
            {
                Debug.LogError("File Create Fail! \n" + e.Message);
            }
        }

#endif

    }

    public delegate void AssetBundleLoadCallBack(LoadState state, AssetBundle bundlle);
#pragma warning disable CS0618
    public delegate void WWWLoadCallBack(LoadState loadState, WWW www);
#pragma warning restore CS0618
    public delegate void LoadCallBack(LoadState loadState, object resObject);
    public class LoadState
    {
        private static LoadState completeState;

        public static LoadState CompleteState
        {
            get
            {
                if (completeState == null)
                {
                    completeState = new LoadState();
                    completeState.isDone = true;
                    completeState.progress = 1;
                }
                return completeState;
            }
        }

        public bool isDone;
        public float progress;

        public void UpdateProgress(ResourceRequest resourceRequest)
        {
            isDone = resourceRequest.isDone;
            progress = resourceRequest.progress;
        }

        public void UpdateProgress(AssetBundleCreateRequest assetBundleCreateRequest)
        {
            isDone = assetBundleCreateRequest.isDone;
            progress = assetBundleCreateRequest.progress;
        }

#pragma warning disable CS0618
        public void UpdateProgress(WWW www)
        {
            isDone = www.isDone;
            progress = www.progress;
        }
#pragma warning restore CS0618
    }
}