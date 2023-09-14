using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 重发消息管理器
    public class ResendMessageManager
    {
        private const string ResendMsgFile = "ResendMsgFile";
        public static float resendTime = 2f;
        public static bool startResend = false;     // 是否开始重发，一般是登陆后
        public static Action<IMessageClass> ReceiveMsgCallBack;
        private static User user;
        private static float tempResendTime = 0;
        private static List<ResendMessage> msgs = new List<ResendMessage>();

        public static void Init()
        {
            ApplicationManager.s_OnApplicationUpdate += Update;
            InputManager.AddAllEventListener<InputNetworkMessageEvent>(MessageReceiveCallBack);
            LoginGameController.OnUserLogin += OnUserLogin;
        }
        private static void OnUserLogin(UserLogin2Client t)
        {
            if (t.code != 0)
                return;
            user = t.user;
            LoadRecord();
        }

        private static void MessageReceiveCallBack(InputNetworkMessageEvent inputEvent)
        {
            if (msgs == null || msgs.Count == 0)
                return;
            foreach (ResendMessage m in msgs)
            {
                if (m.removeMT == inputEvent.m_MessgaeType)
                {
                    Debug.Log("移除重发：" + m.removeMT);
                    msgs.Remove(m);
                    SaveRecord();
                    if (ReceiveMsgCallBack != null)
                    {
                        IMessageClass msgInterface = null;
                        Type type = Type.GetType(inputEvent.m_MessgaeType);
                        if (type == null)
                        {
                            Debug.LogError("No MessgaeType :" + inputEvent.m_MessgaeType);
                        }
                        else
                        {
                            object dataObj = JsonSerializer.FromJson(type, inputEvent.Data["Content"].ToString());
                            msgInterface = (IMessageClass)dataObj;
                        }
                        ReceiveMsgCallBack(msgInterface);
                    }
                    break;
                }
            }
        }

        private static void LoadRecord()
        {
            if (user == null)
                return;
            msgs.Clear();
            List<ResendMessage> list = GetData<ResendMessage>(user.userID);
            if (list != null)
            {
                if (msgs != null)
                {
                    msgs.AddRange(list);
                }
                else
                {
                    msgs = list;
                }
            }
            Debug.Log("加载重发记录：" + msgs.Count);
        }

        private static List<T> GetData<T>(string key)
        {
            string res = RecordManager.GetStringRecord(ResendMsgFile, key, "");
            if (string.IsNullOrEmpty(res))
                return null;
            List<T> msgs = JsonSerializer.FromJson<List<T>>(res);
            return msgs;
        }

        private static void SaveRecord()
        {
            if (user == null)
                return;
            String json = JsonSerializer.ToJson(msgs);
            RecordManager.SaveRecord(ResendMsgFile, user.userID, json);
            Debug.Log("保持重发记录:" + msgs.Count);
        }

        private static void Update()
        {
            if (msgs == null)
            {
                msgs = new List<ResendMessage>();
            }
            if (msgs.Count == 0)
                return;
            if (!startResend)
                return;
            if (tempResendTime > 0)
            {
                tempResendTime -= Time.deltaTime;
                return;
            }
            tempResendTime = resendTime;
            foreach (ResendMessage m in msgs)
            {
                if (m.noSend)
                    continue;
                JsonMessageProcessingController.SendMessage(m.mt, m.content);
            }
        }

        public static void AddResendMessage<T>(T data, string removeMT, bool noSend = false)
        {
            string content = JsonSerializer.ToJson(data);
            string mt = typeof(T).FullName;
            ResendMessage msgResnd = null;
            foreach (ResendMessage m in msgs)
            {
                if (m.content == content)
                {
                    msgResnd = m;
                    break;
                }
            }
            if (msgResnd != null)
            {
                msgResnd.removeMT = removeMT;
                msgResnd.content = content;
                msgResnd.noSend = noSend;
            }
            else
            {
                ResendMessage msg = new ResendMessage(removeMT, mt, content, noSend);
                msgs.Add(msg);
            }
            SaveRecord();
        }
    }
}