using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UGUIJoyStick_Ro : UGUIJoyStickBase
    {
        const float c_baseScreenWidth = 1920; // ������Ļ���ȷֱ���
        private float screenScale;
        public RectTransform rotate;
        public Image center;

        override public void MyStart()
        {
            base.MyStart();
            screenScale = c_baseScreenWidth / Screen.width;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            center.enabled = true;
        }

        override public void OnDrag(PointerEventData eventData)
        {

            Vector3 contentPostion = content.anchoredPosition + eventData.delta * screenScale;
            if (contentPostion.magnitude > mRadius)
            {
                contentPostion = contentPostion.normalized * mRadius;
            }
            content.anchoredPosition3D = contentPostion;
            float z = Mathf.Atan2(contentPostion.normalized.y, contentPostion.normalized.x) * 180 / Mathf.PI - 90 + 50;
            rotate.localEulerAngles = new Vector3(0, 0, z);
        }

        override public void OnEndDrag(PointerEventData eventData)
        {
            content.anchoredPosition3D = Vector3.zero;
            center.enabled = false;
        }
    }
}