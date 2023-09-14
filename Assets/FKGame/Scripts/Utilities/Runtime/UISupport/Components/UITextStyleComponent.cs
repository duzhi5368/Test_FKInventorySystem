using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    [RequireComponent(typeof(Text))]
    public class UITextStyleComponent : MonoBehaviour
    {
        public string styleName;
        private Text text;

        void Start()
        {
            SystemLanguage language = LanguageManager.CurrentLanguage;
            SetTextStyleData(language);
        }

        public void SetTextStyleData(SystemLanguage language)
        {
            if (text == null)
                text = GetComponent<Text>();
            if (string.IsNullOrEmpty(styleName))
                return;
            UITextStyleManager.SetText(text, styleName, language);
        }
    }
}