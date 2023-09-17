using UnityEngine;
using UnityEditor.ProjectWindowCallback;
using System.IO;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ApplicationStatusCreater
    {
        [MenuItem("Assets/Create/FKGame/��Application Statusģ�����", false, 90)]
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
                // ������Դ
                Object obj = CreateAssetFromTemplate(pathName, resourceFile);
                // ������ʾ����Դ
                ProjectWindowUtil.ShowCreatedAsset(obj);
            }
            internal static Object CreateAssetFromTemplate(string pahtName, string resourceFile)
            {
                // ��ȡҪ��������Դ�ľ���·��
                string fullName = Path.GetFullPath(pahtName);
                string className = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(fullName));
                // ��ȡ����ģ���ļ�
                StreamReader reader = new StreamReader(resourceFile);
                string content = reader.ReadToEnd();
                reader.Close();
                // �滻Ĭ�ϵ��ļ���
                content = content.Replace("{0}", className);
                // д�����ļ�
                StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
                writer.Write(content);
                writer.Close();
                //ˢ�±�����Դ
                AssetDatabase.ImportAsset(pahtName);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(pahtName, typeof(UnityEngine.Object));
            }
        }
    }
}
