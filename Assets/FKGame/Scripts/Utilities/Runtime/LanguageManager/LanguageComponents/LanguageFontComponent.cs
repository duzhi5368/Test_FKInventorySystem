using FKGame.Macro;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    [RequireComponent(typeof(Text))]
    public class LanguageFontComponent : MonoBehaviour
    {
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
            try
            {
                Font font = ResourceManager.Load<Font>(LanguageManager.GetContentByKey(ResourcesMacro.LANGUAGE_FONT_KEY));
                m_text.font = font;
            }
            catch (System.Exception e)
            {
                Debug.LogError("…Ë÷√”Ô—‘≥ˆ¥Ì£°m_text£∫" + ResourcesMacro.LANGUAGE_FONT_KEY + "\n" + e);
            }
        }
    }
}