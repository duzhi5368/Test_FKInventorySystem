﻿using FKGame.Macro;
using FKGame.UIWidgets;
using UnityEngine.Assertions;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [System.Serializable]
    public class UI : Settings
    {
        public override string Name
        {
            get
            {
                return "UI";
            }
        }

        [InspectorLabel(LanguagesMacro.NOTIFICATION_WIDGET)]
        public string notificationName = "Notification";
        [InspectorLabel(LanguagesMacro.DIALOG_WIDGET)]
        public string dialogBoxName = "Dialog Box";
        [InspectorLabel(LanguagesMacro.QUEST_WINDOW_WIDGET)]
        public string questWindowName = "Quest";
        [InspectorLabel(LanguagesMacro.QUEST_SELECTION_WINDOW_WIDGET)]
        public string questSelectionWindowName = "Quest Selection";

        private Notification m_Notification;
        public Notification notification
        {
            get
            {
                if (this.m_Notification == null)
                {
                    this.m_Notification = WidgetUtility.Find<Notification>(this.notificationName);
                }
                Assert.IsNotNull(this.m_Notification, "Notification widget with name " + this.notificationName + " is not present in scene.");
                return this.m_Notification;
            }
        }

        private DialogBox m_DialogBox;
        public DialogBox dialogBox
        {
            get
            {
                if (this.m_DialogBox == null)
                {
                    this.m_DialogBox = WidgetUtility.Find<DialogBox>(this.dialogBoxName);
                }
                Assert.IsNotNull(this.m_DialogBox, "DialogBox widget with name " + this.dialogBoxName + " is not present in scene.");
                return this.m_DialogBox;
            }
        }

        private QuestWindow m_QuestWindow;
        public QuestWindow questWindow
        {
            get
            {
                if (this.m_QuestWindow == null)
                {
                    this.m_QuestWindow = WidgetUtility.Find<QuestWindow>(questWindowName);
                }
                Assert.IsNotNull(this.m_QuestWindow, "QuestWindow widget with name " + this.questWindowName + " is not present in scene.");
                return this.m_QuestWindow;
            }
        }

        private DialogBox m_QuestSelectionWindow;
        public DialogBox questSelectionWindow
        {
            get
            {
                if (this.m_QuestSelectionWindow == null)
                {
                    this.m_QuestSelectionWindow = WidgetUtility.Find<DialogBox>(this.questSelectionWindowName);
                }
                Assert.IsNotNull(this.m_QuestSelectionWindow, "DialogBox widget with name " + this.questSelectionWindowName + " is not present in scene.");
                return this.m_QuestSelectionWindow;
            }
        }
    }
}