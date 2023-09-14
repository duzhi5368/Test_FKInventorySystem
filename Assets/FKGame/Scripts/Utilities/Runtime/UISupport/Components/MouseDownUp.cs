using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class MouseDownUp : MonoBehaviour
    {
        private string m_UIEventKey;
        private InputEventRegisterInfo<InputUIOnMouseEvent> inputUIOnMouseEventDown;
        private InputEventRegisterInfo<InputUIOnMouseEvent> inputUIOnMouseEventUp;

        public virtual void InitEvent(string UIEventKey)
        {
            m_UIEventKey = UIEventKey;
            InputUIEventProxy.GetOnMouseListener(m_UIEventKey, name, name, true, OnMouseDownEvent);
            InputUIEventProxy.GetOnMouseListener(m_UIEventKey, name, name, false, OnMouseUpEvent);
        }

        public virtual void OnMouseDownEvent(InputUIOnMouseEvent inputEvent){}
        public virtual void OnMouseUpEvent(InputUIOnMouseEvent inputEvent){}

        public void DisposeEvent()
        {
            inputUIOnMouseEventDown.RemoveListener();
            inputUIOnMouseEventUp.RemoveListener();
            inputUIOnMouseEventDown = null;
            inputUIOnMouseEventUp = null;
        }

        private void OnMouseDown()
        {
            InputUIEventProxy.DispatchMouseEvent(name, name, true, null);
        }

        private void OnMouseUp()
        {
            InputUIEventProxy.DispatchMouseEvent(name, name, false, null);
        }
    }
}