using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class InputUIEventProxy : IInputProxyBase
    {
        public static InputButtonClickRegisterInfo GetOnClickListener(Button button, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnClickEvent> callback)
        {
            InputButtonClickRegisterInfo info = HeapObjectPool<InputButtonClickRegisterInfo>.GetObject();
            info.eventKey = InputUIOnClickEvent.GetEventKey(UIName, ComponentName, parm);
            info.callBack = callback;
            info.m_button = button;
            info.m_OnClick = () =>
            {
                DispatchOnClickEvent(UIName, ComponentName, parm);
            };
            return info;
        }

        public static InputEventRegisterInfo<InputUILongPressEvent> GetLongPressListener(LongPressAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUILongPressEvent> callback)
        {
            InputlongPressRegisterInfo info = HeapObjectPool<InputlongPressRegisterInfo>.GetObject();
            info.eventKey = InputUILongPressEvent.GetEventKey(UIName, ComponentName, parm);
            info.callBack = callback;
            info.m_acceptor = acceptor;
            info.m_OnLongPress = (type) =>
            {
                DispatchLongPressEvent(UIName, ComponentName, parm, type);
            };
            return info;
        }

        internal static InputEventRegisterInfo<InputUIOnEndDragEvent> GetOnEndDragListener(string m_UIEventKey, string UIName, string ComponentName, InputEventHandle<InputUIOnEndDragEvent> callback)
        {
            InputEventRegisterInfo<InputUIOnEndDragEvent> info = HeapObjectPool<InputEventRegisterInfo<InputUIOnEndDragEvent>>.GetObject();
            info.eventKey = InputUIOnEndDragEvent.GetEventKey(UIName, ComponentName);
            info.callBack = callback;
            InputManager.AddListener(InputUIOnEndDragEvent.GetEventKey(UIName, ComponentName), callback);
            return info;
        }

        internal static InputEventRegisterInfo<InputUIOnDragEvent> GetOnDragListener(string m_UIEventKey, string UIName, string ComponentName, InputEventHandle<InputUIOnDragEvent> callback)
        {
            InputEventRegisterInfo<InputUIOnDragEvent> info = HeapObjectPool<InputEventRegisterInfo<InputUIOnDragEvent>>.GetObject();
            info.eventKey = InputUIOnDragEvent.GetEventKey(UIName, ComponentName);
            info.callBack = callback;
            InputManager.AddListener(InputUIOnDragEvent.GetEventKey(UIName, ComponentName), callback);
            return info;
        }

        internal static InputEventRegisterInfo<InputUIOnBeginDragEvent> GetOnBeginDragListener(string m_UIEventKey, string UIName, string ComponentName, InputEventHandle<InputUIOnBeginDragEvent> callback)
        {
            InputEventRegisterInfo<InputUIOnBeginDragEvent> info = HeapObjectPool<InputEventRegisterInfo<InputUIOnBeginDragEvent>>.GetObject();
            info.eventKey = InputUIOnBeginDragEvent.GetEventKey(UIName, ComponentName);
            info.callBack = callback;
            InputManager.AddListener(InputUIOnBeginDragEvent.GetEventKey(UIName, ComponentName), callback);
            return info;
        }

        internal static InputEventRegisterInfo<InputUIOnMouseEvent> GetOnMouseListener(string m_UIEventKey, string UIName, string ComponentName, bool isDown, InputEventHandle<InputUIOnMouseEvent> callback)
        {
            InputEventRegisterInfo<InputUIOnMouseEvent> info = HeapObjectPool<InputEventRegisterInfo<InputUIOnMouseEvent>>.GetObject();
            info.eventKey = InputUIOnMouseEvent.GetEventKey(UIName, ComponentName, isDown);
            info.callBack = callback;
            InputManager.AddListener(InputUIOnMouseEvent.GetEventKey(UIName, ComponentName, isDown), callback);
            return info;
        }

        public static InputEventRegisterInfo<InputUIOnScrollEvent> GetOnScrollListener(string UIName, string ComponentName, InputEventHandle<InputUIOnScrollEvent> callback)
        {
            InputEventRegisterInfo<InputUIOnScrollEvent> info = HeapObjectPool<InputEventRegisterInfo<InputUIOnScrollEvent>>.GetObject();
            info.eventKey = InputUIOnScrollEvent.GetEventKey(UIName, ComponentName);
            info.callBack = callback;
            InputManager.AddListener(InputUIOnScrollEvent.GetEventKey(UIName, ComponentName), callback);
            return info;
        }

        public static InputEventRegisterInfo<InputUIOnDragEvent> GetOnDragListener(DragAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnDragEvent> callback)
        {
            InputDragRegisterInfo info = HeapObjectPool<InputDragRegisterInfo>.GetObject();
            info.eventKey = InputUIOnDragEvent.GetEventKey(UIName, ComponentName, parm);
            info.callBack = callback;
            info.m_acceptor = acceptor;
            info.m_OnDrag = (data) =>
            {
                DispatchDragEvent(UIName, ComponentName, parm, data);
            };
            return info;
        }

        public static InputBeginDragRegisterInfo GetOnBeginDragListener(DragAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnBeginDragEvent> callback)
        {
            InputBeginDragRegisterInfo info = HeapObjectPool<InputBeginDragRegisterInfo>.GetObject();
            info.eventKey = InputUIOnBeginDragEvent.GetEventKey(UIName, ComponentName, parm);
            info.callBack = callback;
            info.m_acceptor = acceptor;
            info.m_OnBeginDrag = (data) =>
            {
                DispatchBeginDragEvent(UIName, ComponentName, parm, data);
            };
            return info;
        }

        public static InputEndDragRegisterInfo GetOnEndDragListener(DragAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnEndDragEvent> callback)
        {
            InputEndDragRegisterInfo info = HeapObjectPool<InputEndDragRegisterInfo>.GetObject();
            info.eventKey = InputUIOnEndDragEvent.GetEventKey(UIName, ComponentName, parm);
            info.callBack = callback;
            info.m_acceptor = acceptor;
            info.m_OnEndDrag = (data) =>
            {
                DispatchEndDragEvent(UIName, ComponentName, parm, data);
            };
            return info;
        }

        public static void DispatchOnClickEvent(string UIName, string ComponentName, string parm)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUIOnClickEvent e = GetUIEvent<InputUIOnClickEvent>(UIName, ComponentName, parm);
                InputManager.Dispatch("InputUIOnClickEvent", e);
            }
        }

        public static void DispatchLongPressEvent(string UIName, string ComponentName, string parm, InputUIEventType type)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUILongPressEvent e = GetUIEvent<InputUILongPressEvent>(UIName, ComponentName, parm);
                e.m_type = type;
                e.EventKey = InputUILongPressEvent.GetEventKey(UIName, ComponentName, parm);
                InputManager.Dispatch("InputUILongPressEvent", e);
            }
        }

        public static void DispatchScrollEvent(string UIName, string ComponentName, string parm, Vector2 position)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUIOnScrollEvent e = GetOnScrollEvent(UIName, ComponentName, parm, position);
                InputManager.Dispatch("InputUIOnScrollEvent", e);
            }
        }

        public static void DispatchDragEvent(string UIName, string ComponentName, string parm, PointerEventData data)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUIOnDragEvent e = GetUIEvent<InputUIOnDragEvent>(UIName, ComponentName, parm);
                e.m_dragPosition = data.position;
                e.m_delta = data.delta;
                InputManager.Dispatch("InputUIOnDragEvent", e);
            }
        }

        public static void DispatchBeginDragEvent(string UIName, string ComponentName, string parm, PointerEventData data)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUIOnBeginDragEvent e = GetUIEvent<InputUIOnBeginDragEvent>(UIName, ComponentName, parm);
                e.m_dragPosition = data.position;
                e.m_delta = data.delta;
                InputManager.Dispatch("InputUIOnBeginDragEvent", e);
            }
        }

        public static void DispatchMouseEvent(string UIName, string ComponentName, bool isDown, string parm)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUIOnMouseEvent e = GetUIEvent<InputUIOnMouseEvent>(UIName, ComponentName, parm);
                e.m_isDown = isDown;
                e.m_type = isDown ? InputUIEventType.PressDown : InputUIEventType.PressUp;
                InputManager.Dispatch("InputUIOnMouseEvent", e);
            }
        }


        public static void DispatchEndDragEvent(string UIName, string ComponentName, string parm, PointerEventData data)
        {
            //只有允许输入时才派发事件
            if (IsActive)
            {
                InputUIOnEndDragEvent e = GetUIEvent<InputUIOnEndDragEvent>(UIName, ComponentName, parm);
                e.m_dragPosition = data.position;
                e.m_delta = data.delta;
                InputManager.Dispatch("InputUIOnEndDragEvent", e);
            }
        }


        static T GetUIEvent<T>(string UIName, string ComponentName, string parm) where T : InputUIEventBase, new()
        {
            T msg = HeapObjectPool<T>.GetObject();
            msg.Reset();
            msg.m_name = UIName;
            msg.m_compName = ComponentName;
            msg.m_pram = parm;
            return msg;
        }

        static InputUIOnScrollEvent GetOnScrollEvent(string UIName, string ComponentName, string parm, Vector2 position)
        {
            InputUIOnScrollEvent msg = GetUIEvent<InputUIOnScrollEvent>(UIName, ComponentName, parm);
            msg.m_pos = position;
            return msg;
        }
    }
}