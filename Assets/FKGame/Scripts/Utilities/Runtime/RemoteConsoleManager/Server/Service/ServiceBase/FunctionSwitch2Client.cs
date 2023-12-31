namespace FKGame
{
    public struct FunctionSwitch2Client : INetSerializable
    {
        public string functionName;
        public bool isOpenFunction;

        public void Deserialize(NetDataReader reader)
        {
            functionName = reader.GetString();
            isOpenFunction = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(functionName);
            writer.Put(isOpenFunction);
        }
    }
}