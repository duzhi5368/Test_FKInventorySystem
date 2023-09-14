namespace FKGame.Telepathy
{
    public struct Message
    {
        public readonly int connectionId;
        public readonly EventType eventType;
        public readonly byte[] data;
        public Message(int connectionId, EventType eventType, byte[] data)
        {
            this.connectionId = connectionId;
            this.eventType = eventType;
            this.data = data;
        }
    }
}