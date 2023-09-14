using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class IInputEventBase
    {
        public float m_t = 0;       // 事件产生的时间，为了压缩序列化文本的大小，这里使用t作为名称

        protected string m_eventKey;
        public string EventKey
        {
            get
            {
                if (m_eventKey == null)
                {
                    m_eventKey = GetEventKey();
                }
                return m_eventKey;
            }
            set { m_eventKey = value; }
        }

        public IInputEventBase()
        {
            Reset();
        }

        public virtual void Reset()
        {
            m_eventKey = null;
            m_t = DevelopReplayManager.CurrentTime;
        }

        protected virtual string GetEventKey()
        {
            if (m_eventKey == null)
            {
                m_eventKey = ToString();
            }
            return m_eventKey;
        }

        // 序列化，使一个输入事件变成可保存的文本
        public virtual string Serialize()
        {
            return Serializer.Serialize(this);
        }

        // 解析，将一个文本变成输入事件的数据
        public IInputEventBase Analysis(string data)
        {
            return JsonUtility.FromJson<IInputEventBase>(data);
        }
    }
}