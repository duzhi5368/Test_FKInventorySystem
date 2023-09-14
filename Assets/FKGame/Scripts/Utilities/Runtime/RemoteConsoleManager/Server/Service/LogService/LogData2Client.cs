using System;
//------------------------------------------------------------------------
namespace FKGame
{
    [Serializable]
    public class LogData2Client : INetSerializable
    {
        public LogData logData = new LogData();
        public void Deserialize(NetDataReader reader)
        {
            logData.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            logData.Serialize(writer);
        }
    }
}