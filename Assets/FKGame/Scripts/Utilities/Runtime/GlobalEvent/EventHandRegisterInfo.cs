using System;
//------------------------------------------------------------------------
namespace FKGame
{

    public delegate void EventHandle(params object[] args);
    public delegate void EventHandle<T>(T e, params object[] args);

    public class EventHandRegisterInfo
    {
        public Enum m_EventKey;
        public EventHandle m_hande;

        public void RemoveListener()
        {
            GlobalEvent.RemoveEvent(m_EventKey, m_hande);
        }
    }
}