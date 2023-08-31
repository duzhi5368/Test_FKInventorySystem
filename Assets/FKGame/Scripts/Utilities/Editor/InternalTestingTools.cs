using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
//------------------------------------------------------------------------
// 常见工具
//------------------------------------------------------------------------
namespace FKGame
{
    public class InternalTestingTools 
    {
        [MenuItem("Tools/FKGame/内置工具/删除 PlayerPrefs 玩家数据缓存")]
        public static void DeletePlayerPrefs() 
        {
            bool result = EditorUtility.DisplayDialog("警告", "确定要删除全部用户数据吗？这会导致任务数据，属性数据，物品数据全部丢失哦。", "是", "否");
            if (result)
                PlayerPrefs.DeleteAll();
        }
        [MenuItem("Tools/FKGame/内置工具/删除 EditorPrefs 编辑器数据缓存")]
        public static void DeleteEditorPrefs()
        {
            bool result = EditorUtility.DisplayDialog("警告", "确定要删除编辑器数据吗？", "是", "否");
            if(result)
                EditorPrefs.DeleteAll();
        }
        [MenuItem("Tools/FKGame/内置工具/强制重编C#脚本")]
        public static void RecompileScripts()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}