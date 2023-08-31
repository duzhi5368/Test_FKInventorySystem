using FKGame.Macro;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
// 风格图标查看器
//------------------------------------------------------------------------
namespace FKGame
{
    public class BuiltInResourcesWindow : EditorWindow
	{
		[MenuItem("Tools/FKGame/内置工具/内置风格和图标查看器")]
		public static void ShowWindow()
		{
			BuiltInResourcesWindow w = GetWindow<BuiltInResourcesWindow>();
            w.titleContent = new GUIContent(LanguagesMacro.STYLE_ICON_VIEWER_TITLE);
            w.Show();
		}

		private struct Drawing
		{
			public Rect Rect;
			public System.Action Draw;
		}

		private List<Drawing> m_ListDrawings;
		private List<Object> m_ListObjects;
		private float m_fScrollPos;
		private float m_fMaxY;
		private Rect m_OldPosition;

		private bool m_bIsShowingStyles = true;
		private bool m_bIsShowingIcons = false;

		private string m_strSearch = "";

		void OnGUI()
		{
			if (position.width != m_OldPosition.width && Event.current.type == EventType.Layout)
			{
				m_ListDrawings = null;
				m_OldPosition = position;
			}

			GUILayout.BeginHorizontal();
			if (GUILayout.Toggle(m_bIsShowingStyles, LanguagesMacro.INTERNAL_STYLES, EditorStyles.toolbarButton) != m_bIsShowingStyles)
			{
				m_bIsShowingStyles = !m_bIsShowingStyles;
				m_bIsShowingIcons = !m_bIsShowingStyles;
				m_ListDrawings = null;
			}
			if (GUILayout.Toggle(m_bIsShowingIcons, LanguagesMacro.INTERNAL_ICONS, EditorStyles.toolbarButton) != m_bIsShowingIcons)
			{
				m_bIsShowingIcons = !m_bIsShowingIcons;
				m_bIsShowingStyles = !m_bIsShowingIcons;
				m_ListDrawings = null;
			}
			GUILayout.EndHorizontal();

			string newSearch = GUILayout.TextField(m_strSearch);
			if (newSearch != m_strSearch)
			{
				m_strSearch = newSearch;
				m_ListDrawings = null;
			}

			float top = 36;
			if (m_ListDrawings == null)
			{
				string lowerSearch = m_strSearch.ToLower();
				m_ListDrawings = new List<Drawing>();
				GUIContent inactiveText = new GUIContent("inactive");
				GUIContent activeText = new GUIContent("active");
				float x = 5.0f;
				float y = 5.0f;
				if (m_bIsShowingStyles)
				{
					foreach (GUIStyle ss in GUI.skin.customStyles)
					{
						if (lowerSearch != "" && !ss.name.ToLower().Contains(lowerSearch))
							continue;

						GUIStyle thisStyle = ss;
						Drawing draw = new Drawing();
						float width = Mathf.Max(
							100.0f,
							GUI.skin.button.CalcSize(new GUIContent(ss.name)).x,
							ss.CalcSize(inactiveText).x + ss.CalcSize(activeText).x
							) + 16.0f;

						float height = 60.0f;
						if (x + width > position.width - 32 && x > 5.0f)
						{
							x = 5.0f;
							y += height + 10.0f;
						}
						draw.Rect = new Rect(x, y, width, height);
						width -= 8.0f;
						draw.Draw = () =>{
							if (GUILayout.Button(thisStyle.name, GUILayout.Width(width)))
								CopyText("(GUIStyle)\"" + thisStyle.name + "\"");

							GUILayout.BeginHorizontal();
							GUILayout.Toggle(false, inactiveText, thisStyle, GUILayout.Width(width / 2));
							GUILayout.Toggle(false, activeText, thisStyle, GUILayout.Width(width / 2));
							GUILayout.EndHorizontal();
						};
						x += width + 18.0f;

						m_ListDrawings.Add(draw);
					}
				}
				else if (m_bIsShowingIcons)
				{
					if (m_ListObjects == null)
					{
						m_ListObjects = new List<Object>(Resources.FindObjectsOfTypeAll(typeof(Texture2D)));
						m_ListObjects.Sort((pA, pB) => System.String.Compare(pA.name, pB.name, System.StringComparison.OrdinalIgnoreCase));
					}

					float rowHeight = 0.0f;
					foreach (Object oo in m_ListObjects)
					{
						Texture2D texture = (Texture2D)oo;
						if (texture.name == "")
							continue;
						if (lowerSearch != "" && !texture.name.ToLower().Contains(lowerSearch))
							continue;

						Drawing draw = new Drawing();
						float width = Mathf.Max(
							GUI.skin.button.CalcSize(new GUIContent(texture.name)).x,
							texture.width
							) + 8.0f;
						float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;
						if (x + width > position.width - 32.0f)
						{
							x = 5.0f;
							y += rowHeight + 8.0f;
							rowHeight = 0.0f;
						}
						draw.Rect = new Rect(x, y, width, height);
						rowHeight = Mathf.Max(rowHeight, height);
						width -= 8.0f;
						draw.Draw = () =>
						{
							// TODO: 分大小层级进行重绘
							if (GUILayout.Button(texture.name, GUILayout.Width(width)))
								CopyText("EditorGUIUtility.FindTexture( \"" + texture.name + "\" )");
							Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
							EditorGUI.DrawTextureTransparent(textureRect, texture);
						};
						x += width + 8.0f;
						m_ListDrawings.Add(draw);
					}
				}
				m_fMaxY = y;
			}

			Rect r = position;
			r.y = top;
			r.height -= r.y;
			r.x = r.width - 16;
			r.width = 16;

			float areaHeight = position.height - top;
			m_fScrollPos = GUI.VerticalScrollbar(r, m_fScrollPos, areaHeight, 0.0f, m_fMaxY);

			Rect area = new Rect(0, top, position.width - 16.0f, areaHeight);
			GUILayout.BeginArea(area);

			int count = 0;
			foreach (Drawing draw in m_ListDrawings)
			{
				Rect newRect = draw.Rect;
				newRect.y -= m_fScrollPos;
				if (newRect.y + newRect.height > 0 && newRect.y < areaHeight)
				{
					GUILayout.BeginArea(newRect, GUI.skin.textField);
					draw.Draw();
					GUILayout.EndArea();
					count++;
				}
			}
			GUILayout.EndArea();
		}

		void CopyText(string pText)
		{
			TextEditor editor = new TextEditor();
			editor.text = pText;
			editor.SelectAll();
			editor.Copy();
		}
	}
}