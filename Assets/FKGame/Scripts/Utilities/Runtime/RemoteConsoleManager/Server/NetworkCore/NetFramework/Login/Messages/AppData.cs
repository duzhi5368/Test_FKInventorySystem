namespace FKGame
{
    public class AppData : INetSerializable
    {
        public string serverAppName = "";
        public string serverAppVersion = "";
        public string bundleIdentifier = "";

        public void Deserialize(NetDataReader reader)
        {
            reader.TryGetString(out serverAppName);
            reader.TryGetString(out serverAppVersion);
            reader.TryGetString(out bundleIdentifier);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(serverAppName);
            writer.Put(serverAppVersion);
            writer.Put(bundleIdentifier);
        }
    }
}