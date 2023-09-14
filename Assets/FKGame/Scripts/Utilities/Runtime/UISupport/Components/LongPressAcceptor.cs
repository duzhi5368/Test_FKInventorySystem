using UnityEngine.EventSystems;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public delegate void InputUIEventLongPressCallBack(InputUIEventType type);

    public class LongPressAcceptor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float LongPressTime = 1f;        // 长按时间
        public InputUIEventLongPressCallBack OnLongPress;
        private float m_Timer = 0;
        private bool isPress = false;
        private bool isDispatch = false;
        private bool isLongPress = false;

        private void OnEnable()
        {
            ResetAcceptor();
        }

        void ResetAcceptor()
        {
            isPress = false;
            isDispatch = false;
            m_Timer = 0;
            isLongPress = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPress = true;
            if (OnLongPress != null)
            {
                OnLongPress(InputUIEventType.PressDown);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnLongPress != null)
            {
                OnLongPress(InputUIEventType.PressUp);
                if (!isLongPress)
                {
                    OnLongPress(InputUIEventType.Click);
                }
            }
            ResetAcceptor();
        }

        void Update()
        {
            if (isPress && !isDispatch)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer > LongPressTime)
                {
                    isDispatch = true;
                    if (OnLongPress != null)
                    {
                        isLongPress = true;
                        OnLongPress(InputUIEventType.LongPress);
                    }
                }
            }
        }
    }
}