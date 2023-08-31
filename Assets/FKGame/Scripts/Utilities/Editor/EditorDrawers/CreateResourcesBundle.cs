using FKGame.Macro;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
// 资源压缩包工具
//------------------------------------------------------------------------
namespace FKGame
{
    public class CreateResourcesBundle : Editor
    {
        [MenuItem("Tools/FKGame/内置工具/创建压缩资源包")]
        public static void BuildCompressionAssetBundle()
        {
            int nIndex = EditorUtility.DisplayDialogComplex("提示", "你需要打包 AssetBundle 资源包已运行程序。", "Windows", "Android", "IOS");
            switch(nIndex)
            {
                case 0:
                    CreateWindowsBundle();
                    break;
                case 1:
                    CreateAndroidBundle();
                    break;
                case 2:
                    CreateIOSBundle();
                    break;
            }
            AssetDatabase.Refresh();
        }

        //参数1 为要查找的总路径， 参数2 保存路径  
        private static void GetDirs(string dirPath, ref List<string> dirs)
        {
            foreach (string path in Directory.GetFiles(dirPath, "*.*"))
            {
                dirs.Add(path.Substring(path.IndexOf("Assets")));
            }

            if (Directory.GetDirectories(dirPath).Length > 0)  //遍历所有文件夹  
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                {
                    GetDirs(path, ref dirs);
                }
            }
        }

        private static void SetAssetImporterDirs(string dirPath)
        {
            DirectoryInfo folder = new DirectoryInfo(dirPath);
            FileSystemInfo[] files = folder.GetFileSystemInfos();
            int length = files.Length;
            for (int i = 0; i < length; i++)
            {
                if (files[i] is DirectoryInfo)
                {
                    SetAssetImporterDirs(files[i].FullName);
                }
                else
                {
                    if (!files[i].Name.EndsWith(".meta"))
                    {
                        SetAssetImporterFiles(files[i].FullName);
                    }
                }
            }
        }

        static void SetAssetImporterFiles(string source)
        {
            string _source = source.Replace('\\', '/');
            string _assetPath = "Assets" + _source.Substring(Application.dataPath.Length);
            string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
            AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
            string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
            assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
            assetImporter.assetBundleName = assetName;
        }


        [MenuItem("Tools/FKGame/内置工具/创建资源包")]
        public static void BuildAssetBundle()
        {
            ClearAssetBundlesName();
            SetAssetImporterDirs(ResourcesMacro.DEFAULT_RESOURCES_DIR);
            string outputPath = GetOutputPath();
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            BuildPipeline.BuildAssetBundles(outputPath, 0, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            Debug.Log("打包完成");
        }

        static string GetOutputPath()
        {
            string outputPath = Path.Combine(Application.streamingAssetsPath,
                GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget));
            return outputPath;
        }

        // 清除之前的资源包名称
        static void ClearAssetBundlesName()
        {
            int length = AssetDatabase.GetAllAssetBundleNames().Length;
            Debug.Log("期望打包资源数量：" + length);
            string[] oldAssetBundleNames = new string[length];
            for (int i = 0; i < length; i++)
            {
                oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            }

            for (int j = 0; j < oldAssetBundleNames.Length; j++)
            {
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
            }
            length = AssetDatabase.GetAllAssetBundleNames().Length;
            Debug.Log("实际打包资源数量：" + length);
        }

        static void CreateWindowsBundle()
        {
            List<string> f = new List<string>();
            GetDirs(ResourcesMacro.DEFAULT_RESOURCES_DIR, ref f);
            string[] files = f.ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = ResourcesMacro.STREAMING_ASSET_BUNDLE_NAME;
            build.assetNames = files;
            Debug.Log("实际打包资源数量：" + files.Length);
            if (!Directory.Exists(Application.streamingAssetsPath + "/Compression/"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/Compression/");
            }
            BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Compression/", new AssetBundleBuild[] { build }, 
                BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
            Debug.Log("打包完毕");
        }

        static void CreateAndroidBundle()
        {
            List<string> f = new List<string>();
            GetDirs(ResourcesMacro.DEFAULT_RESOURCES_DIR, ref f);
            string[] files = f.ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = ResourcesMacro.STREAMING_ASSET_BUNDLE_NAME;
            build.assetNames = files;
            Debug.Log("实际打包资源数量：" + files.Length);
            if (!Directory.Exists(Application.streamingAssetsPath + "/Compression/"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/Compression/");
            }
            BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Compression/", new AssetBundleBuild[] { build }, 
                BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
            Debug.Log("打包完毕");
        }

        static void CreateIOSBundle()
        {
            List<string> f = new List<string>();
            GetDirs(ResourcesMacro.DEFAULT_RESOURCES_DIR, ref f);
            string[] files = f.ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = ResourcesMacro.STREAMING_ASSET_BUNDLE_NAME;
            build.assetNames = files;
            Debug.Log("实际打包资源数量：" + files.Length);
            if (!Directory.Exists(Application.streamingAssetsPath + "/Compression/"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/Compression/");
            }
            BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Compression/", new AssetBundleBuild[] { build },
                BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
            Debug.Log("打包完毕");
        }

        static string GetPlatformFolder(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "IOS";
                case BuildTarget.WebGL:
                    return "WebPlayer";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSX:
                    return "OSX";
                default:
                    return null;
            }
        }
    }
}