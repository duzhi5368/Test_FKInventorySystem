using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
    public class TableDataEditorWindow : EditorWindow
    {
        private static TableDataEditorWindow window;
        private TableDataEditor editor = new TableDataEditor();
        private string chooseFileName = "";

        [MenuItem("Tools/FKGame/����֧��/���ݱ༭��", priority = 501)]
        public static void ShowWindow()
        {
            window = EditorWindow.GetWindow<TableDataEditorWindow>(false, "���ݱ༭��");
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