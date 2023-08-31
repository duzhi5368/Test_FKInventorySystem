using FKGame.Macro;
using FKGame.UIWidgets;
using UnityEngine.Events;
//------------------------------------------------------------------------
// 属性系统 -> 设置 -> 通知
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    public static class NotificationExtension
    {
        public static void Show(this UINotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (StatsManager.UI.dialogBox != null)
            {
                StatsManager.UI.dialogBox.Show(options, result, buttons);
            }
        }

        public static void Show(this UINotificationOptions options, params string[] replacements)
        {
            if (StatsManager.UI.notification != null)
            {
                StatsManager.UI.notification.AddItem(options, replacements);
            }
        }
    }
}

namespace FKGame.StatSystem.Configuration
{
    [System.Serializable]
    public class Notifications : Settings
    {
        public override string Name
        {
            get
            {
                return LanguagesMacro.NOTIFICATION;
            }
        }
    }
}