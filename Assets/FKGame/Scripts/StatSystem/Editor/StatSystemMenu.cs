using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    public static class StatSystemMenu
    {
        [MenuItem("Tools/FKGame/属性系统/编辑器", false, 0)]
        private static void OpenItemEditor()
        {
            StatSystemEditor.ShowWindow();
        }

		[MenuItem("Tools/FKGame/属性系统/创建属性管理器", false, 1)]
		private static void CreateStatManager()
		{
			GameObject go = new GameObject("Stats Manager");
			go.AddComponent<StatsManager>();
			Selection.activeGameObject = go;
		}

		[MenuItem("Tools/FKGame/属性系统/创建属性管理器", true)]
		private static bool ValidateCreateStatusSystem()
		{
			return GameObject.FindObjectOfType<StatsManager>() == null;
		}
	}
}