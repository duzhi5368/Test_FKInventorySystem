using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LanguageManager
    {
        public const string c_configFileName = "LanguageConfig";
        public const string c_defaultModuleKey = "default";
        public const string c_DataFilePrefix = "LangData_";
        public const string c_mainKey = "key";
        public const string c_valueKey = "value";
        public static CallBack<SystemLanguage> OnChangeLanguage;
        static private Dictionary<string, string> s_languageDataDict = new Dictionary<string, string>();    // ������������
        private static Dictionary<string, int> loadTextFileTimesDic = new Dictionary<string, int>();
        private static LanguageSettingConfig config;

        static private SystemLanguage s_currentLanguage = SystemLanguage.ChineseSimplified;                 // ��ǰ����
        public static SystemLanguage CurrentLanguage
        {
            get
            {
                Init();
                return s_currentLanguage;
            }
        }

        static private bool isInit = false;
        public static bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }

        public static void Init()
        {
            if (!isInit)
            {
                isInit = true;
                if (config == null)
                {
                    config = LanguageDataUtils.LoadRuntimeConfig();
                }
                if (config == null)
                    return;
                s_currentLanguage = SetConfig();
                Debug.Log("ʹ�����ԣ�" + s_currentLanguage);
            }
        }

        private static SystemLanguage SetConfig()
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            if (systemLanguage == SystemLanguage.Chinese)
            {
                systemLanguage = SystemLanguage.ChineseSimplified;
            }
            Debug.Log("config.useSystemLanguage:" + config.useSystemLanguage + " config.defaultLanguage:" + config.defaultLanguage);
            if (config.useSystemLanguage)
            {

                if (config.gameExistLanguages.Contains(systemLanguage))
                {
                    return systemLanguage;
                }
                else
                {
                    if (config.gameExistLanguages.Contains(SystemLanguage.English))
                    {
                        return SystemLanguage.English;
                    }
                }
            }
            return config.defaultLanguage;
        }

        public static void SetLanguage(SystemLanguage lang)
        {
            Init();
            SystemLanguage oldLan = s_currentLanguage;
            if (config == null)
                return;
            if (lang == SystemLanguage.Chinese)
                lang = SystemLanguage.ChineseSimplified;

            if (config.gameExistLanguages.Contains(lang))
            {
                Debug.Log("�л����ԣ�" + lang);
                s_currentLanguage = lang;
            }
            else
            {
                Debug.LogError("��ǰ���Բ����� " + lang);
                return;
            }
            if (oldLan != s_currentLanguage)
            {
                s_languageDataDict.Clear();
                if (OnChangeLanguage != null)
                {
                    OnChangeLanguage(s_currentLanguage);
                }
            }
        }

        [Obsolete]
        public static string GetContent(string moduleName, string contentID, List<object> contentParams)
        {
            return GetContent(moduleName, contentID, contentParams.ToArray());
        }

        public static bool ContainsFullKeyName(string fullKeyName)
        {
            if (string.IsNullOrEmpty(fullKeyName))
            {
                Debug.LogError("LanguageManager =>Error : key is null :" + fullKeyName);
                return false;
            }
            Init();

            if (s_languageDataDict.ContainsKey(fullKeyName))
            {
                return true;
            }
            else
            {
                int indexEnd = fullKeyName.LastIndexOf("/");
                if (indexEnd < 0)
                {
                    Debug.LogError("LanguageManager => Error : Format is error :" + fullKeyName);
                    return false;
                }
                string fullFileName = fullKeyName.Remove(indexEnd);
                DataTable data = LoadDataTable(s_currentLanguage, fullFileName);
                foreach (var item in data.tableIDDict)
                {
                    try
                    {
                        if (!s_languageDataDict.ContainsKey(fullFileName + "/" + item))
                        {
                            s_languageDataDict.Add(fullFileName + "/" + item, data[item].GetString(c_valueKey));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Find:" + fullKeyName + "\n ContainsFullKeyName Error (" + fullFileName + "/" + item + ") -> (" + data[item].GetString(c_valueKey) + ")\n" + e);
                    }
                }
                return s_languageDataDict.ContainsKey(fullKeyName);
            }
        }

        public static string GetContentByKey(string fullKeyName, params object[] contentParams)
        {
            Init();
            string content = null;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                if (ContainsFullKeyName(fullKeyName))
                {
                    content = s_languageDataDict[fullKeyName];
                }
                else
                {
                    Debug.LogError("LanguageManager => Error : no find key :" + fullKeyName);
                    return "";
                }

                stringBuilder.Append(content);
                if (contentParams != null && contentParams.Length > 0)
                {
                    for (int i = 0; i < contentParams.Length; i++)
                    {
                        object pars = contentParams[i];
                        if (pars == null)
                            continue;
                        string replaceTmp = "{" + i + "}";
                        stringBuilder.Replace(replaceTmp, pars.ToString());
                    }
                }
                if (ApplicationManager.Instance != null && ApplicationManager.Instance.showLanguageValue && ApplicationManager.Instance.m_AppMode == AppMode.Developing)
                {
                    stringBuilder.Insert(0, "[");
                    stringBuilder.Insert(stringBuilder.Length - 1, "]");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("�����Ի�ȡ����!" + fullKeyName + "\n" + e);
                return null;
            }
            return stringBuilder.ToString();
        }

        private static DataTable LoadDataTable(SystemLanguage language, string fullFileName)
        {
            if (Application.isPlaying)
            {
                string name = GetLanguageDataName(language, fullFileName);
                TextAsset text = ResourceManager.Load<TextAsset>(name);
                if (text == null)
                {
                    Debug.LogError("Error�� no Language file ��" + name);
                    return null;
                }
                if (loadTextFileTimesDic.ContainsKey(name))
                    loadTextFileTimesDic[name]++;
                else
                {
                    loadTextFileTimesDic.Add(name, 1);
                }
                DataTable data = DataTable.Analysis(text.text);
                return data;
            }
            else
            {
                return LanguageDataUtils.LoadFileData(language, fullFileName);
            }
        }

        [Obsolete]
        public static string GetContent(string moduleName, string contentID, params object[] contentParams)
        {
            string fullkey = moduleName.Replace('_', '/') + "/" + contentID;
            return GetContentByKey(fullkey, contentParams);
        }

        // ��ö����Ա�����ļ���
        public static string GetLanguageDataName(SystemLanguage langeuageName, string fullkeyFileName)
        {
            string modelName = fullkeyFileName.Replace('/', '_');
            return c_DataFilePrefix + langeuageName + "_" + modelName;
        }

        public static void Release()
        {
            s_languageDataDict.Clear();
            isInit = false;

            foreach (var item in loadTextFileTimesDic)
            {
                ResourceManager.DestoryAssetsCounter(item.Key, item.Value);
            }
            loadTextFileTimesDic.Clear();
        }

        //��ǰ�����Ƿ��Ǻ���
        public static bool CurrentLanguageIsChinese()
        {
            bool isChinese = LanguageManager.CurrentLanguage == SystemLanguage.ChineseSimplified || LanguageManager.CurrentLanguage == SystemLanguage.ChineseTraditional || LanguageManager.CurrentLanguage == SystemLanguage.Chinese;
            return isChinese;
        }
    }
}