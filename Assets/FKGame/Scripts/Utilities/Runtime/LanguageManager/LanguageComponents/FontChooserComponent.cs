using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    // 字体选择器，当字体为繁体时自动选择另一套字体
    public class FontChooserComponent : MonoBehaviour
    {
        public Font m_Traditional;
        public Font m_Simplified;
        private Text m_text;

        void Start()
        {
            if (m_text == null)
            {
                m_text = GetComponent<Text>();
            }
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

        void ResetLanguage()
        {
            if (m_text != null)
            {
                if (LanguageManager.CurrentLanguage == SystemLanguage.ChineseTraditional)
                {
                    m_text.font = m_Traditional;
                }
                else
                {
                    m_text.font = m_Simplified;
                }
            }
        }
    }
}