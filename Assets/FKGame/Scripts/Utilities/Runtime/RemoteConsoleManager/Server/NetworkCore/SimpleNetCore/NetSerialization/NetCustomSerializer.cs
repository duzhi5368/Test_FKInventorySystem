using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    // ����һ����������Ϊ��Ϣͷ��ʵ��INetSerializable���Զ�����Ϣ���л���
    public class NetCustomSerializer : INetMsgSerializer
    {
        private Dictionary<string, System.Type> typeDic = new Dictionary<string, System.Type>();
        private ByteOrder byteOrder;
        public void Init(NetConfiguration configuration)
        {
            byteOrder = configuration.byteOrder;
            System.Type[] types = ReflectionTool.FastGetChildTypes(typeof(INetSerializable));
            foreach (var t in types)
            {
                typeDic.Add(t.Name, t);
            }
        }

        public object Deserialize(byte[] datas, out object msgType)
        {
            NetDataReader reader = new NetDataReader(byteOrder);
            reader.SetSource(datas, 0);
            msgType = reader.GetString();
            string msgT = msgType.ToString();
            if (!typeDic.ContainsKey(msgT))
            {
                NetDebug.LogError("No msgType:" + msgType);
                return null;
            }
            System.Type type = typeDic[msgT];
            INetSerializable serializable = (INetSerializable)ReflectionTool.CreateDefultInstance(type);
            serializable.Deserialize(reader);
            return serializable;
        }

        public object GetMsgType(object data)
        {
            return data.GetType().Name;
        }

        public byte[] Serialize(object msgType, object data)
        {
            NetDataWriter writer = new NetDataWriter(byteOrder);
            writer.Reset();
            writer.PutValue(msgType);
            INetSerializable serializable = data as INetSerializable;

            if (serializable != null)
            {
                serializable.Serialize(writer);
            }
            else
            {
                NetDebug.LogError("cant change INetSerializable");
            }
            return writer.CopyData();
        }
    }
}