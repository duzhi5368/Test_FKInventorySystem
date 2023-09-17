using FKGame.Macro;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioGroupSystemEditorWindow : EditorWindow
    {
        private string SaveDir = Application.dataPath + "/FKGame/Resources/" + GlobeDefine.CONFIG_DIRECTORY + "/";
        private List<AudioGroupData> datas = null;

        [MenuItem("Tools/FKGame/����֧��/������ϵͳ")]
        private static void OpenWindow()
        {
            AudioGroupSystemEditorWindow window = GetWindow<AudioGroupSystemEditorWindow>();
            window.titleContent = new GUIContent(LanguagesMacro.GAME_BOOT_CONFIG_WINODW_TITLE);
            Vector2 size = new Vector2(420f, 72f);
            window.minSize = size;
            window.Init();
        }

        private void OnEnable()
        {
            Init();
        }
        
        private void Init()
        {
            string path = SaveDir + AudioGroupSystem.ConfigName + ".txt";
            if (File.Exists(path))
            {
                string text = FileUtils.LoadTextFileByPath(path);
                datas = JsonSerializer.FromJson<List<AudioGroupData>>(text);
            }
            if (datas == null)
                datas = new List<AudioGroupData>();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            try
            {
                EditorDrawGUIUtil.DrawScrollView(this, () =>
                {
                    EditorDrawGUIUtil.DrawList("", datas, itemTitleName: (item) =>
                    {
                        AudioGroupData da = (AudioGroupData)item;
                        return da.keyName;
                    });
                });
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(LanguagesMacro.SAVE))
                {
                    string json = JsonSerializer.ToJson(datas);
                    FileUtils.CreateTextFile(SaveDir + AudioGroupSystem.ConfigName + ".txt", json);
                    ShowNotification(new GUIContent("�ѱ���!"));
                }
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }
        }
    }
}