using FKGame.Macro;
using FKGame.UIWidgets;
using UnityEngine;
using UnityEngine.Events;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public static class NotificationExtension
    {
        public static void Show(this NotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (QuestManager.UI.dialogBox != null)
            {
                QuestManager.UI.dialogBox.Show(options, result, buttons);
            }
        }

        public static void Show(this NotificationOptions options, params string[] replacements)
        {
            if (QuestManager.UI.notification != null)
            {
                QuestManager.UI.notification.AddItem(options, replacements);
            }
        }
    }
}

namespace FKGame.QuestSystem
{
    [System.Serializable]
    public class QuestNotifications : Settings
    {
        public override string Name
        {
            get
            {
                return LanguagesMacro.NOTIFICATION;
            }
        }
        [Header(LanguagesMacro.TRIGGER_HEADER)]
        public NotificationOptions toFarAway = new NotificationOptions()
        {
            text = LanguagesMacro.TO_FAR_AWAY_NOTIFICATION
        };
        public NotificationOptions inUse = new NotificationOptions()
        {
            text = LanguagesMacro.IN_USE_NOTIFICATION
        };
        [Header(LanguagesMacro.QUEST_HEADER)]
        public NotificationOptions questCompleted = new NotificationOptions()
        {
            text = "{0} 任务已完成"
        };
        public NotificationOptions questFailed = new NotificationOptions()
        {
            text = "{0} 任务失败"
        };
        public NotificationOptions taskCompleted = new NotificationOptions()
        {
            text = "{0} 任务已完成"
        };
        public NotificationOptions taskFailed = new NotificationOptions()
        {
            text = "{0} 任务失败"
        };
        public NotificationOptions cancelQuest= new NotificationOptions()
        {
            title = LanguagesMacro.CANCEL_QUEST,
            text = LanguagesMacro.ARE_YOU_SURE_CANCEL_QUEST
        };
    }
}