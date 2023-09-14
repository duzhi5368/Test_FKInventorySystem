using System.Collections.Generic;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    [Serializable]
    public class MethodData : INetSerializable
    {
        public string methodType;
        public string showName;
        public string description;
        public string classFullName;
        public string methodName;
        public List<ParamsData> paramsDatas = new List<ParamsData>();

        public void Deserialize(NetDataReader reader)
        {
            methodType = reader.GetString();
            showName = reader.GetString();
            description = reader.GetString();
            classFullName = reader.GetString();
            methodName = reader.GetString();
            paramsDatas = NetDataReaderExtension.GetListData<ParamsData>(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(methodType);
            writer.Put(showName);
            writer.Put(description);
            writer.Put(classFullName);
            writer.Put(methodName);
            NetDataWriterExtension.PutListData(writer, paramsDatas);
        }
    }
}