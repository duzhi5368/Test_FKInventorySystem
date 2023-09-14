namespace FKGame
{
    public struct Login2Client : INetSerializable
    {
        public uint code;
        public string playerID;
        public AppData appData;
        public void Deserialize(NetDataReader reader)
        {
            code = reader.GetUInt();
            reader.TryGetString(out playerID);
            appData = new AppData();
            appData.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(code);
            writer.Put(playerID);
            appData.Serialize(writer);
        }
    }
}