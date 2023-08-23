using UnityEngine;
using UnityEngine.UI;
using FKGame.UIWidgets;
//------------------------------------------------------------------------
// 提示信息窗
//------------------------------------------------------------------------
public class NotificationTrigger : MonoBehaviour {
	private Notification m_Notification;
	public NotificationOptions[] options;

	private void Start(){
		this.m_Notification = WidgetUtility.Find<Notification> ("Notification");
	}

	public void AddRandomNotification(){
		NotificationOptions option=options[Random.Range(0,options.Length)];
		m_Notification.AddItem(option);
	}

	public void AddNotification(InputField input){
		m_Notification.AddItem (input.text);
	}

	public void AddNotification(float index){
		NotificationOptions option = options [Mathf.RoundToInt (index)];
		m_Notification.AddItem (option);
	}
}
