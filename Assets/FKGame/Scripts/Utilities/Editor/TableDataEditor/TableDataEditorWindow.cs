using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
    public class TableDataEditorWindow : EditorWindow
    {
        private static TableDataEditorWindow window;
        private TableDataEditor editor = new TableDataEditor();
        private string chooseFileName = "";

        [MenuItem("Tools/FKGame/基础支持/数据编辑器", priority = 501)]
        public static void ShowWindow()
        {
            window = EditorWindow.GetWindow<TableDataEditorWindow>(false, "数据编辑器");
            window.autoRepaintOnSceneChange = true;
            window.wantsMouseMove = true;
        }

        private void OnEnable()
        {
            if (editor == null)
                editor = new TableDataEditor();
            editor.Init(this);
            GlobalEvent.AddEvent(EditorEvent.LanguageDataEditorChange, Refresh);
        }

        private void OnGUI()
        {
            chooseFileName = editor.OnGUI(chooseFileName);
        }

        private void OnDestroy()
        {
            editor.OnDestroy();
            GlobalEvent.RemoveEvent(EditorEvent.LanguageDataEditorChange, Refresh);
        }

        private void Refresh(params object[] args)
        {
            editor.Init(this);
        }
    }
}