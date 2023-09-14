namespace FKGame
{
    public class ShowInfoData : INetSerializable
    {
        public string typeName;
        public string label;            // АэИз:CPU
        public string key;
        public string valueTypeStr;
        public string value;
        public string discription;

        public void Deserialize(NetDataReader reader)
        {
            typeName = reader.GetString();
            label = reader.GetString();
            key = reader.GetString();
            valueTypeStr = reader.GetString();
            value = reader.GetString();
            discription = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(typeName);
            writer.Put(label);
            writer.Put(key);
            writer.Put(valueTypeStr);
            writer.Put(value);
            writer.Put(discription);
        }
        public string GetPath()
        {
            return typeName + "/" + label + "/" + key;
        }
        public override string ToString()
        {
            return GetPath() + " :" + value;
        }
    }
}