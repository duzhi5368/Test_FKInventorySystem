using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GameBootConfigEditorWindow : EditorWindow
    {
        private static GameBootConfig config;
        private static Dictionary<Type, AppModuleBase> allModules = new Dictionary<Type, AppModuleBase>();

        [MenuItem("Tools/FKGame/����֧��/��Ϸ������ù���", priority = 0)]
        private static void OpenWindow()
        {
            GameBootConfigEditorWindow window = GetWindow<GameBootConfigEditorWindow>();
            window.titleContent = new GUIContent(LanguagesMacro.GAME_BOOT_CONFIG_WINODW_TITLE);
            Vector2 size = new Vector2(380f, 72f);
            window.minSize = size;
            window.Init();
        }

        private void Init()
        {
            config = GameBootConfig.LoadConfig();
            if (config == null)
                config = new GameBootConfig();
            allModules.Clear();
            Type[] types = ReflectionUtils.GetChildTypes(typeof(AppModuleBase));
            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                AppModuleBase appModule = null;
                if (config.allAppModuleSetting.ContainsKey(type.Name))
                {
                    appModule = (AppModuleBase)ReflectionUtils.CreateDefultInstance(type);
                    appModule = (AppModuleBase)config.allAppModuleSetting[type.Name].GetValue(appModule);
                }
                else
                {
                    appModule = (AppModuleBase)ReflectionUtils.CreateDefultInstance(type);
                }
                allModules.Add(type, appModule);
            }
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            EditorDrawGUIUtil.DrawScrollView(this, () =>
            {
                EditorDrawGUIUtil.DrawClassData("", config);
                foreach (var item in allModules)
                {
                    EditorDrawGUIUtil.DrawClassData(item.Key.Name, item.Value);
                }
            });
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("����"))
            {
                Save();
                AssetDatabase.Refresh();
                ShowNotification(new GUIContent("����ɹ���"));
            }
        }

        private void Save()
        {
            config.allAppModuleSetting.Clear();
            foreach (var item in allModules)
            {
                ClassValue value = new ClassValue(item.Value, false);
                config.allAppModuleSetting.Add(item.Key.Name, value);
            }
            GameBootConfig.Save(config);
        }
    }
}

