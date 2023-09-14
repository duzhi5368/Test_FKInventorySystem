using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class InputDispatcher<Event> : IInputDispatcher where Event : IInputEventBase
    {
        protected Dictionary<string, InputEventHandle<Event>> m_Listeners = new Dictionary<string, InputEventHandle<Event>>();
        // ���д��������¼�����ʱ�������
        public InputEventHandle<Event> OnEventDispatch;
        // �����������ͺͷ��������������������һ��ӳ��
        Dictionary<InputEventHandle<IInputEventBase>, InputEventHandle<Event>> m_ListenerHash = new Dictionary<InputEventHandle<IInputEventBase>, InputEventHandle<Event>>();

        InputEventHandle<Event> m_handle;
        string m_eventKey;

        public override void AddListener(string eventKey, InputEventHandle<IInputEventBase> callBack)
        {
            InputEventHandle<Event> temp = (inputEvent) =>
            {
                callBack((IInputEventBase)inputEvent);
            };
            m_ListenerHash.Add(callBack, temp);
            AddListener(eventKey, temp);
        }
        public override void RemoveListener(string eventKey, InputEventHandle<IInputEventBase> callBack)
        {
            if (!m_ListenerHash.ContainsKey(callBack))
            {
                throw new Exception("RemoveListener Exception: dont find Listener Hash ! eventKey: ->" + eventKey + "<-");
            }
            InputEventHandle<Event> temp = m_ListenerHash[callBack];
            m_ListenerHash.Remove(callBack);
            RemoveListener(eventKey, temp);
        }

        public override void Dispatch(IInputEventBase inputEvent)
        {
            Dispatch((Event)inputEvent);
        }

        public void AddListener(string eventKey, InputEventHandle<Event> callBack)
        {
            if (!m_Listeners.ContainsKey(eventKey))
            {
                m_Listeners.Add(eventKey, callBack);
            }
            else
            {
                m_Listeners[eventKey] += callBack;
            }
        }

        public void RemoveListener(string eventKey, InputEventHandle<Event> callBack)
        {
            if (m_Listeners.ContainsKey(eventKey))
            {
                m_Listeners[eventKey] -= callBack;
            }
        }

        public void Dispatch(Event inputEvent)
        {
            m_eventKey = inputEvent.EventKey;
            if (m_Listeners.TryGetValue(m_eventKey, out m_handle))
            {
                DispatchSingleEvent(inputEvent, m_handle);
            }
            // �����¼��ɷ�ʱ����
            DispatchSingleEvent(inputEvent, OnEventDispatch);
            // �����¼��ɷ�ʱ������
            AllEventDispatch(m_eventKey, inputEvent);
        }

        void DispatchSingleEvent(Event inputEvent, InputEventHandle<Event> callBack)
        {
            if (callBack != null)
            {
                try
                {
                    callBack(inputEvent);
                }
                catch (Exception e)
                {
                    Debug.LogError("DispatchSingleEvent Name: " + typeof(Event).ToString() + " key: " + inputEvent.EventKey + " Exception: " + e.ToString());
                }
            }
        }
    }
}