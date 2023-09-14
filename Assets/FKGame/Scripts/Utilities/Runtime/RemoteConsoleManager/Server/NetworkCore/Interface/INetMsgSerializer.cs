namespace FKGame
{
    public interface INetMsgSerializer
    {
        void Init(NetConfiguration configuration);
        // ��ȡ��Ϣ����
        object GetMsgType(object data);
        // ��Ϣ���л�
        byte[] Serialize(object msgType, object data);
        // ��Ϣ�����л�
        object Deserialize(byte[] datas, out object msgType);
    }
}