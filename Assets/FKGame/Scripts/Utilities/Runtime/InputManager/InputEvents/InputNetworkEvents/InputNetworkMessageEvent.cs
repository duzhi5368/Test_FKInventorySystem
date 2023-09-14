using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class InputNetworkMessageEvent : IInputEventBase
    {
        public string m_MessgaeType = "";           // 消息类型
        public string m_content = "";               // 消息内容
        Dictionary<string, object> m_data = null;   // 消息数据

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