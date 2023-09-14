using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GlobalEvent
    {
        // ��EnumΪKey���¼��ɷ�
        private static Dictionary<Enum, EventHandle> mEventDic = new Dictionary<Enum, EventHandle>();
        private static Dictionary<Enum, List<EventHandle>> mUseOnceEventDic = new Dictionary<Enum, List<EventHandle>>();

        // ����¼����ص�
        public static void AddEvent(Enum type, EventHandle handle, bool isUseOnce = false)
        {
            if (isUseOnce)
            {
                if (mUseOnceEventDic.ContainsKey(type))
                {
                    if (!mUseOnceEventDic[type].Contains(handle))
                        mUseOnceEventDic[type].Add(handle);
                    else
                        Debug.LogWarning("already existing EventType: " + type + " handle: " + handle);
                }
                else
                {
                    List<EventHandle> temp = new List<EventHandle>();
                    temp.Add(handle);
                    mUseOnceEventDic.Add(type, temp);
                }
            }
            else
            {
                if (mEventDic.ContainsKey(type))
                {
                    mEventDic[type] += handle;
                }
                else
                {
                    mEventDic.Add(type, handle);
                }
            }
        }

        internal static void AddTypeEvent<T>(string v)
        {
            throw new NotImplementedException();
        }

        // �Ƴ�ĳ���¼���һ���ص�
        public static void RemoveEvent(Enum type, EventHandle handle)
        {
            if (mUseOnceEventDic.ContainsKey(type))
            {
                if (mUseOnceEventDic[type].Contains(handle))
                {
                    mUseOnceEventDic[type].Remove(handle);
                    if (mUseOnceEventDic[type].Count == 0)
                    {
                        mUseOnceEventDic.Remove(type);
                    }
                }
            }
            if (mEventDic.ContainsKey(type))
            {
                mEventDic[type] -= handle;
            }
        }

        internal static void AddTypeEvent<T>()
        {
            throw new NotImplementedException();
        }

        // �Ƴ�ĳ���¼�
        public static void RemoveEvent(Enum type)
        {
            if (mUseOnceEventDic.ContainsKey(type))
            {
                mUseOnceEventDic.Remove(type);
            }

            if (mEventDic.ContainsKey(type))
            {
                mEventDic.Remove(type);
            }
        }

        // �����¼�
        public static void DispatchEvent(Enum type, params object[] args)
        {
            if (mEventDic.ContainsKey(type))
            {
                try
                {
                    if (mEventDic[type] != null)
                    {
                        mEventDic[type](args);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }

            if (mUseOnceEventDic.ContainsKey(type))
            {
                for (int i = 0; i < mUseOnceEventDic[type].Count; i++)
                {
                    // ����ί������
                    foreach (EventHandle callBack in mUseOnceEventDic[type][i].GetInvocationList())
                    {
                        try
                        {
                            callBack(args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                    }
                }
                RemoveEvent(type);
            }
        }


        // ��StringΪKey���¼��ɷ�
        private static Dictionary<string, List<EventHandle>> m_stringEventDic = new Dictionary<string, List<EventHandle>>();
        private static Dictionary<string, List<EventHandle>> m_stringOnceEventDic = new Dictionary<string, List<EventHandle>>();

        // ����¼����ص�
        public static void AddEvent(string eventKey, EventHandle handle, bool isUseOnce = false)
        {
            if (isUseOnce)
            {
                if (m_stringOnceEventDic.ContainsKey(eventKey))
                {
                    if (!m_stringOnceEventDic[eventKey].Contains(handle))
                        m_stringOnceEventDic[eventKey].Add(handle);
                    else
                        Debug.LogWarning("already existing EventType: " + eventKey + " handle: " + handle);
                }
                else
                {
                    List<EventHandle> temp = new List<EventHandle>();
                    temp.Add(handle);
                    m_stringOnceEventDic.Add(eventKey, temp);
                }
            }
            else
            {
                if (m_stringEventDic.ContainsKey(eventKey))
                {
                    if (!m_stringEventDic[eventKey].Contains(handle))
                        m_stringEventDic[eventKey].Add(handle);
                    else
                        Debug.LogWarning("already existing EventType: " + eventKey + " handle: " + handle);
                }
                else
                {
                    List<EventHandle> temp = new List<EventHandle>();
                    temp.Add(handle);
                    m_stringEventDic.Add(eventKey, temp);
                }
            }
        }

        internal static void AddEvent<T>(object onRequestRealNameResult)
        {
            throw new NotImplementedException();
        }

        // �Ƴ�ĳ���¼���һ���ص�
        public static void RemoveEvent(string eventKey, EventHandle handle)
        {
            if (m_stringEventDic.ContainsKey(eventKey))
            {
                if (m_stringEventDic[eventKey].Contains(handle))
                {
                    m_stringEventDic[eventKey].Remove(handle);
                }
            }

            if (m_stringOnceEventDic.ContainsKey(eventKey))
            {
                if (m_stringOnceEventDic[eventKey].Contains(handle))
                {
                    m_stringOnceEventDic[eventKey].Remove(handle);
                }
            }
        }

        // �Ƴ�ĳ���¼�
        public static void RemoveEvent(string eventKey)
        {
            if (m_stringEventDic.ContainsKey(eventKey))
            {
                m_stringEventDic.Remove(eventKey);
            }
            if (m_stringOnceEventDic.ContainsKey(eventKey))
            {
                m_stringOnceEventDic.Remove(eventKey);
            }
        }

        //  �Ƴ������¼�
        public static void RemoveAllEvent()
        {
            mUseOnceEventDic.Clear();

            mEventDic.Clear();

            m_stringEventDic.Clear();
        }

        // �����¼�
        public static void DispatchEvent(string eventKey, params object[] args)
        {
            if (m_stringEventDic.ContainsKey(eventKey))
            {
                for (int i = 0; i < m_stringEventDic[eventKey].Count; i++)
                {
                    // ����ί������
                    foreach (EventHandle callBack in m_stringEventDic[eventKey][i].GetInvocationList())
                    {
                        try
                        {
                            callBack(args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                    }
                }
            }
            if (m_stringOnceEventDic.ContainsKey(eventKey))
            {
                for (int i = 0; i < m_stringOnceEventDic[eventKey].Count; i++)
                {
                    // ����ί������
                    foreach (EventHandle callBack in m_stringOnceEventDic[eventKey][i].GetInvocationList())
                    {
                        try
                        {
                            callBack(args);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.ToString());
                        }
                    }
                }
                RemoveEvent(eventKey);
            }
        }

        // ��TypeΪKey���¼��ɷ�
        private static Dictionary<Type, EventDispatcher> mTypeEventDic = new Dictionary<Type, EventDispatcher>();
        private static Dictionary<Type, EventDispatcher> mTypeUseOnceEventDic = new Dictionary<Type, EventDispatcher>();

        // ����¼����ص�
        public static void AddTypeEvent<T>(EventHandle<T> handle, bool isUseOnce = false)
        {
            GetEventDispatcher<T>(isUseOnce).m_CallBack += handle;
        }

        // �Ƴ�ĳ���¼���һ���ص�
        public static void RemoveTypeEvent<T>(EventHandle<T> handle, bool isUseOnce = false)
        {
            GetEventDispatcher<T>(isUseOnce).m_CallBack -= handle;
        }

        // �Ƴ�ĳ���¼�
        public static void RemoveTypeEvent<T>(bool isUseOnce = false)
        {
            GetEventDispatcher<T>(isUseOnce).m_CallBack = null;
        }

        // �����¼�
        public static void DispatchTypeEvent<T>(T e, params object[] args)
        {
            GetEventDispatcher<T>(false).Call(e, args);
            //ֻ����һ�εĵ��ú�����
            GetEventDispatcher<T>(true).Call(e, args);
            GetEventDispatcher<T>(true).m_CallBack = null;
        }

        static EventDispatcher<T> GetEventDispatcher<T>(bool isOnce)
        {
            Type type = typeof(T);
            if (isOnce)
            {
                if (mTypeUseOnceEventDic.ContainsKey(type))
                {
                    return (EventDispatcher<T>)mTypeUseOnceEventDic[type];
                }
                else
                {
                    EventDispatcher<T> temp = new EventDispatcher<T>();
                    mTypeUseOnceEventDic.Add(type, temp);
                    return temp;
                }
            }
            else
            {
                if (mTypeEventDic.ContainsKey(type))
                {
                    return (EventDispatcher<T>)mTypeEventDic[type];
                }
                else
                {
                    EventDispatcher<T> temp = new EventDispatcher<T>();
                    mTypeEventDic.Add(type, temp);
                    return temp;
                }
            }
        }

        abstract class EventDispatcher { }
        class EventDispatcher<T> : EventDispatcher
        {
            public EventHandle<T> m_CallBack;
            public void Call(T e, params object[] args)
            {
                if (m_CallBack != null)
                {
                    try
                    {
                        m_CallBack(e, args);
                    }
                    catch (Exception exc)
                    {
                        Debug.LogError(exc.ToString());
                    }
                }
            }
        }
    }
}