using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class IInputEventBase
    {
        public float m_t = 0;       // �¼�������ʱ�䣬Ϊ��ѹ�����л��ı��Ĵ�С������ʹ��t��Ϊ����

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

        // ���л���ʹһ�������¼���ɿɱ�����ı�
        public virtual string Serialize()
        {
            return Serializer.Serialize(this);
        }

        // ��������һ���ı���������¼�������
        public IInputEventBase Analysis(string data)
        {
            return JsonUtility.FromJson<IInputEventBase>(data);
        }
    }
}