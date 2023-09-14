using System.Text;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class PathTool
    {
        public static string GetPath(ResLoadLocation loadType)
        {
            StringBuilder path = new StringBuilder();
            switch (loadType)
            {
                case ResLoadLocation.Resource:
#if UNITY_EDITOR
                    path.Append(Application.dataPath);
                    path.Append("/Resources/");
                    break;
#endif

                case ResLoadLocation.Streaming:
#if UNITY_ANDROID && !UNITY_EDITOR
                    //path.Append("file://");
                    path.Append(Application.dataPath );
                    path.Append("!assets/");
#else
                    path.Append(Application.streamingAssetsPath);
                    path.Append("/");
#endif
                    break;

                case ResLoadLocation.Persistent:
                    path.Append(Application.persistentDataPath);
                    path.Append("/");
                    break;

                case ResLoadLocation.Catch:
                    path.Append(Application.temporaryCachePath);
                    path.Append("/");
                    break;

                default:
                    Debug.LogError("Type Error !" + loadType);
                    break;
            }
            return path.ToString();
        }

        // ������Դ�����Application.persistentDataPath+"/Resources/"Ŀ¼��
        public static string GetAssetsBundlePersistentPath()
        {
            return Application.persistentDataPath + "/Resources/";
        }

        /// <summary>
        /// ��Ͼ���·��
        /// </summary>
        /// <param name="loadType">��Դ��������</param>
        /// <param name="relativelyPath">���·��</param>
        /// <returns>����·��</returns>
        public static string GetAbsolutePath(ResLoadLocation loadType, string relativelyPath)
        {
            return GetPath(loadType) + relativelyPath;
        }

#if UNITY_WEBGL
    /// <summary>
    /// ��ȡ����URL
    /// </summary>
    /// <param name="relativelyPath">���·��</param>
    /// <returns></returns>
    public static string GetLoadURL(string relativelyPath)
    {
#if UNITY_EDITOR
        return "file://" + Application.streamingAssetsPath + "/" + relativelyPath;
#else
        return Application.absoluteURL + "StreamingAssets/" + relativelyPath;
#endif
    }
#endif

        // ��ȡ���·��
        public static string GetRelativelyPath(string path, string fileName, string expandName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(path);
            builder.Append("/");
            builder.Append(fileName);
            builder.Append(".");
            builder.Append(expandName);

            return builder.ToString();
        }

        /// <summary>
        /// ��ȡĳ��Ŀ¼�µ����·��
        /// </summary>
        /// <param name="FullPath">����·��</param>
        /// <param name="DirectoryPath">Ŀ��Ŀ¼</param>
        public static string GetDirectoryRelativePath(string DirectoryPath, string FullPath)
        {
            DirectoryPath = DirectoryPath.Replace(@"\", "/");
            FullPath = FullPath.Replace(@"\", "/");

            FullPath = FullPath.Replace(DirectoryPath, "");

            return FullPath;
        }

        /// <summary>
        /// ��ȡ�༭���µ�·��
        /// </summary>
        /// <param name="directoryName">Ŀ¼��</param>
        /// <param name="fileName">�ļ���</param>
        /// <param name="expandName">��չ��</param>
        /// <returns></returns>
        public static string GetEditorPath(string directoryName, string fileName, string expandName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Application.dataPath);
            builder.Append("/Editor");
            builder.Append(directoryName);
            builder.Append("/");
            builder.Append(fileName);
            builder.Append(".");
            builder.Append(expandName);

            return builder.ToString();
        }
    }
}