using FKGame.Macro;
using FKGame.UIWidgets;
using UnityEngine;
using UnityEngine.Events;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public static class NotificationExtension
    {
        public static void Show(this UINotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (QuestManager.UI.dialogBox != null)
            {
                QuestManager.UI.dialogBox.Show(options, result, buttons);
            }
        }

        public static void Show(this UINotificationOptions options, params string[] replacements)
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
        public UINotificationOptions toFarAway = new UINotificationOptions()
        {
            text = LanguagesMacro.TO_FAR_AWAY_NOTIFICATION
        };
        public UINotificationOptions inUse = new UINotificationOptions()
        {
            text = LanguagesMacro.IN_USE_NOTIFICATION
        };
        [Header(LanguagesMacro.QUEST_HEADER)]
        public UINotificationOptions questCompleted = new UINotificationOptions()
        {
            text = "{0} 任务已完成"
        };
        public UINotificationOptions questFailed = new UINotificationOptions()
        {
            text = "{0} 任务失败"
        };
        public UINotificationOptions taskCompleted = new UINotificationOptions()
        {
            text = "{0} 任务已完成"
        };
        public UINotificationOptions taskFailed = new UINotificationOptions()
        {
            text = "{0} 任务失败"
        };
        public UINotificationOptions cancelQuest= new UINotificationOptions()
        {
            title = LanguagesMacro.CANCEL_QUEST,
            text = LanguagesMacro.ARE_YOU_SURE_CANCEL_QUEST
        };
    }
}