using FKGame.Macro;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
	public class CopyComponentEditor : EditorWindow
	{
		private GameObject m_Source;
		private GameObject m_Destination;

		[UnityEditor.MenuItem("Tools/FKGame/游戏对象组件拷贝工具", false)]
		public static void ShowWindow()
		{
			CopyComponentEditor window = EditorWindow.GetWindow<CopyComponentEditor>(LanguagesMacro.COMPONENT_COPY_EDITOR_TITLE);
			Vector2 size = new Vector2(300f, 80f);
			window.minSize = size;
			window.wantsMouseMove = true;
		}

		private Vector2 m_ScrollPosition;
		private Dictionary<Component, bool> m_ComponentMap;

        private void OnGUI()
        {
			this.m_Source = EditorGUILayout.ObjectField("源 GameObject",this.m_Source, typeof(GameObject),true) as GameObject;
			this.m_Destination= EditorGUILayout.ObjectField("目标 GameObject",this.m_Destination, typeof(GameObject), true) as GameObject;
			if (this.m_Source == null || this.m_Destination == null)
				return;

			if (this.m_ComponentMap == null)
			{
				this.m_ComponentMap = new Dictionary<Component, bool>();
				Component[] components = this.m_Source.GetComponents<Component>().Where(x => x.hideFlags == HideFlags.None).ToArray();
				for (int i = 0; i < components.Length; i++)
				{
					if (ComponentUtility.CopyComponent(components[i]))
					{
						this.m_ComponentMap.Add(components[i],true);
					}
				}
			}
			this.m_ScrollPosition = EditorGUILayout.BeginScrollView(this.m_ScrollPosition);
			GUILayout.Label("组件列表", EditorStyles.boldLabel);
			for (int i = 0; i < this.m_ComponentMap.Count; i++) {
				this.m_ComponentMap[this.m_ComponentMap.ElementAt(i).Key] = EditorGUILayout.Toggle(this.m_ComponentMap.ElementAt(i).Key.GetType().Name, this.m_ComponentMap.ElementAt(i).Value);
			}
			EditorGUILayout.EndScrollView();

			GUILayout.FlexibleSpace();
			if (GUILayout.Button("拷贝全部组件")) {
				foreach(KeyValuePair<Component,bool> kvp in this.m_ComponentMap)
				{
					if (kvp.Value && ComponentUtility.CopyComponent(kvp.Key))
					{
						Component component = this.m_Destination.AddComponent(kvp.Key.GetType()) as Component;
						ComponentUtility.PasteComponentValues(component);
					}
				}
				Selection.activeObject = m_Destination;
			}
		}	
	}
}