using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
// 通知面板中的单独一项通知
//------------------------------------------------------------------------
namespace FKGame.UIWidgets{
	[System.Serializable]
	public class UINotificationOptions {
        public string title = string.Empty;
		public string text = string.Empty;
		public Color color = Color.white;
		public Sprite icon;
		public float delay = 2.0f;
		public float duration = 2.0f;
		public bool ignoreTimeScale = true;

		public UINotificationOptions(UINotificationOptions other){
			this.title = other.title;
			this.text=other.text;
			this.icon = other.icon;
			this.color=other.color;
			this.duration=other.duration;
			this.ignoreTimeScale=other.ignoreTimeScale;
		}
		
		public UINotificationOptions(){}
	}
}