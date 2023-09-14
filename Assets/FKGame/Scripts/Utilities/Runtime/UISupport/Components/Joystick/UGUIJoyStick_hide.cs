using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UGUIJoyStick_hide : UIBase, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        protected float mRadius;
        public RectTransform content;
        public RectTransform bg;            // ���ƿ���ק��Χ�Ĵ�С
        public UGUIJoyStickHandle onJoyStick;
        public bool canMove = true;
        // ��ǰ�ֱ��ʣ���UImanager�ϵı�׼�ֱ���֮��Ļ����
        public float conversionX;
        public float conversionY;
        public GameObject rocker;           // ��Ҫ�ƶ������ص�ҡ�˱�����ҡ��
        private RectTransform rockerRectTran;
        private Vector2 referenceResolution;
        private Vector2 centerDelta = new Vector2(0, 0);

        void Start()
        {
            // ����ҡ�˿�İ뾶
            mRadius = (bg.sizeDelta.x - content.sizeDelta.x) * 0.5f;
            referenceResolution = UIManager.UIManagerGo.GetComponent<CanvasScaler>().referenceResolution;
            conversionX = referenceResolution.x / Screen.width;
            conversionY = referenceResolution.y / Screen.height;
            rocker.SetActive(false);
            rockerRectTran = rocker.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canMove = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            centerDelta.x = eventData.delta.x * conversionX;
            centerDelta.y = eventData.delta.y * conversionY;
            Vector3 contentPostion = content.anchoredPosition + centerDelta;
            if (contentPostion.magnitude > mRadius)
            {
                contentPostion = contentPostion.normalized * mRadius;
            }
            content.anchoredPosition3D = contentPostion;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canMove = false;
            content.anchoredPosition3D = Vector3.zero;
            onJoyStick(Vector3.zero);
            rocker.SetActive(false);
        }

        public Vector3 GetDir()
        {
            Vector3 dir = new Vector3(content.anchoredPosition3D.x, 0, content.anchoredPosition3D.y);
            dir /= mRadius;
            return dir;
        }

        void Update()
        {
            if (onJoyStick != null)
            {
                try
                {
                    if (GetDir() != Vector3.zero && canMove)
                    {
                        onJoyStick(GetDir());
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        public void ReHomePos()
        {
            canMove = false;
            content.anchoredPosition3D = Vector3.zero;
            onJoyStick(Vector3.zero);
        }

        // ����Ļ���꣬���㵽UI����
        public Vector3 ScreenPosToUIPos(Vector2 screenPos)
        {
            Vector2 normalized = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            normalized = normalized * 2 - Vector2.one;
            Vector2 UIpos = new Vector2(normalized.x * referenceResolution.x * 0.5f, normalized.y * referenceResolution.y * 0.5f);
            return UIpos;
        }

        // ��갴��ʱ
        public void OnPointerDown(PointerEventData eventData)
        {
            rocker.SetActive(true);
            rockerRectTran.localPosition = ScreenPosToUIPos(eventData.position); ;
        }
    }
}