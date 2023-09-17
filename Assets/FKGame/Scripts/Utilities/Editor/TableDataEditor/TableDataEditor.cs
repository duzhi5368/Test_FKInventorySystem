using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.IO;
//------------------------------------------------------------------------
namespace FKGame
{
    public class TableDataEditor
    {
        private const string FontPlayerPrefKey = "DataEditorWindow.FontKey";
        private List<String> configFileNames = new List<string>();
        private DataTable m_currentData;
        private List<string> langKeys;
        private EditorWindow editorWindow;
        private string chooseFileName = "";
        private string resetChooseFileName = "";

        private List<float> withItemList = new List<float>();
        private List<float> heightItemList = new List<float>();
        private int minItem = 30;
        private const int wHeight = 45;
        private const int wWith = 110;
        private Vector2 svPos;
        private List<int> selectRowIndexs = new List<int>();
        private List<int> selectColumnIndexs = new List<int>();
        private GUIStyle buttonStyle;
        private GUIStyle helpBoxStyle;
        private Vector2 topGridSVPos;
        private Vector2 leftGridSVPos;

        private int oldButtonFontSize = 0;
        private int nowButtonFontSize = 0;
        private int MaxButtonFontSize = 40;
        private string searchValue = "";
        private List<int> showSearchValueIndexList = new List<int>();       // 用于搜索时记录只显示对于行的Index
        private List<int> saveSearchValueIndexList = new List<int>();

        private List<string> fieldNames = new List<string>();
        private List<string> tableKeys = new List<string>();
        private string tableName = "";
        private string fieldName = "";
        private string tableKey = "";

        private Editor previewEditor;
        private GameObject previewObj;
        private object editPrefabValue = null;
        private Editor previewEditor1;
        private Texture2D previewTex;
        private object editTextureValue;
        private Vector2Int modifiIndex = new Vector2Int();

        public void Init(EditorWindow editorWindow)
        {
            if (editorWindow)
                this.editorWindow = editorWindow;
            langKeys = LanguageDataEditorUtils.GetLanguageAllFunKeyList();
            configFileNames.Clear();
            string m_directoryPath = Application.dataPath + DataManager.directoryName;
            if (Directory.Exists(m_directoryPath))
            {
                configFileNames.AddRange(PathUtils.GetDirectoryFileNames(m_directoryPath, new string[] { ".txt" }, false, false));
            }
            if (!string.IsNullOrEmpty(chooseFileName) && configFileNames.Contains(chooseFileName))
                LoadData(chooseFileName);
            editorWindow.Repaint();
        }

        public void OnDestroy()
        {
            PlayerPrefs.SetInt(FontPlayerPrefKey, nowButtonFontSize);
            PlayerPrefs.Save();
        }

        public string OnGUI(string _chooseFileName)
        {
            this.chooseFileName = _chooseFileName;
            if (!string.IsNullOrEmpty(resetChooseFileName))
            {
                chooseFileName = resetChooseFileName;
                resetChooseFileName = "";
            }
            if (buttonStyle == null || helpBoxStyle == null)
            {
                buttonStyle = "Button";
                helpBoxStyle = "HelpBox";
            }
            EditorDrawGUIUtil.RichTextSupport = true;
            GUILayout.Space(8);
            ChooseFile();
            GUILayout.Space(9);
            if (!string.IsNullOrEmpty(chooseFileName))
            {
                GridTopFunctionGUI();
                GUILayout.Space(5);
                DrawTableDataGUI();
            }
            return chooseFileName;
        }

        void ChooseFile()
        {
            GUILayout.BeginHorizontal();
            chooseFileName = EditorDrawGUIUtil.DrawPopup("选择文件", chooseFileName, configFileNames, LoadData);
            if (m_currentData == null)
            {
                LoadData(chooseFileName);
            }
            if (!string.IsNullOrEmpty(chooseFileName) && GUILayout.Button("删除", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("警告", "是否删除文件[" + chooseFileName + "]", "确定", "取消"))
                {
                    File.Delete(Application.dataPath + "/" + DataManager.directoryName + "/" + chooseFileName + ".txt");
                    string classPath = Application.dataPath + "/FKGame/Resources/Script/DataClassGenerate/" + chooseFileName + "Generate.cs";
                    if (File.Exists(classPath))
                    {
                        File.Delete(classPath);
                    }
                    AssetDatabase.Refresh();
                    m_currentData = null;
                    GlobalEvent.DispatchEvent(EditorEvent.LanguageDataEditorChange);
                    // Init(null);
                    return;
                }
            }
            if (GUILayout.Button("新建", GUILayout.Width(60)))
            {
                GeneralDataModificationWindow.otherParameter = "";
                GeneralDataModificationWindow.OpenWindow(editorWindow, "新建配置文件", "", (value) =>
                {
                    value = EditorDrawGUIUtil.DrawBaseValue("新建配置文件:", value);
                    GeneralDataModificationWindow.otherParameter = EditorDrawGUIUtil.DrawBaseValue("主键名:", GeneralDataModificationWindow.otherParameter);
                    string fileName = value.ToString();
                    string key = GeneralDataModificationWindow.otherParameter.ToString();
                    if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(key))
                    {
                        EditorGUILayout.HelpBox("文件名或主键名不能为空！！", MessageType.Error);
                    }
                    else if ((key.Contains(" ")) || (fileName.Contains(" ")))
                    {
                        EditorGUILayout.HelpBox("文件名或主键名有空格！！", MessageType.Error);
                    }
                    else if (configFileNames.Contains(fileName))
                    {
                        EditorGUILayout.HelpBox("文件名重复！！", MessageType.Error);
                    }
                    return value;
                },
                (value) =>
                {
                    string fileName = value.ToString();
                    string key = GeneralDataModificationWindow.otherParameter.ToString();
                    if (string.IsNullOrEmpty(fileName)
                        || string.IsNullOrEmpty(key)
                        || (key.Contains(" ")) || (fileName.Contains(" "))
                        || configFileNames.Contains(fileName))
                    {
                        return false;
                    }
                    else
                        return true;
                },
                (value) =>
                {
                    DataTable data = new DataTable();
                    string keyName = GeneralDataModificationWindow.otherParameter.ToString();
                    chooseFileName = value.ToString();
                    data.tableKeyDict.Add(keyName);
                    resetChooseFileName = chooseFileName;
                    SaveData(chooseFileName, data);
                    GlobalEvent.DispatchEvent(EditorEvent.LanguageDataEditorChange);
                    LoadData(chooseFileName);
                    AssetDatabase.Refresh();
                });
            }
            if (!string.IsNullOrEmpty(chooseFileName) && GUILayout.Button("加载上一次保存", GUILayout.Width(90)))
            {
                DataManager.CleanCache();
                LoadData(chooseFileName);
            }
            GUILayout.EndHorizontal();
        }

        private void LoadData(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            chooseFileName = fileName;
            m_currentData = DataManager.GetData(fileName);
            int widthCount = m_currentData.tableKeyDict.Count;
            if (widthCount > withItemList.Count)
            {
                int num = widthCount - withItemList.Count;
                for (int i = 0; i < num; i++)
                {
                    withItemList.Add(wWith);
                }
            }
            else if (widthCount < withItemList.Count)
            {
                while (widthCount < withItemList.Count)
                {
                    withItemList.RemoveAt(withItemList.Count - 1);
                }
            }
            heightItemList.Clear();
            // 所有的数据行+描述+默认值+空一行
            heightItemList.Add(wHeight);
            for (int i = 0; i < m_currentData.Count + 2; i++)
            {
                heightItemList.Add(wHeight);
            }
            for (int i = 0; i < selectRowIndexs.Count; i++)
            {
                if (selectRowIndexs[i] >= heightItemList.Count)
                {
                    selectRowIndexs.RemoveAt(i);
                    i = i - 1;
                }
            }
            for (int i = 0; i < selectColumnIndexs.Count; i++)
            {
                if (selectColumnIndexs[i] >= withItemList.Count)
                {
                    selectColumnIndexs.RemoveAt(i);
                    i = i - 1;
                }
            }
        }

        // 绘制所有格子
        void DrawTableDataGUI()
        {
            Rect position = editorWindow.position;
            Rect r = new Rect(0, 90, position.width, 18);
            float detaH = wHeight;
            if (heightItemList.Count >= 3)
            {
                detaH = heightItemList[0] + heightItemList[1] + heightItemList[2];
            }
            Rect svPos_temp = new Rect(new Vector2(0, r.y), new Vector2(position.width - wWith, position.height - r.y - detaH));
            Vector2 v = GetCententSize();
            svPos = GUI.BeginScrollView(new Rect(svPos_temp.x + wWith, svPos_temp.y + detaH, svPos_temp.width, svPos_temp.height), svPos, 
                new Rect(r.x + wWith, r.y + detaH, v.x, v.y));
            // 数据中两排后的数据
            DrawGridItem(r.position + new Vector2(wWith, wHeight), 2, heightItemList.Count);
            GUI.EndScrollView();
            Debug.unityLogger.logEnabled = false;
            // 顶部格子
            topGridSVPos = GUI.BeginScrollView(new Rect(svPos_temp.x, svPos_temp.y, svPos_temp.width + wWith, detaH), topGridSVPos, 
                new Rect(r.x, r.y, v.x + wWith, detaH), false, false, "Label", "Label");
            topGridSVPos = new Vector2(svPos.x, topGridSVPos.y);
            Debug.unityLogger.logEnabled = true;
            // 数据中的前两排格子
            DrawGridItem(r.position + new Vector2(wWith, wHeight), 0, 2);
            float tempWith = wWith;
            GUI.Label(new Rect(r.position, new Vector2(wWith, wHeight)), "", "SelectionRect");
            for (int i = 0; i < withItemList.Count; i++)
            {
                Rect dragR = new Rect(tempWith, r.y, withItemList[i], wHeight);
                Rect maxR = new Rect(tempWith, r.y, Screen.width * 5, wHeight);
                dragR = EditorDrawGUIUtility.DrawCanDragArea(dragR, maxR, null, EditorCanDragAreaSide.Right);
                if (dragR.width < minItem)
                    dragR.width = minItem;
                withItemList[i] = dragR.width;
                tempWith += withItemList[i];
                if (GUI.Button(new Rect(dragR.position, new Vector2(dragR.width, dragR.height / 3)), "▼"))
                {
                    if (selectColumnIndexs.Contains(i))
                        selectColumnIndexs.Remove(i);
                    else
                        selectColumnIndexs.Add(i);
                }
                if (GUI.Button(new Rect(dragR.x, dragR.y + dragR.height / 3, dragR.width, dragR.height * 2 / 3), i.ToString()))
                {
                    if (i == 0)
                        continue;
                    List<string> keys = m_currentData.tableKeyDict;
                    string key = keys[i];
                    if (EditorUtility.DisplayDialog("警告", "是否删除字段[" + key + "]", "确定", "取消"))
                    {
                        DeleteField(m_currentData, key);
                        withItemList.RemoveAt(i);
                        return;
                    }
                }
            }
            GUI.EndScrollView();

            // 左边格子
            float tempHeight = r.y + wHeight;
            for (int i = 0; i < 2; i++)
            {
                Rect dragR = new Rect(r.x, tempHeight, wWith, heightItemList[i]);
                Rect maxR = new Rect(r.x, tempHeight, wWith, position.height);
                dragR = EditorDrawGUIUtility.DrawCanDragArea(dragR, maxR, null, EditorCanDragAreaSide.Bottom);
                if (dragR.height < minItem)
                    dragR.height = minItem;
                heightItemList[i] = dragR.height;
                tempHeight += heightItemList[i];
                string vStr = "";
                if (i == 0)
                {
                    vStr = "D";
                    GUI.Label(dragR, vStr, buttonStyle);
                }
                else if (i == 1)
                {
                    vStr = "description";
                    GUI.Label(dragR, vStr, buttonStyle);
                }
            }
            // 默认第一列
            Debug.unityLogger.logEnabled = false;
            leftGridSVPos = GUI.BeginScrollView(new Rect(svPos_temp.x, tempHeight, wWith, svPos_temp.height), leftGridSVPos, new Rect(r.x, tempHeight, wWith, v.y), false, false, "Label", "Label");
            leftGridSVPos = new Vector2(leftGridSVPos.x, svPos.y);
            Debug.unityLogger.logEnabled = true;
            for (int i = 2; i < heightItemList.Count; i++)
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (showSearchValueIndexList != null && !showSearchValueIndexList.Contains(i))
                        continue;
                }
                Rect dragR = new Rect(r.x, tempHeight, wWith, heightItemList[i]);
                Rect maxR = new Rect(r.x, tempHeight, wWith, position.height);
                dragR = EditorDrawGUIUtility.DrawCanDragArea(dragR, maxR, null, EditorCanDragAreaSide.Bottom);
                if (dragR.height < minItem)
                    dragR.height = minItem;
                heightItemList[i] = dragR.height;
                tempHeight += heightItemList[i];
                string vStr = "";
                if (selectRowIndexs.Contains(i))
                    GUI.color = Color.cyan;
                if (i == 2)
                {
                    vStr = "default";
                    GUI.Label(dragR, vStr, buttonStyle);
                }
                else
                {
                    int num = i - 3;
                    vStr = num.ToString();
                    float firstLenth = dragR.width / 4;
                    float threeQHeight = dragR.height / 3;
                    if (GUI.Button(new Rect(dragR.position, new Vector2(firstLenth, dragR.height)), "?"))
                    {
                        if (selectRowIndexs.Contains(i))
                            selectRowIndexs.Remove(i);
                        else
                            selectRowIndexs.Add(i);
                    }
                    if (GUI.Button(new Rect(dragR.x + firstLenth, dragR.y, firstLenth, threeQHeight), "▲"))
                    {
                        if (num > 0)
                            m_currentData.tableIDDict.Reverse(num - 1, 2);
                    }
                    if (GUI.Button(new Rect(dragR.x + firstLenth, dragR.y + threeQHeight, firstLenth, threeQHeight), "?"))
                    {
                        GeneralDataModificationWindow.OpenWindow(editorWindow, "移动一行数据", num, (value) =>
                        {
                            value = EditorDrawGUIUtil.DrawBaseValue("Index:", value);
                            GUILayout.Label("Current Index:" + num);
                            value = Mathf.Clamp((int)value, 0, m_currentData.Count);
                            return value;
                        },
                        null,
                (value) =>
                {
                    int index = (int)value;
                    string key = m_currentData.tableIDDict[num];
                    m_currentData.tableIDDict.RemoveAt(num);
                    m_currentData.tableIDDict.Insert(index, key);
                });
                        return;
                    }
                    if (GUI.Button(new Rect(dragR.x + firstLenth, dragR.y + threeQHeight * 2, firstLenth, threeQHeight), "▼"))
                    {
                        if (num < m_currentData.tableIDDict.Count - 1)
                            m_currentData.tableIDDict.Reverse(num, 2);
                    }
                    if (GUI.Button(new Rect(dragR.x + firstLenth * 2, dragR.y, dragR.width - firstLenth * 2, dragR.height), vStr))
                    {
                        if (EditorUtility.DisplayDialog("警告", "是否删除第[" + num + "]行数据", "确定", "取消"))
                        {
                            m_currentData.Remove(m_currentData.tableIDDict[num]);
                            m_currentData.tableIDDict.RemoveAt(num);
                            heightItemList.RemoveAt(i);
                            return;
                        }
                    }
                }
                GUI.color = Color.white;
            }
            GUI.EndScrollView();
        }

        private Vector2 GetCententSize()
        {
            float tempWith = 0;
            for (int i = 0; i < withItemList.Count; i++)
            {
                tempWith += withItemList[i];
            }
            tempWith += wWith;
            float tempHeight = 0;
            for (int i = 2; i < heightItemList.Count; i++)
            {

                tempHeight += heightItemList[i];
            }
            tempHeight += wHeight;
            return new Vector2(tempWith, tempHeight);
        }

        private void GridTopFunctionGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("添加一行数据", GUILayout.Width(90)))
            {
                AddLineDataGUI();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加字段", GUILayout.Width(90)))
            {
                Add2FieldGUI();
            }
            GUILayout.FlexibleSpace();
            GUILayout.Space(5);
            if (GUILayout.Button("转换字段为多语言", GUILayout.Width(120)))
            {
                ChangeField2Language();
                AssetDatabase.Refresh();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("生成Data类", GUILayout.Width(90)))
            {
                DataConfigUtils.CreatDataCSharpFile(chooseFileName, m_currentData);
                AssetDatabase.Refresh();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("生成全部Data类", GUILayout.Width(120)))
            {
                DataConfigUtils.CreatAllClass(configFileNames);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("清除缓存", GUILayout.Width(90)))
            {
                LanguageManager.IsInit = false;
                DataManager.CleanCache();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("保存", GUILayout.Width(140)))
            {
                SaveData(chooseFileName, m_currentData);
                AssetDatabase.Refresh();
                editorWindow.ShowNotification(new GUIContent("Save Success!"));
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(7);
            GUILayout.BeginHorizontal();

            // 字体大小调节
            helpBoxStyle.fontSize = 11;
            oldButtonFontSize = helpBoxStyle.fontSize;
            if (nowButtonFontSize <= 0)
            {
                nowButtonFontSize = PlayerPrefs.GetInt(FontPlayerPrefKey, oldButtonFontSize);
            }
            nowButtonFontSize = EditorGUILayout.IntSlider("字体大小", nowButtonFontSize, oldButtonFontSize / 2, MaxButtonFontSize);
            GUILayout.FlexibleSpace();
            searchValue = EditorDrawGUIUtil.DrawSearchField(searchValue, () =>
            {
                saveSearchValueIndexList.Clear();
                showSearchValueIndexList = null;
            });
            GUILayout.EndHorizontal();
        }

        private void ChangeField2Language()
        {
            List<string> strTypes = new List<string>();
            foreach (var item in m_currentData.tableKeyDict)
            {
                if (!m_currentData.tableTypesDict.ContainsKey(item) || m_currentData.tableTypesDict[item] == FieldType.String)
                    strTypes.Add(item);
            }
            GeneralDataModificationWindow.otherParameter = "";
            GeneralDataModificationWindow.OpenWindow(editorWindow, "将字段数据转换为多语言数据", "", (value) =>
            {
                value = EditorDrawGUIUtil.DrawPopup("字段:", value.ToString(), strTypes);
                GeneralDataModificationWindow.otherParameter = EditorDrawGUIUtil.DrawBaseValue("多语言文件名:", GeneralDataModificationWindow.otherParameter);
                string key = value.ToString();
                if (string.IsNullOrEmpty(key))
                    EditorGUILayout.HelpBox("Key不能为空！！", MessageType.Error);
                else if (key.Contains(" "))
                    EditorGUILayout.HelpBox("Key 不能有空格！！", MessageType.Error);
                else if (!m_currentData.tableKeyDict.Contains(key.Trim()))
                    EditorGUILayout.HelpBox("不包含的字段！！", MessageType.Error);
                return value;
            },
             (value) =>
             {
                 string key = value.ToString();
                 if (string.IsNullOrEmpty(key) || (key.Contains(" ")) || !m_currentData.tableKeyDict.Contains(key.Trim()))
                 {
                     return false;
                 }
                 return true;
             },
           (value) =>
           {
               string l_fileName = GeneralDataModificationWindow.otherParameter.ToString();
               string field = value.ToString();
               m_currentData.fieldAssetTypeDict[field] = DataFieldAssetType.LocalizedLanguage;
               m_currentData.defaultValueDict[field] = "null";
               Dictionary<string, string> oldContentDic = new Dictionary<string, string>();
               foreach (var item in m_currentData)
               {
                   string v = item.Value[field];
                   Debug.Log("item.Key :" + item.Key + "  v :" + v);
                   oldContentDic.Add(item.Key, v);
               }
               LanguageDataEditorWindow lWin = LanguageDataEditorWindow.ShowWindow();
               Dictionary<string, string> newDataDic = lWin.CreateNewFile(l_fileName, oldContentDic);
               foreach (var item in newDataDic)
               {
                   m_currentData[item.Key][field] = item.Value;
               }
               SaveData(chooseFileName, m_currentData);
           });
        }

        // 绘制每个数据格子
        private void DrawGridItem(Vector2 startPos, int heightStartIndex, int heightEndIndex)
        {
            helpBoxStyle.fontSize = nowButtonFontSize;
            float tempHeight = 0;
            for (int i = 0; i < heightStartIndex; i++)
            {
                tempHeight += heightItemList[i];
            }
            for (int i = heightStartIndex; i < heightEndIndex; i++)
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    if (showSearchValueIndexList != null && !showSearchValueIndexList.Contains(i))
                        continue;
                }
                float h = heightItemList[i];
                float tempWith = 0;
                for (int j = 0; j < withItemList.Count; j++)
                {
                    float w = withItemList[j];
                    Vector2 size = new Vector2(w, h);
                    Vector2 pos = startPos + new Vector2(tempWith, tempHeight);
                    object value = null;
                    string showStr = "";
                    string field = m_currentData.tableKeyDict[j];
                    FieldType fieldValueType = GetFieldType(j, field);
                    string enumType = GetEnumType(fieldValueType, field);
                    DataFieldAssetType fieldAssetType = GetDataFieldAssetType(field);
                    string defaultValue = GetDefaultValue(fieldValueType, enumType, field);
                    List<char> m_ArraySplitFormat = GetArraySplitFormat(field);

                    if (i == 0)
                    {
                        GUI.color = Color.yellow;
                        showStr = EditorDrawGUIUtil.GetFormatName(field, DataConfigUtils.ConfigFieldValueType2Type(fieldValueType, enumType, m_ArraySplitFormat), "red");
                    }
                    else if (i == 1)
                    {
                        GUI.color = Color.cyan;
                        showStr = GetDescription(field);
                    }
                    else if (i == 2)
                    {
                        GUI.color = Color.green;
                        showStr = defaultValue;
                    }
                    else
                    {
                        SingleData data = null;
                        try
                        {
                            data = m_currentData[m_currentData.tableIDDict[i - 3]];
                        }
                        catch (Exception e)
                        {
                            for (int m = 0; m < m_currentData.tableIDDict.Count; m++)
                            {
                                Debug.Log(" index :" + i + " m:" + m + " item:" + m_currentData.tableIDDict[m]);
                            }
                            Debug.LogError("error:" + e);
                            continue;
                        }
                        bool isDefault = false;
                        if (data.ContainsKey(field))
                        {
                            if (data[field] != defaultValue)
                            {
                                showStr = data[field];
                            }
                            else
                            {
                                showStr = defaultValue;
                                isDefault = true;
                            }
                        }
                        else
                        {
                            showStr = defaultValue;
                            isDefault = true;
                        }
                        if (fieldAssetType == DataFieldAssetType.LocalizedLanguage)
                        {
                            string k = showStr;
                            if (k != "null" && langKeys.Contains(k))
                            {
                                showStr = LanguageManager.GetContentByKey(k);
                            }
                        }
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            bool isContainsSearchKeyWards = false;
                            showStr = ShowContainsChar(showStr, searchValue, ref isContainsSearchKeyWards);
                            if (isContainsSearchKeyWards)
                            {
                                if (!saveSearchValueIndexList.Contains(i))
                                    saveSearchValueIndexList.Add(i);
                            }
                        }
                        if (isDefault)
                        {
                            showStr = "<color=green>" + showStr + "</color>";
                        }
                    }
                    Rect rect = new Rect(pos, size);
                    if (i == 1 || i == 2)
                    {
                        GUI.Button(rect, showStr, helpBoxStyle);
                    }
                    else
                    {
                        if (selectColumnIndexs.Contains(j))
                            GUI.color = Color.magenta;
                        if (selectRowIndexs.Contains(i))
                            GUI.color = Color.cyan;
                        object modofyValue = null;
                        if (i > 0)
                        {
                            SingleData data = null;
                            try
                            {
                                data = m_currentData[m_currentData.tableIDDict[i - 3]];
                            }
                            catch (Exception e)
                            {
                                for (int m = 0; m < m_currentData.tableIDDict.Count; m++)
                                {
                                    Debug.Log(" index :" + i + " m:" + m + " item:" + m_currentData.tableIDDict[m]);
                                }
                                Debug.LogError("error:" + e);
                                continue;
                            }
                            if (data.ContainsKey(field))
                                defaultValue = data[field];
                            try
                            {
                                value = DataConfigUtils.TableString2ObjectValue(defaultValue, fieldValueType, enumType, m_ArraySplitFormat);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                                GUI.color = Color.red;
                            }
                            if (fieldAssetType == DataFieldAssetType.Data && !fieldValueType.ToString().Contains("Array"))
                            {
                                modofyValue = DrawValueEitorGUI.DrawValue(rect, value, helpBoxStyle);
                            }
                        }
                        if (modofyValue == null && GUI.Button(rect, showStr, helpBoxStyle))
                        {
                            modifiIndex = new Vector2Int(i - 2, j);
                            if (i == 0)
                            {
                                TableConfigFieldInfo f = new TableConfigFieldInfo();
                                f.fieldName = field;
                                f.description = m_currentData.commentValueDict.ContainsKey(field) ? m_currentData.commentValueDict[field] : "";
                                f.fieldValueType = fieldValueType;
                                f.defultValue = DataConfigUtils.TableString2ObjectValue(defaultValue, fieldValueType, enumType, m_ArraySplitFormat);
                                f.enumType = enumType;
                                f.fieldAssetType = GetDataFieldAssetType(field);
                                f.m_ArraySplitFormat = GetArraySplitFormat(field);
                                value = f;
                            }
                            else
                            {
                                if (value == null)
                                    value = DataConfigUtils.TableString2ObjectValue("", fieldValueType, enumType, m_ArraySplitFormat);
                            }
                            GeneralDataModificationWindow.OpenWindow(editorWindow, "修改数据", value, DrawModifiValueGUI, CheckModifiValueCallBack, ModificationCompleteCallBack);
                        }
                        if (modofyValue != null)
                        {
                            int index = i - 3;
                            string valueStr = DataConfigUtils.ObjectValue2TableString(modofyValue, GetArraySplitFormat(field));
                            SingleData data = m_currentData[m_currentData.tableIDDict[index]];
                            if (j == 0)
                            {
                                if (m_currentData.tableIDDict[index] != valueStr)
                                {
                                    bool repeat = false;
                                    bool isNull = false;
                                    for (int m = 0; m < m_currentData.tableIDDict.Count; m++)
                                    {
                                        if (m != index && m_currentData.tableIDDict[m] == valueStr)
                                        {
                                            repeat = true;
                                            break;
                                        }
                                    }
                                    if (string.IsNullOrEmpty(valueStr) || "null".Equals(valueStr.ToLower()))
                                    {
                                        isNull = true;
                                    }
                                    if (repeat || isNull)
                                    {
                                        if (repeat)
                                        {
                                            for (int m = 0; m < m_currentData.tableIDDict.Count; m++)
                                            {
                                                Debug.Log(" index :" + index + " m:" + m + " item:" + m_currentData.tableIDDict[m]);
                                            }
                                            Debug.LogError("重复的Key:" + valueStr);
                                        }
                                        if (isNull)
                                        {
                                            Debug.LogError("Key 不能为null或空");
                                        }
                                    }
                                    else
                                    {
                                        m_currentData.tableIDDict[index] = valueStr;
                                        data[field] = valueStr;
                                        List<string> keys = new List<string>(m_currentData.Keys);
                                        List<SingleData> values = new List<SingleData>(m_currentData.Values);
                                        keys[index] = valueStr;
                                        m_currentData.Clear();
                                        for (int n = 0; n < keys.Count; n++)
                                        {
                                            m_currentData.Add(keys[n], values[n]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                data[field] = valueStr;
                            }
                        }
                    }
                    GUI.color = Color.white;
                    tempWith += w;
                }
                tempHeight += h;
            }

            helpBoxStyle.fontSize = oldButtonFontSize;
            if (!string.IsNullOrEmpty(searchValue))
            {
                showSearchValueIndexList = saveSearchValueIndexList;
                showSearchValueIndexList.Add(0);
                showSearchValueIndexList.Add(1);
                showSearchValueIndexList.Add(2);
            }
        }

        private string ShowContainsChar(string value, string searchValue, ref bool isContainsSearchKeyWards)
        {
            string res = value;
            isContainsSearchKeyWards = false;
            if (value.Contains(searchValue))
            {
                res = value.Replace(searchValue, "<color=red>" + searchValue + "</color>");
                isContainsSearchKeyWards = true;
            }
            return res;
        }

        private DataFieldAssetType GetDataFieldAssetType(string field)
        {
            DataFieldAssetType type;
            if (!m_currentData.fieldAssetTypeDict.TryGetValue(field, out type))
            {
                type = DataFieldAssetType.Data;
            }
            return type;
        }

        private FieldType GetFieldType(int index, string field)
        {
            try
            {
                FieldType fieldValueType = index == 0 ? FieldType.String : m_currentData.tableTypesDict[field];
                return fieldValueType;
            }
            catch (Exception e)
            {
                Debug.Log("field " + field + " exception " + e.ToString());
                return FieldType.String;
            }
        }

        public List<char> GetArraySplitFormat(string field)
        {
            char[] chars = null;
            if (m_currentData.arraySplitFormatDict.TryGetValue(field, out chars))
            {
                return new List<char>(chars);
            }
            return new List<char>();
        }

        private string GetEnumType(FieldType fieldValueType, string field)
        {
            string enumType = fieldValueType == FieldType.Enum ?
                       (m_currentData.tableEnumTypeDict.ContainsKey(field) ? m_currentData.tableEnumTypeDict[field] : EditorTool.GetAllEnumType()[0])
                       : "";
            return enumType;
        }

        private string GetDefaultValue(FieldType fieldValueType, string enumType, string field)
        {
            string defaultValue;
            if (!m_currentData.defaultValueDict.TryGetValue(field, out defaultValue))
            {
                Type t = DataConfigUtils.ConfigFieldValueType2Type(fieldValueType, enumType, GetArraySplitFormat(field));
                object obj = ReflectionUtils.CreateDefultInstance(t);
                defaultValue = DataConfigUtils.ObjectValue2TableString(obj, GetArraySplitFormat(field));
                m_currentData.defaultValueDict.Add(field, defaultValue);
            }
            return defaultValue;
        }

        private string GetDescription(string field)
        {
            string value;
            if (!m_currentData.commentValueDict.TryGetValue(field, out value))
            {
                value = "";
                m_currentData.commentValueDict.Add(field, value);
            }
            return value;
        }

        // 绘制数据修改窗口
        private object DrawModifiValueGUI(object t)
        {
            if (t is TableConfigFieldInfo)
            {
                TableConfigFieldInfo info = (TableConfigFieldInfo)t;
                DrawTableConfigFieldInfo(info);
                string df = DataConfigUtils.ObjectValue2TableString(info.defultValue, info.m_ArraySplitFormat);
                if (info.fieldValueType == FieldType.String && string.IsNullOrEmpty(df))
                    EditorGUILayout.HelpBox("默认值不能为空！！=>", MessageType.Error);
                if (CheckIsNameRepeat(info))
                    EditorGUILayout.HelpBox("字段名不能重复！！", MessageType.Error);
            }
            else
            {
                string field = m_currentData.tableKeyDict[modifiIndex.y];
                SingleData data = m_currentData[m_currentData.tableIDDict[modifiIndex.x - 1]];
                EditorDrawGUIUtil.CanEdit = false;
                field = EditorDrawGUIUtil.DrawBaseValue("字段名", field).ToString();
                EditorDrawGUIUtil.CanEdit = true;
                GUILayout.Space(7);
                string description = GetDescription(field);
                GUILayout.Label("字段描述：" + description);
                FieldType fieldValueType = GetFieldType(modifiIndex.y, field);
                string enumType = GetEnumType(fieldValueType, field);
                string defaultValue = GetDefaultValue(fieldValueType, enumType, field);
                DataFieldAssetType fieldAssetType = GetDataFieldAssetType(field);
                GUILayout.BeginHorizontal();
                GUILayout.Label("默认值 :" + defaultValue);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("默认值", GUILayout.Width(60)))
                {
                    List<char> m_ArraySplitFormat = GetArraySplitFormat(field);
                    t = DataConfigUtils.TableString2ObjectValue(defaultValue, fieldValueType, enumType, m_ArraySplitFormat);
                }
                GUILayout.EndHorizontal();
                string text = "值";
                if (fieldAssetType == DataFieldAssetType.LocalizedLanguage)
                {
                    t = DrawLocalizedLanguageField(text, t);
                }
                else if (fieldAssetType == DataFieldAssetType.Prefab)
                {
                    t = DrawPrefabGUI(text, t);
                }
                else if (fieldAssetType == DataFieldAssetType.Texture)
                {
                    t = DrawTextureGUI(text, t);
                }
                else if (fieldAssetType == DataFieldAssetType.TableKey)
                {
                    t = DrawTableGUI(text, t);
                }
                else
                {
                    t = EditorDrawGUIUtil.DrawBaseValue(text, t);
                    if (fieldValueType == FieldType.Enum)
                    {
                        string v = EditorDrawGUIUtil.DrawBaseValue(text, t.ToString()).ToString();
                        if (v != t.ToString())
                        {
                            List<string> list = new List<string>(Enum.GetNames(t.GetType()));
                            if (list.Contains(v))
                            {
                                t = Enum.Parse(t.GetType(), v);
                            }
                        }
                    }
                }
            }
            return t;
        }

        private bool CheckModifiValueCallBack(object t)
        {
            if (t is TableConfigFieldInfo)
            {
                TableConfigFieldInfo info = (TableConfigFieldInfo)t;
                if (string.IsNullOrEmpty(info.fieldName))
                    return false;
                if (CheckIsNameRepeat(info))
                    return false;
                string df = DataConfigUtils.ObjectValue2TableString(info.defultValue, info.m_ArraySplitFormat);
                if (info.fieldValueType == FieldType.String && string.IsNullOrEmpty(df))
                    return false;
            }
            return true;
        }

        private bool CheckIsNameRepeat(TableConfigFieldInfo info)
        {
            string field = m_currentData.tableKeyDict[modifiIndex.y];
            for (int i = 0; i < m_currentData.tableKeyDict.Count; i++)
            {
                string f = m_currentData.tableKeyDict[i];
                if (i == modifiIndex.y)
                    continue;
                else
                {
                    if (f == info.fieldName)
                        return true;
                }
            }
            return false;
        }

        // 绘制字段信息
        private void DrawTableConfigFieldInfo(TableConfigFieldInfo temp)
        {
            Type type = DataConfigUtils.ConfigFieldValueType2Type(temp.fieldValueType, temp.enumType, temp.m_ArraySplitFormat);
            if (type != null && (temp.defultValue == null || type != temp.defultValue.GetType()))
            {
                Debug.Log("create Default type:" + type);
                temp.defultValue = ReflectionUtils.CreateDefultInstance(type);
            }
            GUILayout.BeginVertical("box");
            GUILayout.EndVertical();
            temp = (TableConfigFieldInfo)EditorDrawGUIUtil.DrawClassData("字段信息", temp, new List<string>() { "defultValue", "enumType" }, null, () =>
            {
                if (temp.fieldAssetType == DataFieldAssetType.LocalizedLanguage
                || temp.fieldAssetType == DataFieldAssetType.Prefab
                || temp.fieldAssetType == DataFieldAssetType.TableKey
                || temp.fieldAssetType == DataFieldAssetType.Texture)
                {
                    if (type != null && type != typeof(string))
                    {
                        temp.fieldAssetType = DataFieldAssetType.Data;
                    }
                }
                if (temp.fieldValueType == FieldType.Enum)
                {
                    temp.enumType = EditorDrawGUIUtil.DrawBaseValue("枚举类型", temp.enumType).ToString();
                }
                else if (temp.fieldValueType.ToString().Contains("Array"))
                {
                    temp.m_ArraySplitFormat = DrawArrayFormat(temp.m_ArraySplitFormat);
                }
                string text = "默认值";
                if (temp.fieldAssetType == DataFieldAssetType.LocalizedLanguage)
                {
                    temp.defultValue = DrawLocalizedLanguageField(text, temp.defultValue);
                }
                else if (temp.fieldAssetType == DataFieldAssetType.Prefab)
                {
                    temp.defultValue = DrawPrefabGUI(text, temp.defultValue);
                }
                else if (temp.fieldAssetType == DataFieldAssetType.Texture)
                {
                    temp.defultValue = DrawTextureGUI(text, temp.defultValue);
                }
                else if (temp.fieldAssetType == DataFieldAssetType.TableKey)
                {
                    temp.defultValue = DrawTableGUI(text, temp.defultValue);
                }
                else
                {
                    temp.defultValue = EditorDrawGUIUtil.DrawBaseValue(text, temp.defultValue);
                }
            });
        }

        private int GetNumSubString(string value, string subStr)
        {
            int fromIndex = 0;
            int count = 0;
            while (true)
            {
                int index = value.IndexOf(subStr, fromIndex);
                if (-1 != index)
                {
                    fromIndex = index + 1;
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        private List<char> DrawArrayFormat(List<char> formatChrars)
        {
            string ssArr = new string(formatChrars.ToArray());
            ssArr = EditorGUILayout.TextField("数组分割字段：", ssArr);
            return new List<char>(ssArr.ToCharArray());
        }

        private object DrawTableGUI(string text, object defultValue)
        {
            defultValue = EditorDrawGUIUtil.DrawBaseValue(text, defultValue);
            string value = defultValue.ToString();
            if ("null".Equals(value.ToLower()))
            {
                return value;
            }
            GUILayout.Label(text + " : " + value);
            if (!string.IsNullOrEmpty(value))
            {
                string[] tempStrs = value.Split('/');
                if (tempStrs.Length == 3)
                {
                    tableName = tempStrs[0];
                    fieldName = tempStrs[1];
                    tableKey = tempStrs[2];
                }
            }
            tableName = EditorDrawGUIUtil.DrawPopup("表格名", tableName, configFileNames, (tName) =>
            {
                if (string.IsNullOrEmpty(tName))
                    return;
                fieldNames.Clear();
                DataTable d = DataManager.GetData(tName);
                fieldNames.AddRange(d.tableKeyDict);
                tableKeys.Clear();
                tableKeys.AddRange(d.tableIDDict);
            });
            tableKey = EditorDrawGUIUtil.DrawPopup("表格数据key", tableKey, tableKeys);
            fieldName = EditorDrawGUIUtil.DrawPopup("字段名", fieldName, fieldNames);
            defultValue = tableName + "/" + fieldName + "/" + tableKey;
            return defultValue;
        }

        private object DrawLocalizedLanguageField(string text, object value)
        {
            value = EditorDrawGUIUtil.DrawBaseValue(text, value);
            if ("null" != value.ToString())
            {
                value = EditorDrawGUIUtil.DrawPopup(text, value.ToString(), langKeys);
                if (value == null)
                    value = "null";
                GUILayout.Space(6);
                GUILayout.Label("多语言字段[" + value + "] : " + LanguageManager.GetContentByKey(value.ToString()));
                GUILayout.Space(8);
                if (GUILayout.Button("打开编辑当前多语言字段"))
                {
                    LanguageDataEditorWindow.OpenAndSearchValue(value.ToString());
                }
            }
            else
            {
                GUILayout.Space(6);
                GUILayout.Label("多语言字段为非null时支持多语言选项");
                GUILayout.Space(8);
            }
            return value;
        }

        private object DrawPrefabGUI(string text, object value)
        {
            if (editPrefabValue != null)
            {
                value = editPrefabValue;
                editPrefabValue = null;
            }
            GUILayout.BeginHorizontal();
            value = EditorDrawGUIUtil.DrawBaseValue(text, value);
            if (GUILayout.Button("o", GUILayout.Width(20)))
            {
                ObjectSelectorWindow.Show(GeneralDataModificationWindow.GetInstance(), value.ToString(),
                    new string[] { "Assets/Resources" },
                    typeof(GameObject),
                    (assetName, obj) =>
                    {
                        editPrefabValue = assetName;
                    });
            }
            GUILayout.EndHorizontal();
            string content = value.ToString();
            bool isHave = false;
            try
            {
                if (ResourcesConfigManager.IsResourceExist(content))
                {
                    GameObject obj = ResourceManager.Load<GameObject>(content);
                    if (obj)
                    {
                        if (obj != previewObj)
                        {
                            previewEditor = Editor.CreateEditor(obj);
                            previewObj = obj;
                        }
                        previewEditor.OnPreviewGUI(GUILayoutUtility.GetRect(300, 300), EditorStyles.helpBox);
                        isHave = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            if (!isHave)
            {
                EditorGUILayout.HelpBox("没有预制 [" + value + "] ！！", MessageType.Error);
            }
            return value;
        }

        private object DrawTextureGUI(string text, object value)
        {
            if (editTextureValue != null)
            {
                value = editTextureValue;
                editTextureValue = null;
            }
            GUILayout.BeginHorizontal();
            value = EditorDrawGUIUtil.DrawBaseValue(text, value);
            if (GUILayout.Button("o", GUILayout.Width(20)))
            {
                ObjectSelectorWindow.Show(GeneralDataModificationWindow.GetInstance(), value.ToString(),
                    new string[] { "Assets/Resources" },
                    typeof(Texture),
                    (assetName, obj) =>
                    {
                        editTextureValue = assetName;
                    });
            }
            GUILayout.EndHorizontal();
            string content = value.ToString();
            bool isHave = false;
            try
            {
                if (ResourcesConfigManager.IsResourceExist(content))
                {
                    Texture2D obj = ResourceManager.Load<Texture2D>(content);
                    if (obj)
                    {
                        if (obj != previewTex)
                        {
                            previewEditor1 = Editor.CreateEditor(obj);
                            previewTex = obj;
                        }
                        previewEditor1.OnPreviewGUI(GUILayoutUtility.GetRect(300, 300), EditorStyles.helpBox);
                        isHave = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            if (!isHave)
            {
                EditorGUILayout.HelpBox("没有图片 [" + value + "] ！！", MessageType.Error);
            }
            return value;
        }

        // 修改格子数据完成回调
        private void ModificationCompleteCallBack(object t)
        {
            if (t is TableConfigFieldInfo)
            {
                TableConfigFieldInfo temp = (TableConfigFieldInfo)t;
                string field = m_currentData.tableKeyDict[modifiIndex.y];
                if (field != temp.fieldName)
                {
                    RenameField(m_currentData, field, temp.fieldName);
                    field = temp.fieldName;
                }
                if (m_currentData.commentValueDict.ContainsKey(field))
                    m_currentData.commentValueDict[field] = temp.description;
                else
                {
                    if (!string.IsNullOrEmpty(temp.description))
                    {
                        m_currentData.commentValueDict.Add(field, temp.description);
                    }
                }
                if (m_currentData.tableTypesDict[field] != temp.fieldValueType)
                {
                    m_currentData.tableTypesDict[field] = temp.fieldValueType;
                    ResetDataField(m_currentData, field, temp.fieldValueType, temp.enumType, temp.defultValue);
                }
                if (temp.fieldValueType == FieldType.Enum)
                {
                    if (m_currentData.tableEnumTypeDict.ContainsKey(field))
                        m_currentData.tableEnumTypeDict[field] = temp.enumType;
                    else
                        m_currentData.tableEnumTypeDict.Add(field, temp.enumType);
                }
                if (m_currentData.arraySplitFormatDict.ContainsKey(field))
                {
                    m_currentData.arraySplitFormatDict[field] = temp.m_ArraySplitFormat.ToArray();
                }
                m_currentData.fieldAssetTypeDict[field] = temp.fieldAssetType;
                string oldDefaultValue = m_currentData.defaultValueDict[field];
                m_currentData.defaultValueDict[field] = DataConfigUtils.ObjectValue2TableString(temp.defultValue, GetArraySplitFormat(field));
                if (oldDefaultValue != m_currentData.defaultValueDict[field])
                {
                    string newDefultValue = m_currentData.defaultValueDict[field];
                    foreach (var id in m_currentData.tableIDDict)
                    {
                        SingleData data = m_currentData[id];
                        if (data.ContainsKey(field) && data[field] == oldDefaultValue)
                        {
                            m_currentData[id][field] = newDefultValue;
                        }
                    }
                }
            }
            else
            {
                string field = m_currentData.tableKeyDict[modifiIndex.y];
                SingleData data = m_currentData[m_currentData.tableIDDict[modifiIndex.x - 1]];
                data[field] = DataConfigUtils.ObjectValue2TableString(t, GetArraySplitFormat(field));
            }
        }

        // 添加一个字段
        private void Add2FieldGUI()
        {
            GeneralDataModificationWindow.OpenWindow(editorWindow, "添加字段", new TableConfigFieldInfo(), (value) =>
            {
                TableConfigFieldInfo info = (TableConfigFieldInfo)value;
                DrawTableConfigFieldInfo(info);

                if (string.IsNullOrEmpty(info.fieldName))
                    EditorGUILayout.HelpBox("字段名不能为空！！", MessageType.Error);
                else if (info.fieldName.Contains(" "))
                    EditorGUILayout.HelpBox("字段名不能有空格！！", MessageType.Error);
                else if (m_currentData.tableKeyDict.Contains(info.fieldName))
                    EditorGUILayout.HelpBox("字段名重复！！", MessageType.Error);
                string df = DataConfigUtils.ObjectValue2TableString(info.defultValue, info.m_ArraySplitFormat);
                if (info.fieldValueType == FieldType.String && string.IsNullOrEmpty(df))
                    EditorGUILayout.HelpBox("默认值不能为空！！", MessageType.Error);
                return value;
            },
            (value) =>
            {
                TableConfigFieldInfo info = (TableConfigFieldInfo)value;
                if (string.IsNullOrEmpty(info.fieldName) || m_currentData.tableKeyDict.Contains(info.fieldName) || info.fieldName.Contains(" "))
                    return false;
                string df = DataConfigUtils.ObjectValue2TableString(info.defultValue, GetArraySplitFormat(info.fieldName));
                if (info.fieldValueType == FieldType.String && string.IsNullOrEmpty(df))
                    return false;
                return true;
            },
            (value) =>
            {
                TableConfigFieldInfo info = (TableConfigFieldInfo)value;
                AddField(m_currentData, info);
                withItemList.Add(wWith);
            });
        }

        // 添加一行数据
        private void AddLineDataGUI()
        {
            GeneralDataModificationWindow.otherParameter = m_currentData.Count;
            GeneralDataModificationWindow.OpenWindow(editorWindow, "插入一行数据", "", (value) =>
            {
                value = EditorDrawGUIUtil.DrawBaseValue("Key:", value);
                GeneralDataModificationWindow.otherParameter = EditorDrawGUIUtil.DrawBaseValue("Insert Index:", GeneralDataModificationWindow.otherParameter);
                GeneralDataModificationWindow.otherParameter = Mathf.Clamp((int)GeneralDataModificationWindow.otherParameter, 0, m_currentData.Count);
                string key = value.ToString();
                if (string.IsNullOrEmpty(key))
                    EditorGUILayout.HelpBox("Key不能为空！！", MessageType.Error);
                else if (key.Contains(" "))
                    EditorGUILayout.HelpBox("Key 不能有空格！！", MessageType.Error);
                else if (m_currentData.tableIDDict.Contains(key.Trim()))
                    EditorGUILayout.HelpBox("Key重复！！", MessageType.Error);
                return value;
            },
             (value) =>
             {
                 string key = value.ToString();
                 if (string.IsNullOrEmpty(key) || (key.Contains(" ")) || m_currentData.tableIDDict.Contains(key.Trim()))
                     return false;
                 return true;
             },
           (value) =>
           {
               int index = (int)GeneralDataModificationWindow.otherParameter;
               string key = value.ToString();
               heightItemList.Add(30);
               SingleData data = new SingleData();
               DataTable table = m_currentData;
               List<string> keys = table.tableKeyDict;
               for (int i = 0; i < keys.Count; i++)
               {
                   string keyTmp = keys[i];
                   if (i == 0)
                   {
                       data.Add(keyTmp, key);
                       data.keyComment = key;
                   }
                   else
                       data.Add(keyTmp, table.defaultValueDict[keyTmp]);
               }
               m_currentData.Add(key, data);
               m_currentData.tableIDDict.Insert(index, key);
           });
        }

        private static void RenameField(DataTable table, string oldFieldName, string newFieldName)
        {
            int indexFiled = table.tableKeyDict.IndexOf(oldFieldName);
            table.tableKeyDict[indexFiled] = newFieldName;
            table.commentValueDict = RenameDictionaryKey(table.commentValueDict, oldFieldName, newFieldName);
            table.tableTypesDict = RenameDictionaryKey(table.tableTypesDict, oldFieldName, newFieldName);
            table.tableEnumTypeDict = RenameDictionaryKey(table.tableEnumTypeDict, oldFieldName, newFieldName);
            table.defaultValueDict = RenameDictionaryKey(table.defaultValueDict, oldFieldName, newFieldName);
            foreach (var item in table.tableIDDict)
            {
                table[item] = (SingleData)RenameDictionaryKey(table[item], oldFieldName, newFieldName);
            }
        }

        private static Dictionary<K, V> RenameDictionaryKey<K, V>(Dictionary<K, V> dic, K oldFieldName, K newFieldName)
        {
            if (!dic.ContainsKey(oldFieldName))
                return dic;
            List<K> keys = new List<K>(dic.Keys);
            List<V> values = new List<V>(dic.Values);
            int indexFiled = keys.IndexOf(oldFieldName);
            keys[indexFiled] = newFieldName;
            dic.Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                dic.Add(keys[i], values[i]);
            }
            return dic;
        }

        // 添加字段
        private void AddField(DataTable table, TableConfigFieldInfo info)
        {
            table.tableKeyDict.Add(info.fieldName);
            table.commentValueDict.Add(info.fieldName, info.description);
            table.tableTypesDict.Add(info.fieldName, info.fieldValueType);
            table.fieldAssetTypeDict.Add(info.fieldName, info.fieldAssetType);
            if (info.fieldValueType == FieldType.Enum)
            {
                table.tableEnumTypeDict.Add(info.fieldName, info.enumType);
            }
            if (info.fieldValueType.ToString().Contains("Array"))
            {
                table.arraySplitFormatDict.Add(info.fieldName, info.m_ArraySplitFormat.ToArray());
            }
            table.defaultValueDict.Add(info.fieldName, DataConfigUtils.ObjectValue2TableString(info.defultValue, info.m_ArraySplitFormat));
            foreach (var item in table.Values)
            {
                item.Add(info.fieldName, DataConfigUtils.ObjectValue2TableString(info.defultValue, info.m_ArraySplitFormat));
            }
        }

        static void DeleteField(DataTable table, string fieldName)
        {
            table.tableKeyDict.Remove(fieldName);
            if (table.commentValueDict.ContainsKey(fieldName))
            {
                table.commentValueDict.Remove(fieldName);
            }
            if (table.tableTypesDict.ContainsKey(fieldName))
            {
                table.tableTypesDict.Remove(fieldName);
            }
            if (table.tableEnumTypeDict.ContainsKey(fieldName))
            {
                table.tableEnumTypeDict.Remove(fieldName);
            }
            if (table.defaultValueDict.ContainsKey(fieldName))
            {
                table.defaultValueDict.Remove(fieldName);
            }
            foreach (var item in table.Values)
            {
                if (item.ContainsKey(fieldName))
                    item.Remove(fieldName);
            }
            if (table.arraySplitFormatDict.ContainsKey(fieldName))
            {
                table.arraySplitFormatDict.Remove(fieldName);
            }
        }

        static void ResetDataField(DataTable data, string key, FieldType type, string enumType, object defaultValue)
        {
            List<char> arraySplitFormat = new List<char>();
            if (data.arraySplitFormatDict.ContainsKey(key))
            {
                arraySplitFormat.AddRange(data.arraySplitFormatDict[key]);
            }
            string newContent = DataConfigUtils.ObjectValue2TableString(defaultValue, arraySplitFormat);
            for (int i = 0; i < data.tableIDDict.Count; i++)
            {
                SingleData tmp = data[data.tableIDDict[i]];

                if (tmp.ContainsKey(key))
                {
                    tmp[key] = newContent;
                }
            }
            if (data.defaultValueDict.ContainsKey(key))
            {
                data.defaultValueDict[key] = newContent;
            }
            else
            {
                data.defaultValueDict.Add(key, newContent);
            }
        }

        public static void SaveData(string ConfigName, DataTable data)
        {
            EditorUtil.WriteStringByFile(
                Application.dataPath + "/" + DataManager.directoryName + "/" + ConfigName + ".txt",
                DataTable.Serialize(data));
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}