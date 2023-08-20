﻿using FKGame.UIWidgets;
using UnityEngine;
using UnityEngine.Events;

namespace FKGame.StatSystem
{
    public static class NotificationExtension
    {
        public static void Show(this NotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (StatsManager.UI.dialogBox != null)
            {
                StatsManager.UI.dialogBox.Show(options, result, buttons);
            }
        }

        public static void Show(this NotificationOptions options, params string[] replacements)
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
                return "Notification";
            }
        }
    }
}