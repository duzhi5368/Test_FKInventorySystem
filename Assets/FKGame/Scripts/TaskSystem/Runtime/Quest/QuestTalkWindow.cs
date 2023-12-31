﻿using FKGame.UIWidgets;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public class QuestTalkWindow : UIWidget
    {
        [Header("References")]
        [SerializeField]
        protected Text m_Title;
        [SerializeField]
        protected Text m_Text;

        public virtual void Show(string title, string text) {
            if (this.m_Title != null)
                this.m_Title.text = title;
            if (this.m_Text != null)
                this.m_Text.text = text;
            base.Show();
        }
    }
}