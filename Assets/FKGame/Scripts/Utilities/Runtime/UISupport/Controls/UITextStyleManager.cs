using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UITextStyleManager
    {
        private const string FilePathDir = "Assets/Resources/Config/";
        private const string FileName = "UITextStyleConfig";
        public static Dictionary<string, Dictionary<SystemLanguage, TextStyleData>> styleDataDic = new Dictionary<string, Dictionary<SystemLanguage, TextStyleData>>();
        public static void Init()
        {
            styleDataDic = LoadData();
        }

        public static Dictionary<string, Dictionary<SystemLanguage, TextStyleData>> LoadData()
        {
            string text = "";
            if (Application.isPlaying)
            {
                text = ResourceManager.LoadText(FileName);
                ResourceManager.DestoryAssetsCounter(FileName);
            }
            else
            {
                text = FileUtils.LoadTextFileByPath(FilePathDir + FileName + ".txt");
            }
            if (!string.IsNullOrEmpty(text))
            {
                return JsonSerializer.FromJson<Dictionary<string, Dictionary<SystemLanguage, TextStyleData>>>(text);
            }
            else
            {
                return new Dictionary<string, Dictionary<SystemLanguage, TextStyleData>>();
            }
        }

        public static void SaveData(Dictionary<string, Dictionary<SystemLanguage, TextStyleData>> styleDataDic)
        {
            string text = JsonSerializer.ToJson(styleDataDic);
            FileUtils.CreateTextFile(FilePathDir + FileName + ".txt", text);
        }

        public static bool ContainsData(string name, SystemLanguage language)
        {
            Dictionary<SystemLanguage, TextStyleData> data = null;
            if (styleDataDic.ContainsKey(name))
                data = styleDataDic[name];
            else
                return false;
            if (data.ContainsKey(language))
            {
                return true;
            }
            else
                return false;
        }

        public static TextStyleData GetTextStyleData(string name, SystemLanguage language)
        {
            Dictionary<SystemLanguage, TextStyleData> data = null;
            if (styleDataDic.ContainsKey(name))
                data = styleDataDic[name];
            else
            {
                Debug.LogError("no TextStyleData name£º" + name);
                return null;
            }
            if (data.ContainsKey(language))
            {
                return data[language];
            }
            else
            {
                Debug.LogError("no TextStyleData language£º" + language);
                return null;
            }
        }

        public static void SetText(Text text, string name, SystemLanguage language)
        {
            if (ContainsData(name, language))
            {
                TextStyleData data = GetTextStyleData(name, language);

                if (!ResourcesConfigManager.IsResourceExist(data.fontName))
                {
                    Debug.LogError("dont find font :" + data.fontName);
                }
                else
                {
                    text.font = ResourceManager.Load<Font>(data.fontName);
                }
                text.fontSize = data.fontSize;
                text.fontStyle = data.fontStyle;
                text.resizeTextForBestFit = data.bestFit;
                text.resizeTextMinSize = data.minSize;
                text.resizeTextMaxSize = data.maxSize;
                text.alignment = data.alignment;
                text.supportRichText = data.richText;
                text.horizontalOverflow = data.horizontalOverflow;
                text.verticalOverflow = data.verticalOverflow;
                text.lineSpacing = data.lineSpacing;
            }
        }

        public static void SetText(Text text, TextStyleData data)
        {
            if (!ResourcesConfigManager.IsResourceExist(data.fontName))
            {
                Debug.LogError("dont find font :" + data.fontName);
            }
            else
            {
                text.font = ResourceManager.Load<Font>(data.fontName);
            }
            text.fontSize = data.fontSize;
            text.fontStyle = data.fontStyle;
            text.resizeTextForBestFit = data.bestFit;
            text.resizeTextMinSize = data.minSize;
            text.resizeTextMaxSize = data.maxSize;
            text.alignment = data.alignment;
            text.supportRichText = data.richText;
            text.horizontalOverflow = data.horizontalOverflow;
            text.verticalOverflow = data.verticalOverflow;
            text.lineSpacing = data.lineSpacing;
        }

        public static TextStyleData GetTextStyleDataFromText(Text text)
        {
            TextStyleData data = new TextStyleData();
            data.fontName = text.font.name;
            data.fontSize = text.fontSize;
            data.fontStyle = text.fontStyle;
            data.bestFit = text.resizeTextForBestFit;
            data.minSize = text.resizeTextMinSize;
            data.maxSize = text.resizeTextMaxSize;
            data.alignment = text.alignment;
            data.richText = text.supportRichText;
            data.horizontalOverflow = text.horizontalOverflow;
            data.verticalOverflow = text.verticalOverflow;
            data.lineSpacing = text.lineSpacing;
            return data;
        }
    }
}