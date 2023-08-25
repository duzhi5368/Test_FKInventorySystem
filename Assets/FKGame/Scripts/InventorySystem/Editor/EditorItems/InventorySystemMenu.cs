using UnityEngine;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
	public static class InventorySystemMenu
	{
		[MenuItem ("Tools/FKGame/物品系统/编辑器", false, 0)]
		private static void OpenItemEditor ()
		{
			InventorySystemEditor.ShowWindow ();
		}

		[MenuItem("Tools/FKGame/物品系统/合并物品数据库", false, 1)]
		private static void OpenMergeDatabaseEditor()
		{
			MergeDatabaseEditor.ShowWindow();
		}

		[MenuItem("Tools/FKGame/物品系统/物品升级", false, 2)]
		private static void OpenItemReferenceEditor()
		{
			ItemReferenceEditor.ShowWindow();
		}

		[MenuItem ("Tools/FKGame/物品系统/创建物品管理器", false, 3)]
		private static void CreateInventoryManager ()
		{
			GameObject go = new GameObject("Inventory Manager");
			go.AddComponent<InventoryManager> ();
			Selection.activeGameObject = go;
		}

		[MenuItem ("Tools/FKGame/物品系统/创建物品管理器", true)]
		static bool ValidateCreateInventoryManager()
		{
			return GameObject.FindObjectOfType<InventoryManager> () == null;
		}
	}
}