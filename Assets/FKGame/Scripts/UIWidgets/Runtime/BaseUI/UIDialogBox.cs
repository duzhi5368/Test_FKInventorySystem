using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//------------------------------------------------------------------------
// 弹出窗口
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
    public class UIDialogBox : UIWidget
    {
		public bool autoClose = true;
        [Header("Reference")]
        public Text title;
        public Text text;
        public Image icon;
        public Button button;

        protected List<Button> buttonCache = new List<Button>();
        protected GameObject m_IconParent;

        protected override void OnAwake()
        {
            base.OnAwake();
            if(icon != null)
                m_IconParent = icon.GetComponentInParent<LayoutElement>().gameObject;
        }

        public virtual void Show(UINotificationOptions settings, UnityAction<int> result, params string[] buttons)
        {
            Show(settings.title, WidgetUtility.ColorString(settings.text, settings.color), settings.icon, result, buttons);
        }

        public virtual void Show(string title, string text, params string[] buttons)
        {
            Show(title, text, null, null, buttons);
        }

        public virtual void Show(string title, string text, UnityAction<int> result, params string[] buttons)
        {
            Show(title, text, null, result, buttons);
        }

        public virtual void Show(string title, string text, Sprite icon, UnityAction<int> result, params string[] buttons)
        {
            for (int i = 0; i < buttonCache.Count; i++)
            {
                buttonCache[i].onClick.RemoveAllListeners();
                buttonCache[i].gameObject.SetActive(false);
            }
            if (this.title != null)
            {
                if (!string.IsNullOrEmpty(title))
                {
                    this.title.text = title;
                    this.title.gameObject.SetActive(true);
                }
                else
                {
                    this.title.gameObject.SetActive(false);
                }
            }
            if (this.text != null)
            {
                this.text.text = text;
            }

            if (this.icon != null)
            {
                if (icon != null)
                {
                    this.icon.overrideSprite = icon;
                    this.m_IconParent.SetActive(true);
                }
                else
                {
                    this.m_IconParent.SetActive(false);
                }
            }
            base.Show();
            button.gameObject.SetActive(false);
            for (int i = 0; i < buttons.Length; i++)
            {
                string caption = buttons[i];
                int index = i;
                AddButton(caption).onClick.AddListener(delegate () {
                    if (this.autoClose)
                    {
                        base.Close();
                    }
                    if (result != null)
                    {
                        result.Invoke(index);
                    }
                });
            }
        }

        private Button AddButton(string text)
        {
            Button mButton = buttonCache.Find(x => !x.isActiveAndEnabled);
            if (mButton == null)
            {
                mButton = Instantiate(button) as Button;
                buttonCache.Add(mButton);
            }
            mButton.gameObject.SetActive(true);
            mButton.onClick.RemoveAllListeners();
            mButton.transform.SetParent(button.transform.parent, false);
            Text[] buttonTexts = mButton.GetComponentsInChildren<Text>(true);
            if (buttonTexts.Length > 0)
            {
                buttonTexts[0].text = text;
            }
            return mButton;
        }
    }
}