using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 自定义消息分发器
    public class MessageDispatcher : IMessageHandler
    {
        protected readonly Dictionary<object, MessageHandlerDelegate> messageHandlers = new Dictionary<object, MessageHandlerDelegate>();
        protected readonly Dictionary<object, MessageHandlerDelegate> noLoginMessageHandlers = new Dictionary<object, MessageHandlerDelegate>();

        public MessageDispatcher(bool isServer) : base(isServer){}

        public override void DispatchMessage(Session session, object msgType, object msgData)
        {
            MessageHandlerDelegate handlerDelegate;
            bool canInvoke = true;
            if (messageHandlers.TryGetValue(msgType, out handlerDelegate))
            {
                if (isServer)
                {
                    if (!PlayerManager.IsLogin(session))
                    {
                        canInvoke = false;
                        Debug.LogError("No Login cant use msg:" + msgType);
                    }
                }
            }
            else if (noLoginMessageHandlers.TryGetValue(msgType, out handlerDelegate))
            {
            }
            else
            {
                canInvoke = false;
            }
            if (canInvoke && handlerDelegate != null)
            {
                NetMessageData messageHandler = new NetMessageData(msgType, session, msgData);
                handlerDelegate.Invoke(messageHandler);
            }
        }

        public override void RegisterMsgEvent<T>(MessageHandlerDelegate handlerDelegate)
        {
            System.Type type = typeof(T);
            string msgType = type.Name;
            bool isNoLoginMsg = typeof(INoLoginMsg).IsAssignableFrom(type);
            Dictionary<object, MessageHandlerDelegate> mapHandler = isNoLoginMsg ? noLoginMessageHandlers : messageHandlers;
            if (mapHandler.ContainsKey(msgType))
                mapHandler[msgType] += handlerDelegate;
            else
            {
                mapHandler.Add(msgType, handlerDelegate);
            }
        }

        public override void UnregisterMsgEvent<T>(MessageHandlerDelegate handlerDelegate)
        {
            System.Type type = typeof(T);
            string msgType = type.Name;
            bool isNoLoginMsg = type.IsAssignableFrom(typeof(INoLoginMsg));
            Dictionary<object, MessageHandlerDelegate> mapHandler = isNoLoginMsg ? noLoginMessageHandlers : messageHandlers;
            if (mapHandler.ContainsKey(msgType))
            {
                mapHandler[msgType] -= handlerDelegate;
            }
            else
            {
                Debug.LogError("No RegisterMessage:" + msgType);
            }
        }
    }
}