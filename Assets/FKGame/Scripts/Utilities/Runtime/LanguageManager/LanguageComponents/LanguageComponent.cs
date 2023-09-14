using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{

    [RequireComponent(typeof(Text))]
    public class LanguageComponent : MonoBehaviour
    {
        public string languageKey = "";
        [System.NonSerialized]
        public Text m_text;

        public void Start()
        {
            if (m_text == null)
            {
                m_text = GetComponent<Text>();
            }
            Init();
        }

        public void Init()
        {
            ResetLanguage();
            LanguageManager.OnChangeLanguage += OnChangeLanguage;
        }

        private void OnChangeLanguage(SystemLanguage t)
        {
            ResetLanguage();
        }

        private void OnDestroy()
        {
            LanguageManager.OnChangeLanguage -= OnChangeLanguage;
        }

        public void ResetLanguage()
        {
            if (string.IsNullOrEmpty(languageKey))
                return;
            try
            {
                string value = LanguageManager.GetContentByKey(languageKey).ToString();
                value = value.Replace("\\n", "\n");
                m_text.text = value;
            }
            catch (System.Exception e)
            {
                Debug.LogError("…Ë÷√”Ô—‘≥ˆ¥Ì£°m_text£∫" + m_text + "\n" + e);
            }
        }
    }
}