using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // Json消息类的接受转换和发送
    public class JsonMessageProcessingController
    {
        public const string ErrorCodeMessage = "ErrorCodeMessage";
        static Dictionary<string, object> mesDic = new Dictionary<string, object>();
        public static void Init()
        {
            InputManager.AddAllEventListener<InputNetworkMessageEvent>(MessageReceiveCallBack);
        }

        private static void MessageReceiveCallBack(InputNetworkMessageEvent inputEvent)
        {
            // 心跳包
            if (inputEvent.m_MessgaeType == "HB")
                return;
            if (Debug.unityLogger.logEnabled)
                Debug.Log("MessageReceiveCallBack;" + JsonSerializer.ToJson(inputEvent));

            System.Type type = System.Type.GetType(inputEvent.m_MessgaeType);

            if (type == null)
            {
                Debug.LogError("No MessgaeType :" + inputEvent.m_MessgaeType);
                return;
            }

            object dataObj = JsonSerializer.FromJson(type, inputEvent.Data["Content"].ToString());
            IMessageClass msgInterface = (IMessageClass)dataObj;
            msgInterface.DispatchMessage();

            if (msgInterface is CodeMessageBase)
            {
                CodeMessageBase codeMsg = (CodeMessageBase)msgInterface;
                GlobalEvent.DispatchEvent(ErrorCodeMessage, codeMsg);
            }
        }

        public static void SendMessage<T>(T data)
        {
            string mt = typeof(T).Name;
            string content = JsonSerializer.ToJson(data);
            SendMessage(mt, content);
        }

        public static void SendMessage(string mt, string content)
        {
            mesDic.Clear();
            Debug.Log("SendMessage : MT:" + mt + " msg :" + content);
            mesDic.Add("Content", content);
            NetworkManager.SendMessage(mt, mesDic);
        }
    }
}