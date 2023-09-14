using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public struct MsgPackest
    {
        public const byte ProtocolVersion = 0;      // ��ǰ��Ϣ�汾

        public byte protocolVer;                    // Э��汾
        public byte msgProperty;                    // ��Ϣ����
        public byte isEncryption;                   // �Ƿ���� 0:�����ܣ�1����
        public byte isCompress;                     // �Ƿ�ѹ�� 0����ѹ����1ѹ��
        public byte compressType;                   // ѹ������

        public byte[] contents;                     // ��Ϣ����
        public Session session;
        private NetDataReader reader;
        internal ByteOrder byteOrder;

        internal MsgPackest(ByteOrder byteOrder, Session session, byte[] data)
        {
            if (data.Length < 5)
                throw new Exception("��ϢData û����������Ϣͷ");
            this.session = session;
            this.byteOrder = byteOrder;
            reader = new NetDataReader(byteOrder);

            reader.SetSource(data, 0);
            protocolVer = reader.GetByte();
            msgProperty = reader.GetByte();
            isEncryption = reader.GetByte();
            isCompress = reader.GetByte();
            compressType = reader.GetByte();
            contents = new byte[data.Length - 5];
            reader.GetBytes(contents, data.Length - 5);

            if (protocolVer != ProtocolVersion)
            {
                throw new Exception("��Ϣ�汾��һ�£�" + protocolVer + " Loacal:" + ProtocolVersion);
            }
        }

        internal static byte[] Write2Bytes(ByteOrder byteOrder, byte isEncryption, byte isCompress, byte compressType, byte msgProperty, byte[] contents)
        {
            NetDataWriter writer = new NetDataWriter(byteOrder);
            writer.Reset();

            writer.Put(ProtocolVersion);
            writer.Put(msgProperty);
            writer.Put(isEncryption);
            writer.Put(isCompress);
            writer.Put(compressType);
            writer.PutByteSource(contents);
            return writer.CopyData();
        }
    }
}