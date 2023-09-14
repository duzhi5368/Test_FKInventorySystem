using System;
using UnityEngine.UI;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 输入检测屏蔽字组件
    public class MaskWord : MonoBehaviour
    {
        public char splitChar = ',';        // 分割字符串
        public string textName = "";        // 字库资源名
        private string[] SentiWords = null; // 定义一个接受文件内容的字符串数组
        private InputField inputField;
        private CallBack<bool> callBack;    // true 表示有屏蔽字，需要重新输入

        public void Init(CallBack<bool> l_callBack)
        {
            callBack = null;
            callBack += l_callBack;
        }

        void Start()
        {
            transform.GetComponent<InputField>().onValueChanged.AddListener(OnValueChanged);
            if (String.IsNullOrEmpty(textName)) // 无屏蔽字库
            {
                Debug.LogError("MaskWord textName error = " + textName);
                return;
            }
            SentiWords = ResourceManager.LoadText(textName).Split(splitChar);
            ResourceManager.DestoryAssetsCounter(textName);
            for (int i = 0; i < SentiWords.Length; i++)
            {
                if (SentiWords[i].Contains("\n"))
                {
                    SentiWords[i] = SentiWords[i].Replace("\r", "");
                    SentiWords[i] = SentiWords[i].Replace("\n", "");
                }
            }
            inputField = transform.GetComponent<InputField>();
        }

        private void OnValueChanged(string t)
        {
            bool needReInput = false;
            if (SentiWords == null)
                return;
            if (!LanguageManager.CurrentLanguageIsChinese())
                return;
            if (string.IsNullOrEmpty(t))
                return;
            foreach (string ssr in SentiWords)
            {
                if (t.Contains(ssr))
                {
                    if (!ssr.Equals(""))
                    {
                        needReInput = true;
                        Debug.Log("包含敏感词汇:" + ssr + ",需要进行替换");
                        break;
                    }
                }
            }
            if (needReInput)
            {
                inputField.text = null;
                if (callBack != null)
                {
                    callBack(needReInput);
                }
            }
        }
    }
}