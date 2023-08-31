using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
// Tips窗口
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
	public class TooltipTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{
		[SerializeField]
		private string instanceName = "Tooltip";
		[SerializeField]
		private bool showBackground=true;
		[SerializeField]
		private float width = 300;
		[SerializeField]
		private Color color = Color.white;
        public string tooltipTitle;
        [TextArea]
		public string tooltip;
		public Sprite icon;
        public StringPair[] properties;

		private UITooltip instance;
        private Coroutine m_DelayTooltipCoroutine;
        private List<KeyValuePair<string, string>> m_PropertyPairs;

		private void Start ()
		{
			instance = WidgetUtility.Find<UITooltip> (instanceName);
			if (enabled && instance == null) {
				enabled = false;
			}
            this.m_PropertyPairs = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < properties.Length; i++) {
                this.m_PropertyPairs.Add(new KeyValuePair<string, string>(properties[i].key,properties[i].value));
            }
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
            ShowTooltip();
		}
		public void OnPointerExit (PointerEventData eventData)
		{
            CloseTooltip();
		}

        private IEnumerator DelayTooltip(float delay)
        {
            float time = 0.0f;
            yield return true;
            while (time < delay)
            {
                time += Time.deltaTime;
                yield return true;
            }
            instance.Show(WidgetUtility.ColorString(tooltipTitle, color), WidgetUtility.ColorString(tooltip, color), icon, m_PropertyPairs, width, showBackground);
        }

        private void ShowTooltip()
        {
            if (this.m_DelayTooltipCoroutine != null)
            {
                StopCoroutine(this.m_DelayTooltipCoroutine);
            }
            this.m_DelayTooltipCoroutine = StartCoroutine(DelayTooltip(0.3f));
        }

        private void CloseTooltip()
        {
            instance.Close();
            if (this.m_DelayTooltipCoroutine != null)
            {
                StopCoroutine(this.m_DelayTooltipCoroutine);
            }
        }

        [System.Serializable]
        public class StringPair {
            public string key;
            public string value;
        }
    }
}