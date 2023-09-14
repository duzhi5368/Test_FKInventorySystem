using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class InputNetworkMessageEvent : IInputEventBase
    {
        public string m_MessgaeType = "";           // ��Ϣ����
        public string m_content = "";               // ��Ϣ����
        Dictionary<string, object> m_data = null;   // ��Ϣ����

        public Dictionary<string, object> Data
        {
            get
            {
                if (m_data == null)
                {
                    if (m_content != null && m_content != "")
                    {
                        m_data = DevelopReplayManager.Deserializer.Deserialize<Dictionary<string, object>>(m_content);
                    }
                    else
                    {
                        m_data = new Dictionary<string, object>();
                    }
                }
                return m_data;
            }
            set
            {
                m_data = value;
            }
        }

        public override void Reset()
        {
            base.Reset();
            m_content = null;
        }

        protected override string GetEventKey()
        {
            return m_MessgaeType;
        }

        public override string Serialize()
        {
            if (m_content == null || m_content == "")
            {
                m_content = Serializer.Serialize(Data);
            }
            return base.Serialize();
        }
    }
}