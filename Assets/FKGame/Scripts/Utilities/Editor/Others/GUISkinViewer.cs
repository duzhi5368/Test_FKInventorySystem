using FKGame.Macro;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GUISkinViewer : EditorWindow
    {
        private Vector2 scrollPosition = Vector2.zero;
        private string search = string.Empty;

        [MenuItem("Tools/FKGame/GUI样式查看器")]
        public static void Init()
        {
            EditorWindow window = GetWindow(typeof(GUISkinViewer));
            window.minSize = new Vector2(690, 300);
            window.titleContent = new GUIContent(LanguagesMacro.GUISKIN_VIEWER_TITLE);
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label("单击示例将复制其名到剪贴板", "label");
            GUILayout.FlexibleSpace();
            GUILayout.Label("查找:");
            search = EditorGUILayout.TextField(search);
            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (GUIStyle style in GUI.skin)
            {
                if (style.name.ToLower().Contains(search.ToLower()))
                {
                    GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                    GUILayout.Space(27);
                    if (GUILayout.Button(style.name, style))
                    {
                        EditorGUIUtility.systemCopyBuffer = "\"" + style.name + "\"";
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.SelectableLabel("\"" + style.name + "\"");
                    GUILayout.EndHorizontal();
                    GUILayout.Space(11);
                }
            }
            GUILayout.EndScrollView();
        }
    }
}
