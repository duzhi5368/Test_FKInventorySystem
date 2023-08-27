using FKGame.Macro;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [InitializeOnLoad]
    public class CreateResourcesBundle
    {
        static CreateResourcesBundle()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            if (!EditorApplication.isCompiling)
            {
                OnUnityScripsCompilingCompleted();
                EditorApplication.update -= Update;
            }
        }

        static void OnUnityScripsCompilingCompleted()
        {
            if(!File.Exists(Application.streamingAssetsPath + "/" + ResourcesMacro.ASSET_BUNDLE_NAME))
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
            }
        }

        //参数1 为要查找的总路径， 参数2 保存路径  
        private static void GetDirs(string dirPath, ref List<string> dirs)
        {
            foreach (string path in Directory.GetFiles(dirPath, "*.*"))
            {
                dirs.Add(path.Substring(path.IndexOf("Assets")));
                Debug.Log(path.Substring(path.IndexOf("Assets")));
            }

            if (Directory.GetDirectories(dirPath).Length > 0)  //遍历所有文件夹  
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                {
                    GetDirs(path, ref dirs);
                }
            }
        }

        [MenuItem("FKGame/内置工具/创建资源包(Windows)")]
        static void CreateWindowsBundle()
        {
            List<string> f = new List<string>();
            GetDirs(ResourcesMacro.DEFAULT_RESOURCES_DIR, ref f);
            string[] files = f.ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                Debug.Log(files[i]);
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = ResourcesMacro.ASSET_BUNDLE_NAME;
            build.assetNames = files;
            if (!Directory.Exists("Assets/StreamingAssets"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets");
            }
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", new AssetBundleBuild[] { build }, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        }

        [MenuItem("FKGame/内置工具/创建资源包(Android)")]
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
            build.assetBundleName = ResourcesMacro.ASSET_BUNDLE_NAME;
            build.assetNames = files;
            if (!Directory.Exists("Assets/StreamingAssets"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets");
            }
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", new AssetBundleBuild[] { build }, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }

        [MenuItem("FKGame/内置工具/创建资源包(IOS)")]
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
            build.assetBundleName = ResourcesMacro.ASSET_BUNDLE_NAME;
            build.assetNames = files;
            if (!Directory.Exists("Assets/StreamingAssets"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets");
            }
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", new AssetBundleBuild[] { build }, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
        }
    }
}