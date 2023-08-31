using UnityEngine;
using UnityEngine.UI;
using FKGame.UIWidgets;
//------------------------------------------------------------------------
// 提示信息窗
//------------------------------------------------------------------------
public class NotificationTrigger : MonoBehaviour {
	private UINotification m_Notification;
	public UINotificationOptions[] options;

	private void Start(){
		this.m_Notification = WidgetUtility.Find<UINotification> ("Notification");
	}

	public void AddRandomNotification(){
		UINotificationOptions option=options[Random.Range(0,options.Length)];
		m_Notification.AddItem(option);
	}

	public void AddNotification(InputField input){
		m_Notification.AddItem (input.text);
	}

	public void AddNotification(float index){
		UINotificationOptions option = options [Mathf.RoundToInt (index)];
		m_Notification.AddItem (option);
	}
}
