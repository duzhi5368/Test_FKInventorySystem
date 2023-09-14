using System;
using UnityEngine.UI;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ���������������
    public class MaskWord : MonoBehaviour
    {
        public char splitChar = ',';        // �ָ��ַ���
        public string textName = "";        // �ֿ���Դ��
        private string[] SentiWords = null; // ����һ�������ļ����ݵ��ַ�������
        private InputField inputField;
        private CallBack<bool> callBack;    // true ��ʾ�������֣���Ҫ��������

        public void Init(CallBack<bool> l_callBack)
        {
            callBack = null;
            callBack += l_callBack;
        }

        void Start()
        {
            transform.GetComponent<InputField>().onValueChanged.AddListener(OnValueChanged);
            if (String.IsNullOrEmpty(textName)) // �������ֿ�
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
                        Debug.Log("�������дʻ�:" + ssr + ",��Ҫ�����滻");
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