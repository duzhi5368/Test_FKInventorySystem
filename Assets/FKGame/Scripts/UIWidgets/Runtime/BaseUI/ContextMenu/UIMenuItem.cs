﻿using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
//------------------------------------------------------------------------
// 列表状UI子元素
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
	public class UIMenuItem : Selectable, IPointerClickHandler
	{
		private UnityEvent m_Trigger = new UnityEvent ();
		public UnityEvent onTrigger {
			get {
				if (m_Trigger == null) {
					m_Trigger = new UnityEvent ();
				}
				return this.m_Trigger;
			}
			set {
				this.m_Trigger = value;
			}
		}

		private void Press ()
		{
			if (!IsActive () || !IsInteractable ())
				return;

			onTrigger.Invoke ();
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			Press ();
		}

		public override void OnPointerEnter (PointerEventData eventData)
		{
			base.OnPointerEnter (eventData);
			DoStateTransition (SelectionState.Highlighted, false);
		}
	}
}