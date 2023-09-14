namespace FKGame
{
    public interface INetMsgSerializer
    {
        void Init(NetConfiguration configuration);
        // 获取消息类型
        object GetMsgType(object data);
        // 消息序列化
        byte[] Serialize(object msgType, object data);
        // 消息反序列化
        object Deserialize(byte[] datas, out object msgType);
    }
}