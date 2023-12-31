﻿using FKGame.Macro;
using FKGame.UIWidgets;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [Icon("Quest")]
    [ComponentMenu(LanguagesMacro.QUEST_SYSTEM + "/" + LanguagesMacro.SHOW_TALK_WINDOW)]
    [System.Serializable]
    public class ActionShowTalkWindow : Action
    {
        [SerializeField]
        protected string m_WindowName;
        [SerializeField]
        protected string m_Title = "Talk";
        [SerializeField]
        protected string m_Text = "";

        protected QuestTalkWindow m_TalkQuestWindow;

        public override void OnStart()
        {
            this.m_TalkQuestWindow = WidgetUtility.Find<QuestTalkWindow>(this.m_WindowName);
        }

        public override ActionStatus OnUpdate()
        {
            if (this.m_TalkQuestWindow == null)
            {
                Debug.LogWarning("Missing window " + this.m_WindowName + " in scene!");
                return ActionStatus.Failure;
            }
            this.m_TalkQuestWindow.Show(this.m_Title, this.m_Text);
            return ActionStatus.Success;
        }
    }
}
