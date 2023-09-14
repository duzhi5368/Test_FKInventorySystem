using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public struct NetworkMessage
    {
        public string m_MessageType;
        public int m_MsgCode;

        public Dictionary<string, object> m_data;
    }
}