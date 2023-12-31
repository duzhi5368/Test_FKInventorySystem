﻿using FKGame.UIWidgets;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(Canvas))]
    [ComponentMenu("UI/Show Notification")]
    [System.Serializable]
    public class ShowNotification : Action
    {
        [SerializeField]
        private string m_WidgetName = "Notification";
        [SerializeField]
        private UINotificationOptions m_Notification = null;

        public override ActionStatus OnUpdate()
        {
            UINotification widget = WidgetUtility.Find<UINotification>(this.m_WidgetName);
            if (widget == null)
            {
                Debug.LogWarning("Missing notification widget " + this.m_WidgetName + " in scene!");
                return ActionStatus.Failure;
            }
            return widget.AddItem(this.m_Notification)?ActionStatus.Success:ActionStatus.Failure;
        }
    }
}