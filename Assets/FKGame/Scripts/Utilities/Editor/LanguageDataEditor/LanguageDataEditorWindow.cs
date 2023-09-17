using FKGame.Macro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LanguageDataEditorWindow : EditorWindow
    {
        private SystemLanguage currentLanguage = SystemLanguage.Unknown;
        private DataTable currentFileDataTable;
        private LanguageSettingConfig config;
        private FolderTreeView treeView;
        private TreeViewState treeViewState = null;

        // �����ļ���ת����ȫ·��/��
        private static List<string> s_languageFullKeyFileNameList = new List<string>();
        private static LanguageDataEditorWindow win = null;

        public int toolbarOption = 0;
        private string[] toolbarTexts = { "ģ���ļ�", "�������ݱ༭", "��������", "�����Թ���" };
        private bool richText = false;
        private string searchValue;
        private string searchKey = "";
        private bool isToggleMergeLanguage = false;
        private string mergePath = "";
        private SystemLanguage mergeLanguage;
        private string selectFullFileName = "";
        private string selectItemFullName = "";
        private string newField = "";

        [MenuItem("Tools/FKGame/����֧��/�����Ա༭��", priority = 600)]
        public static LanguageDataEditorWindow ShowWindow()
        {
            win = EditorWindow.GetWindow<LanguageDataEditorWindow>();
            win.Init();
            return win;
        }

        private void Init()
        {
            win = this;
            config = LanguageDataUtils.LoadRuntimeConfig();
            if (config == null)
            {
                return;
            }
            if (Application.isPlaying)
            {
                currentLanguage = LanguageManager.CurrentLanguage;
            }
            if (!config.gameExistLanguages.Contains(currentLanguage))
            {
                currentLanguage = config.defaultLanguage;
            }
            s_languageFullKeyFileNameList = LanguageDataEditorUtils.LoadLangusgeAllFileNames(currentLanguage);
            if (!string.IsNullOrEmpty(selectFullFileName))
            {
                currentFileDataTable = LanguageDataUtils.LoadFileData(currentLanguage, selectFullFileName);
            }
            if (treeViewState == null)
                treeViewState = new TreeViewState();

            treeView = new FolderTreeView(treeViewState);
            treeView.SetData(s_languageFullKeyFileNameList);
            treeView.dblclickItemCallBack = ModuleFileDblclickItemCallBack;
            treeView.selectCallBack = ModuleFileFolderSelectCallBack;
        }

        // ����޸�������������
        private void CheckAllLanguageFileIntact()
        {
            List<string> defaultFullKeyNameList = LanguageDataEditorUtils.LoadLangusgeAllFileNames(config.defaultLanguage);
            StringBuilder logBuider = new StringBuilder();
            logBuider.Append("��ʼ�޸�����������....\n");
            logBuider.Append("��׼����:" + config.defaultLanguage + "\n");

            foreach (var nowLan in config.gameExistLanguages)
            {
                if (nowLan == config.defaultLanguage)
                    continue;
                logBuider.Append("��ʼ�������:" + nowLan + "\n");
                List<string> nowFullKeyNameList = LanguageDataEditorUtils.LoadLangusgeAllFileNames(nowLan);
                foreach (var fullKeyFileName in defaultFullKeyNameList)
                {
                    DataTable dt = LanguageDataUtils.LoadFileData(config.defaultLanguage, fullKeyFileName);
                    if (!nowFullKeyNameList.Contains(fullKeyFileName))
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        foreach (var id in dt.tableIDDict)
                        {
                            dic.Add(id, "");
                        }
                        CreateLanguageNewFile(nowLan, fullKeyFileName, dic);
                        logBuider.Append(nowLan + "ȱ���ļ�:" + fullKeyFileName + " �����!\n");
                    }
                    else
                    {
                        DataTable nowDT = LanguageDataUtils.LoadFileData(nowLan, fullKeyFileName);
                        List<string> addKeys = new List<string>();
                        foreach (var id in dt.tableIDDict)
                        {
                            if (!nowDT.tableIDDict.Contains(id))
                            {
                                addKeys.Add(id);
                            }
                        }
                        AddNewKey(nowLan, fullKeyFileName, addKeys.ToArray());
                        if (addKeys.Count > 0)
                        {
                            logBuider.Append(nowLan + "=>" + fullKeyFileName + "ȱ���ֶ�:\n");
                            foreach (var item in addKeys)
                            {
                                logBuider.Append("\t" + item + "\n");
                            }
                            logBuider.Append("\n");
                        }
                    }
                }
            }
            logBuider.Append("�������������޸���ɣ�\n");
            AssetDatabase.Refresh();
            Debug.Log(logBuider.ToString());
            ShowNotification(new GUIContent("�޸����"));
        }

        void OnEnable()
        {
            Init();
        }

        void OnGUI()
        {
            titleContent.text = "�����Ա༭��";
            if (config == null || config.gameExistLanguages.Count == 0)
            {
                AddLanguageGUI();
                return;
            }
            richText = (bool)EditorDrawGUIUtil.DrawBaseValue("ʹ�ø��ı���ʾ��", richText);
            SelectLanguageGUI();
            DefaultLanguageGUI();
            SelectEditorModuleGUI();
            SearchValueGUI();
            toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts, GUILayout.Width(Screen.width));
            switch (toolbarOption)
            {
                case 0:
                    EditorLanguageModuleFileGUI();
                    break;
                case 1:
                    EditorLanguageFieldGUI();
                    break;
                case 2:
                    ConfigSettingGUI();
                    AddLanguageGUI();
                    DeleteLanguageGUI();
                    break;
                case 3:
                    LanguageToolGUI();
                    break;
            }
            GUILayout.FlexibleSpace();
            SaveDataGUI();
        }

        public static void OpenAndSearchValue(string fullFlieKey)
        {
            LanguageDataEditorWindow w = ShowWindow();
            w.searchValue = fullFlieKey;
            w.toolbarOption = 1;
        }

        private void SearchValueGUI()
        {
            searchValue = EditorDrawGUIUtil.DrawSearchField(searchValue);
            if (!string.IsNullOrEmpty(searchValue))
            {
                if (searchValue.Contains("/"))
                {
                    string[] tempV = searchValue.Split('/');
                    string key = tempV[tempV.Length - 1];
                    int indexEnd = searchValue.LastIndexOf("/");
                    string moduleName = searchValue.Remove(indexEnd);
                    if (s_languageFullKeyFileNameList.Contains(moduleName))
                    {
                        if (selectFullFileName != moduleName)
                        {
                            selectFullFileName = moduleName;
                            Debug.Log("moduleName :" + moduleName);
                            currentFileDataTable = LanguageDataUtils.LoadFileData(currentLanguage, selectFullFileName);
                            Debug.Log("currentFileDataTable :" + currentFileDataTable);
                            Debug.Log("  keys:" + currentFileDataTable.tableIDDict.Count);
                        }
                        searchKey = key;
                    }
                    else
                    {
                        selectFullFileName = "";
                        searchKey = "";
                    }
                }
                else
                {
                    searchKey = searchValue;
                }
            }
            else
            {
                searchKey = "";
            }
        }

        void DefaultLanguageGUI()
        {
            if (currentLanguage == config.defaultLanguage)
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField("Ĭ������");
            }
        }

        void ConfigSettingGUI()
        {
            config.useSystemLanguage = (bool)EditorDrawGUIUtil.DrawBaseValue("ʹ��ϵͳ����", config.useSystemLanguage);
            config.defaultLanguage = EditorDrawGUIUtil.DrawPopup("Ĭ������", config.defaultLanguage, config.gameExistLanguages);
            string lanNames = "";
            foreach (var item in config.gameExistLanguages)
            {
                lanNames += " " + item;
            }
            EditorGUILayout.LabelField("�Ѻ������ԣ�" + lanNames);
        }

        void SelectLanguageGUI()
        {
            GUILayout.BeginHorizontal();
            currentLanguage = EditorDrawGUIUtil.DrawPopup("��ǰ����", currentLanguage, config.gameExistLanguages, (lan) =>
            {
                if (!string.IsNullOrEmpty(selectFullFileName))
                    currentFileDataTable = LanguageDataUtils.LoadFileData(lan, selectFullFileName);
                Debug.Log("�л���:" + lan);
                LanguageManager.SetLanguage(lan);
            });
            if (currentLanguage == SystemLanguage.Chinese)
                currentLanguage = SystemLanguage.ChineseSimplified;
            if (GUILayout.Button("������һ�α���"))
            {
                DataManager.CleanCache();
                LanguageManager.IsInit = false;
                GlobalEvent.DispatchEvent(EditorEvent.LanguageDataEditorChange);
                currentFileDataTable = LanguageDataUtils.LoadFileData(currentLanguage, selectFullFileName);
                GUI.FocusControl("");
            }
            if (GUILayout.Button("����޸�������������"))
            {
                CheckAllLanguageFileIntact();
            }
            GUILayout.EndHorizontal();
        }

        void LanguageToolGUI()
        {
            isToggleMergeLanguage = GUILayout.Toggle(isToggleMergeLanguage, "�����Ժϲ�");
            if (isToggleMergeLanguage)
            {
                LanguageMergeGUI();
            }
        }

        void LanguageMergeGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("·��");
            mergePath = GUILayout.TextField(mergePath);
            GUILayout.EndHorizontal();
            mergeLanguage = EditorDrawGUIUtil.DrawPopup("��ǰҪ�ϲ�������", currentLanguage, config.gameExistLanguages);
            if (GUILayout.Button("�ϲ�"))
            {
                MergeLanguagePath(mergePath, mergeLanguage);
            }
        }

        void MergeLanguagePath(string path, SystemLanguage language)
        {
            List<string> list = FileTool.GetAllFileNamesByPath(path, new string[] { "txt" });
            for (int i = 0; i < list.Count; i++)
            {
                MergeLanguage(list[i], language);
            }
        }

        void MergeLanguage(string path, SystemLanguage language)
        {
            Debug.Log("MergeLanguage " + path);
            string languageKey = FileTool.GetFileNameByPath(FileTool.RemoveExpandName(path)).Replace(LanguageManager.c_DataFilePrefix + language + "_", "").Replace("_", "/");
            Debug.Log("languageKey " + languageKey);
            string content = ResourceIOTool.ReadStringByFile(path);
            DataTable aimLanguage = DataTable.Analysis(content);
            DataTable localLanguage = LanguageDataUtils.LoadFileData(language, languageKey);
            foreach (var key in aimLanguage.tableIDDict)
            {
                string value = aimLanguage[key].GetString(LanguageManager.c_valueKey);
                SingleData sd = new SingleData();
                sd.Add(LanguageManager.c_mainKey, key);
                sd.Add(LanguageManager.c_valueKey, value);
                if (!localLanguage.tableIDDict.Contains(key))
                {
                    Debug.Log("�����ֶ� " + key + " -> " + value);
                    localLanguage.AddData(sd);
                }
                else
                {
                    if (localLanguage[key].GetString(LanguageManager.c_valueKey) != value)
                    {
                        Debug.Log("�����ֶ� key" + key + " Value >" + localLanguage[key].GetString(LanguageManager.c_valueKey) + "< newValue >" + value + "<");
                        localLanguage[key] = sd;
                    }
                }
            }
            LanguageDataEditorUtils.SaveData(language, languageKey, localLanguage);
        }

        
        private void SelectEditorModuleGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("ѡ��༭ģ��");
            GUILayout.FlexibleSpace();
            EditorGUILayout.SelectableLabel(selectFullFileName);
            GUILayout.EndHorizontal();
        }

        // ģ���ļ���˫��������ѡ���ļ�
        private void ModuleFileDblclickItemCallBack(FolderTreeViewItem t)
        {
            if (t.isDirectory)
                return;
            selectFullFileName = t.fullPath;
            currentFileDataTable = LanguageDataUtils.LoadFileData(currentLanguage, selectFullFileName);
            toolbarOption = 1;
        }

        // ģ���ļ��е���ѡ���ļ�
        private void ModuleFileFolderSelectCallBack(FolderTreeViewItem t)
        {
            if (t.isDirectory)
                return;
            selectItemFullName = t.fullPath;
        }
        
        void EditorLanguageModuleFileGUI()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("��������ģ���б�(˫��ѡ���ļ�)");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("����ģ��", GUILayout.Width(70)))
            {
                AddLanguageModelGUI();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(9);
            GUIStyle style = "box";
            if (!string.IsNullOrEmpty(selectItemFullName))
            {
                style = "U2D.createRect";
            }
            GUILayout.BeginHorizontal(style);
            GUILayout.Label("ѡ����ļ���" + selectItemFullName);
            if (!string.IsNullOrEmpty(selectItemFullName))
            {
                if (GUILayout.Button("ɾ��", GUILayout.Width(40)))
                {
                    if (EditorUtility.DisplayDialog("��ʾ", "ȷ��ɾ�� :" + selectItemFullName, "OK", "Cancel"))
                    {
                        if (selectItemFullName == selectFullFileName)
                            selectFullFileName = "";
                        s_languageFullKeyFileNameList.Remove(selectItemFullName);
                        foreach (var lan in config.gameExistLanguages)
                        {
                            string path = LanguageDataUtils.GetLanguageSavePath(lan, selectItemFullName);
                            FileUtils.DeleteFile(path);
                        }
                        // SaveData();
                        AssetDatabase.Refresh();
                        selectItemFullName = "";
                        OnEnable();
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
            Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            treeView.OnGUI(rect);
        }

        void AddLanguageModelGUI()
        {
            GeneralDataModificationWindow.OpenWindow(this, "����ģ��", "", (value) =>
            {
                value = EditorDrawGUIUtil.DrawBaseValue("ģ����", value);
                string tempName = value.ToString();
                EditorGUILayout.HelpBox("�»��߷ָ��ļ��У��磺AA_BB_CC", MessageType.Info);
                if (string.IsNullOrEmpty(tempName))
                    EditorGUILayout.HelpBox("���ֲ���Ϊ��", MessageType.Error);
                if (s_languageFullKeyFileNameList.Contains(tempName.Replace('_', '/')))
                    EditorGUILayout.HelpBox("�����ظ�", MessageType.Error);
                return value;
            }, (value) =>
            {
                string tempName = value.ToString();
                if (string.IsNullOrEmpty(tempName))
                    return false;
                if (s_languageFullKeyFileNameList.Contains(tempName.Replace('_', '/')))
                    return false;
                return true;
            }, (value) =>
            {
                string fileName = value.ToString();
                CreateNewFile(fileName, null);
                string tempContent = fileName.Replace('_', '/');
                selectFullFileName = tempContent;
                //SaveData();
                Init();
            });
        }

        void EditorLanguageFieldGUI()
        {
            if (string.IsNullOrEmpty(selectFullFileName))
                return;
            if (currentFileDataTable == null)
                return;
            DataTable data = currentFileDataTable;
            List<string> languageKeyList = data.tableIDDict;
            AddLangeuageFieldGUI(languageKeyList);
            EditorGUILayout.Space();
            EditorDrawGUIUtil.DrawScrollView(languageKeyList, () =>
            {
                for (int i = 0; i < languageKeyList.Count; i++)
                {
                    string key = languageKeyList[i];
                    if (!string.IsNullOrEmpty(searchKey))
                        if (!key.Contains(searchKey))
                            continue;
                    GUILayout.Space(5);
                    GUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.BeginHorizontal();
                    string content = "";
                    if (data != null)
                    {
                        if (!data.ContainsKey(key))
                        {
                            SingleData sd = new SingleData();
                            sd.Add(LanguageManager.c_mainKey, key);
                            sd.Add(LanguageManager.c_valueKey, "");
                            data.AddData(sd);
                        }
                        content = data[key].GetString(LanguageManager.c_valueKey);
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("��ʾ", "ȷ��ɾ��key", "OK", "Cancel"))
                        {
                            DeleteKey(selectFullFileName, key);
                            Init();
                            return;
                        }
                    }
                    string showKeyValue = key;
                    if (currentLanguage != config.defaultLanguage)
                    {
                        showKeyValue += ":" + GetLanguageValue(config.defaultLanguage, selectFullFileName, key);
                    }
                    GUIStyle styleLable = "Label";
                    styleLable.wordWrap = true;
                    styleLable.richText = richText;
                    styleLable.alignment = TextAnchor.MiddleLeft;
                    EditorGUILayout.SelectableLabel(showKeyValue, styleLable);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUIStyle style = "TextArea";
                    style.wordWrap = true;
                    style.richText = richText;
                    content = EditorGUILayout.TextArea(content, style);
                    if (data != null)
                    {
                        data[key][LanguageManager.c_valueKey] = content;
                    }
                    if (GUILayout.Button("CopyPath", GUILayout.Width(75)))
                    {
                        string tempContent = selectFullFileName;
                        tempContent += "/" + key;
                        TextEditor tx = new TextEditor();
                        tx.text = tempContent;
                        tx.OnFocus();
                        tx.Copy();
                        ShowNotification(new GUIContent("�Ѹ���"));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }, "box");
        }

        void AddLangeuageFieldGUI(List<string> languageKeyList)
        {
            EditorGUILayout.LabelField("�����ֶ�");
            if (true)
            {
                EditorGUI.indentLevel = 3;
                newField = EditorGUILayout.TextField("�ֶ���", newField);

                if (newField != "" && !languageKeyList.Contains(newField))
                {
                    if (GUILayout.Button("���������ֶ�"))
                    {
                        AddNewKeyAllLanguage(selectFullFileName, newField);
                        Init();
                        newField = "";
                    }
                    EditorGUILayout.Space();
                }
                else
                {
                    if (languageKeyList.Contains(newField))
                    {
                        EditorGUILayout.LabelField("�ֶ����ظ���", EditorGUIStyleData.WarnMessageLabel);
                    }
                }
            }
        }

        // ��ȡĳ�����ԵĶ�����ֵ
        private string GetLanguageValue(SystemLanguage language, string fullFileName, string key)
        {
            DataTable data = LanguageDataUtils.LoadFileData(language, fullFileName);
            if (data.ContainsKey(key) && data[key].ContainsKey(LanguageManager.c_valueKey))
                return data[key][LanguageManager.c_valueKey];
            return "";
        }

        void DeleteLanguageGUI()
        {
            if (GUILayout.Button("ɾ������"))
            {
                SystemLanguage deleteLan = config.defaultLanguage;
                if (EditorUtility.DisplayDialog("����", "ȷ��Ҫɾ��[" + deleteLan + "]������", "��", "ȡ��"))
                {
                    config.gameExistLanguages.Remove(deleteLan);
                    if (config.gameExistLanguages.Count > 0)
                        config.defaultLanguage = config.gameExistLanguages[0];
                    else
                        config.defaultLanguage = SystemLanguage.Unknown;
                    if (!config.gameExistLanguages.Contains(currentLanguage))
                    {
                        currentLanguage = config.defaultLanguage;
                    }
                    try
                    {
                        currentFileDataTable = null;
                        selectFullFileName = "";
                        Directory.Delete(ResourcesMacro.LANGUAGE_DATA_SAVE_PATH_DIR + deleteLan, true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    SaveData();
                    AssetDatabase.Refresh();
                }
            }
        }

        void AddMissLanguageGUI(DataTable data, string key)
        {
            if (GUILayout.Button("��Ӽ�¼"))
            {
                SingleData newData = new SingleData();
                newData.Add(LanguageManager.c_valueKey, "");
                newData.Add(LanguageManager.c_mainKey, key);
                data.AddData(newData);
            }
        }

        void SaveDataGUI()
        {
            if (GUILayout.Button("����"))
            {
                SaveData();
                ShowNotification(new GUIContent("�ѱ���"));
            }
        }

        void SaveData()
        {
            LanguageDataUtils.SaveEditorConfig(config);
            LanguageDataEditorUtils.SaveData(currentLanguage, selectFullFileName, currentFileDataTable);
            LanguageManager.Release();
            LanguageManager.SetLanguage(currentLanguage);
            GlobalEvent.DispatchEvent(EditorEvent.LanguageDataEditorChange);
            UnityEditor.AssetDatabase.Refresh();
        }

        void AddLanguageGUI()
        {
            if (GUILayout.Button("��������"))
            {
                GeneralDataModificationWindow.OpenWindow(this, "��������", SystemLanguage.Afrikaans, (value) =>
                {
                    SystemLanguage lan = (SystemLanguage)value;
                    lan = (SystemLanguage)EditorDrawGUIUtil.DrawBaseValue("���ԣ�", lan);
                    if (config != null && config.gameExistLanguages.Contains(lan))
                    {
                        EditorGUILayout.HelpBox("�Ѵ���", MessageType.Error);
                    }
                    if (lan == SystemLanguage.Chinese)
                        lan = SystemLanguage.ChineseSimplified;
                    if (lan == SystemLanguage.Unknown)
                        lan = SystemLanguage.ChineseSimplified;
                    return lan;
                }, (value) =>
                {
                    SystemLanguage lan = (SystemLanguage)value;
                    if (config != null && config.gameExistLanguages.Contains(lan))
                    {
                        return false;
                    }
                    return true;
                }, (value) =>
                {
                    if (config == null)
                        config = new LanguageSettingConfig();
                    SystemLanguage lan = (SystemLanguage)value;
                    config.gameExistLanguages.Add(lan);
                    CreateNewLangusge(lan);
                });
            }
        }

        private void CreateNewLangusge(SystemLanguage lan)
        {
            if (config == null)
            {
                config = new LanguageSettingConfig();
            }
            if (config.defaultLanguage == SystemLanguage.Unknown)
            {
                config.defaultLanguage = lan;
                config.gameExistLanguages.Add(lan);
                currentLanguage = lan;
            }
            else
            {
                foreach (var item in s_languageFullKeyFileNameList)
                {
                    DataTable dt = LanguageDataUtils.LoadFileData(config.defaultLanguage, item);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (var id in dt.tableIDDict)
                    {
                        dic.Add(id, "");
                    }
                    CreateLanguageNewFile(lan, item, dic);
                }
            }
            SaveData();
            AssetDatabase.Refresh();
            Init();
        }

        /// <summary>
        /// �½��������ļ�,��ÿ������������ļ�
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentDic"></param>
        /// <returns>����ÿ��key��Ӧ�Ķ����Է���key</returns>
        public Dictionary<string, string> CreateNewFile(string fileName, Dictionary<string, string> contentDic)
        {
            Dictionary<string, string> keyPaths = new Dictionary<string, string>();
            string tempContent = fileName.Replace('_', '/');
            // ��ÿ����������ļ�
            foreach (var item in config.gameExistLanguages)
            {
                keyPaths = CreateLanguageNewFile(item, tempContent, contentDic);
            }
            UnityEditor.AssetDatabase.Refresh();
            return keyPaths;
        }

        public Dictionary<string, string> CreateLanguageNewFile(SystemLanguage language, string fullKeyFileName, Dictionary<string, string> contentDic)
        {
            Dictionary<string, string> keyPaths = new Dictionary<string, string>();
            DataTable data = new DataTable();
            data.tableKeyDict.Add(LanguageManager.c_mainKey);
            data.tableKeyDict.Add(LanguageManager.c_valueKey);
            data.SetDefault(LanguageManager.c_valueKey, "NoValue");
            if (contentDic != null)
            {
                foreach (var item in contentDic)
                {
                    SingleData sd = new SingleData();
                    sd.Add(LanguageManager.c_mainKey, item.Key);
                    sd.Add(LanguageManager.c_valueKey, item.Value);
                    data.AddData(sd);
                    keyPaths.Add(item.Key, fullKeyFileName + "/" + item.Key);
                }

            }
            LanguageDataEditorUtils.SaveData(language, fullKeyFileName, data);
            return keyPaths;
        }

        private void AddNewKeyAllLanguage(string fullKeyFileName, string key)
        {
            foreach (var language in config.gameExistLanguages)
            {
                AddNewKey(language, fullKeyFileName, new string[] { key });
            }
            UnityEditor.AssetDatabase.Refresh();
        }

        private void AddNewKey(SystemLanguage language, string fullKeyFileName, string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return;
            DataTable data = LanguageDataUtils.LoadFileData(language, fullKeyFileName);
            foreach (var key in keys)
            {
                if (data.ContainsKey(key))
                    continue;
                SingleData sd = new SingleData();
                sd.Add(LanguageManager.c_mainKey, key);
                sd.Add(LanguageManager.c_valueKey, "");
                data.AddData(sd);
            }
            LanguageDataEditorUtils.SaveData(language, fullKeyFileName, data);
            UnityEditor.AssetDatabase.Refresh();
        }

        private void DeleteKey(string fullKeyFileName, string key)
        {
            foreach (var language in config.gameExistLanguages)
            {
                DataTable data = LanguageDataUtils.LoadFileData(language, fullKeyFileName);
                data.RemoveData(key);
                LanguageDataEditorUtils.SaveData(language, fullKeyFileName, data);
            }
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}