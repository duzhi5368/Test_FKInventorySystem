using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class InputNetworkEventProxy : IInputProxyBase
    {
        const int c_msgPoolSize = 5;    //������Ϣ�ش�С
        const int c_connectMsgPool = 2; //���������¼��ش�С
        static InputNetworkMessageEvent[] m_msgPool;
        static InputNetworkConnectStatusEvent[] m_connectMsgPool;
        static int m_connectIndex = 0;
        static int m_msgIndex = 0;
        static bool isInit = false;

        public static void DispatchStatusEvent(NetworkState status)
        {
            //ֻ����������ʱ���ɷ��¼�
            if (IsActive)
            {
                InitPool();
                InputNetworkConnectStatusEvent e = GetConnectMsgEvent(status);
                InputManager.Dispatch("InputNetworkConnectStatusEvent", e);
            }
        }

        public static void DispatchMessageEvent(string massageType, Dictionary<string, object> data)
        {
            //ֻ����������ʱ���ɷ��¼�
            if (IsActive)
            {
                InitPool();
                InputNetworkMessageEvent e = GetMsgEvent();

                e.m_MessgaeType = massageType;
                e.Data = data;
                InputManager.Dispatch("InputNetworkMessageEvent", e);
            }
        }

        static void InitPool()
        {
            if (!isInit)
            {
                isInit = true;
                m_connectMsgPool = new InputNetworkConnectStatusEvent[c_connectMsgPool];
                for (int i = 0; i < c_connectMsgPool; i++)
                {
                    m_connectMsgPool[i] = new InputNetworkConnectStatusEvent();
                }

                m_msgPool = new InputNetworkMessageEvent[c_msgPoolSize];
                for (int i = 0; i < c_msgPoolSize; i++)
                {
                    m_msgPool[i] = new InputNetworkMessageEvent();
                }
            }
        }

        static InputNetworkMessageEvent GetMsgEvent()
        {
            InputNetworkMessageEvent msg = m_msgPool[m_msgIndex];
            msg.Reset();

            m_msgIndex++;
            if (m_msgIndex >= m_msgPool.Length)
            {
                m_msgIndex = 0;
            }
            return msg;
        }

        static InputNetworkConnectStatusEvent GetConnectMsgEvent(NetworkState status)
        {
            InputNetworkConnectStatusEvent msg = m_connectMsgPool[m_connectIndex];
            msg.Reset();
            msg.m_status = status;

            m_connectIndex++;
            if (m_connectIndex >= m_connectMsgPool.Length)
            {
                m_connectIndex = 0;
            }
            return msg;
        }
    }
}