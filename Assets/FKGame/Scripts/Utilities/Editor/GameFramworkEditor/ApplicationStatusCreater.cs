using UnityEngine;
using UnityEditor.ProjectWindowCallback;
using System.IO;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ApplicationStatusCreater
    {
        [MenuItem("Assets/Create/FKGame/新Application Status模板代码", false, 90)]
        public static void CreateNewApplicationStatusCode()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateScriptAssetAction>(),
                GetSelectedPathOrFallback() + "/NewFKApplicationStatus.cs",
                null,
                Application.dataPath + "/FKGame/Resources/" + GlobeDefine.CODE_TEMPLATE_DIRECTORY + "/ApplicationStatusTemplate.txt");
        }

        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }

        class CreateScriptAssetAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // 创建资源
                Object obj = CreateAssetFromTemplate(pathName, resourceFile);
                // 高亮显示该资源
                ProjectWindowUtil.ShowCreatedAsset(obj);
            }
            internal static Object CreateAssetFromTemplate(string pahtName, string resourceFile)
            {
                // 获取要创建的资源的绝对路径
                string fullName = Path.GetFullPath(pahtName);
                string className = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(fullName));
                // 读取本地模板文件
                StreamReader reader = new StreamReader(resourceFile);
                string content = reader.ReadToEnd();
                reader.Close();
                // 替换默认的文件名
                content = content.Replace("{0}", className);
                // 写入新文件
                StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
                writer.Write(content);
                writer.Close();
                //刷新本地资源
                AssetDatabase.ImportAsset(pahtName);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(pahtName, typeof(UnityEngine.Object));
            }
        }
    }
}
