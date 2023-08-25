using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public static class QuestSystemMenu
    {
        [MenuItem("Tools/FKGame/任务系统/编辑器", false, 0)]
        private static void OpenEditor()
        {
            QuestSystemEditor.ShowWindow();
        }

		[MenuItem("Tools/FKGame/任务系统/创建任务管理器", false, 1)]
		private static void CreateQuestManager()
		{
			GameObject go = new GameObject("Quest Manager");
			go.AddComponent<QuestManager>();
			Selection.activeGameObject = go;
		}

		[MenuItem("Tools/FKGame/任务系统/创建任务管理器", true)]
		static bool ValidateCreateQuestManager()
		{
			return GameObject.FindObjectOfType<QuestManager>() == null;
		}
	}
}