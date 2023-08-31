//------------------------------------------------------------------------
// 通知面板
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
	public class UINotification : UIContainer<UINotificationOptions>
	{
		public bool fade = true;
        public string timeFormat = "HH:mm:ss";

        public virtual bool AddItem(UINotificationOptions item, params string[] replacements) {
            UINotificationOptions options = new UINotificationOptions(item);
            for (int i = 0; i < replacements.Length; i++) {
                options.text = options.text.Replace("{"+i+"}", replacements[i]);
            }
            return base.AddItem(options);
        }

        public virtual bool AddItem(string text, params string[] replacements)
        {
            UINotificationOptions options = new UINotificationOptions();
            options.text = text;
            for (int i = 0; i < replacements.Length; i++)
            {
                options.text = options.text.Replace("{" + i + "}", replacements[i]);
            }
            return base.AddItem(options);
        }

        public override bool CanAddItem(UINotificationOptions item, out UISlot<UINotificationOptions> slot, bool createSlot = false)
        {
            slot = null;
            return gameObject.activeInHierarchy && base.CanAddItem(item, out slot, createSlot);
        }
    }
}