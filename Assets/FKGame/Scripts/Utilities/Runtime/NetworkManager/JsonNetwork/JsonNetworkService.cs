using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{

    public class JsonNetworkService : INetwork
    {
        public int m_msgCode = 0;
        public const char c_endChar = '&';                              // ��Ϣ��β��
        public const string c_endCharReplaceString = "<FCP:AND>";       // �ı�������н�β�����滻�����
        private Queue<string> mesQueue = new Queue<string>();           // ��Ϣ����
        private StringBuilder m_buffer = new StringBuilder();

        public override void Connect()
        {
            EncryptionService.Init();
            m_msgCode = 0;
            m_buffer = new StringBuilder();
            base.Connect();
        }

        public override void SpiltMessage(byte[] data, ref int offset, int length)
        {
            lock (mesQueue)
            {
                mesQueue.Enqueue(Encoding.UTF8.GetString(data, offset, length));
                DealMessage(mesQueue.Dequeue());
                offset = 0;
            }
        }

        // ������Ϣ
        public override void SendMessage(string MessageType, Dictionary<string, object> data)
        {
            try
            {
                if (!data.ContainsKey("MT"))
                {
                    data.Add("MT", MessageType);
                }
                string mes = MiniJSON.Serialize(data);
                SendMessage(MessageType, mes);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString() + " MT:" + MessageType);
            }
        }

        public override void SendMessage(string MessageType, string content)
        {
            try
            {
                content = content.Replace(c_endChar.ToString(), c_endCharReplaceString);
                try
                {
                    if (msgCompress != null)
                        content = msgCompress.CompressString(content);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                // ����
                if (MessageType != IHeartBeatBase.c_HeartBeatMT && EncryptionService.IsSecret)
                {
                    content = EncryptionService.Encrypt(content);
                }
                byte[] bytes = Encoding.UTF8.GetBytes(content + c_endChar);
                m_socketService.Send(bytes);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public void DealMessage(string s)
        {
            lock (m_buffer)
            {
                bool isEnd = false;
                if (s.Substring(s.Length - 1, 1) == c_endChar.ToString())
                {
                    isEnd = true;
                }
                m_buffer.Append(s);
                string buffer = m_buffer.ToString();
                m_buffer.Remove(0, m_buffer.Length);
                string[] str = buffer.Split(c_endChar);
                for (int i = 0; i < str.Length; i++)
                {
                    if (i != str.Length - 1)
                    {
                        CallBack(str[i]);
                    }
                    else
                    {
                        if (isEnd)
                        {
                            CallBack(str[i]);
                        }
                        else
                        {
                            m_buffer.Append(str[i]);
                        }
                    }
                }
            }
        }

        public void CallBack(string s)
        {
            try
            {
                if (s != null && s != "")
                {
                    if (msgCompress != null)
                    {
                        try
                        {
                            s = msgCompress.DecompressString(s);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }

                    // ����
                    if (EncryptionService.IsSecret)
                    {
                        s = EncryptionService.Decrypt(s);
                    }
                    NetworkMessage msg = new NetworkMessage();
                    s = s.Replace(c_endCharReplaceString, c_endChar.ToString());
                    Dictionary<string, object> data = MiniJSON.Deserialize(s) as Dictionary<string, object>;
                    msg.m_data = data;
                    msg.m_MessageType = data["MT"].ToString();
                    if (data.ContainsKey("MsgCode"))
                    {
                        msg.m_MsgCode = int.Parse(data["MsgCode"].ToString());

                        if (m_msgCode != msg.m_MsgCode)
                        {
                            Debug.LogError("MsgCode error currentCode " + m_msgCode + " server code " + msg.m_MsgCode);
                            if (msg.m_MsgCode > m_msgCode)
                            {
                                m_msgCode = msg.m_MsgCode;
                                m_msgCode++;
                                m_messageCallBack(msg);
                            }
                        }
                        else
                        {
                            m_msgCode++;
                            m_messageCallBack(msg);
                        }
                    }
                    else
                    {
                        m_messageCallBack(msg);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Message error ->" + s + "<-\n" + e.ToString());
            }
        }
    }
}