using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
// 列表状UI
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
	public class UIContextMenu : UIWidget
	{
		[Header ("Reference")]
		[SerializeField]
		protected UIMenuItem m_MenuItemPrefab= null;
		protected List<UIMenuItem> itemCache = new List<UIMenuItem> ();

		public override void Show ()
		{
			m_RectTransform.position = Input.mousePosition;
			base.Show ();
		}

		protected override void Update ()
		{
			base.Update();
			if (m_CanvasGroup.alpha > 0f && (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2))) {

				var pointer = new PointerEventData (EventSystem.current);
				pointer.position = Input.mousePosition;
				var raycastResults = new List<RaycastResult> ();
				EventSystem.current.RaycastAll (pointer, raycastResults);

				for (int i = 0; i < raycastResults.Count; i++) {
					UIMenuItem item = raycastResults [i].gameObject.GetComponent<UIMenuItem> ();
					if (item != null) {
						item.OnPointerClick (pointer);
						return;
					}
				}

				Close ();
			}
		}

		public virtual void Clear ()
		{
			for (int i = 0; i < itemCache.Count; i++) {
				itemCache [i].gameObject.SetActive (false);
			}
		}

		public virtual UIMenuItem AddMenuItem (string text, UnityAction used)
		{
			UIMenuItem item = itemCache.Find (x => !x.gameObject.activeSelf);

			if (item == null) {
				item = Instantiate (m_MenuItemPrefab) as UIMenuItem;
				itemCache.Add (item);
			}
			Text itemText = item.GetComponentInChildren<Text> ();

			if (itemText != null) {
				itemText.text = text;
			}
			item.onTrigger.RemoveAllListeners ();
			item.gameObject.SetActive (true);
			item.transform.SetParent (m_RectTransform, false);
			item.onTrigger.AddListener (delegate() {
				Close ();
				if (used != null) {
					used.Invoke ();
				}
			});
			return item;
		}
	}
}