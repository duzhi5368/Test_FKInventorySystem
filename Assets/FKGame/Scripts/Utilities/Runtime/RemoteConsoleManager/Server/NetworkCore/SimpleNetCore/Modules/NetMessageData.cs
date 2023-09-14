namespace FKGame
{
    public class NetMessageData
    {
        public object msgType { get; private set; }
        public Session session { get; private set; }
        public object msgData { get; private set; }
        public NetMessageData(object msgType, Session session, object msgData)
        {
            this.msgType = msgType;
            this.session = session;
            this.msgData = msgData;
        }

        public T GetMessage<T>() where T : new()
        {
            if (msgData != null)
                return (T)msgData;
            return default(T);
        }
    }
}