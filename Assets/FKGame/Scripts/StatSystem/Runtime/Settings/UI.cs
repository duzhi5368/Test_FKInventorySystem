using FKGame.UIWidgets;
using UnityEngine;
using UnityEngine.Assertions;
//------------------------------------------------------------------------
// 属性系统 -> 设置 -> UI
//------------------------------------------------------------------------
namespace FKGame.StatSystem.Configuration
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

        [InspectorLabel("通知面板", "[通知] 组件的名称")]
        public string notificationName = "Notification";
        [InspectorLabel("弹出面板", "[弹出面板] 组件的名称")]
        public string dialogBoxName = "Dialog Box";


        private Notification m_Notification;
        public Notification notification
        {
            get
            {
                if (this.m_Notification == null)
                {
                    this.m_Notification = WidgetUtility.Find<Notification>(this.notificationName);
                    Debug.Log(this.m_Notification);
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
    }
}