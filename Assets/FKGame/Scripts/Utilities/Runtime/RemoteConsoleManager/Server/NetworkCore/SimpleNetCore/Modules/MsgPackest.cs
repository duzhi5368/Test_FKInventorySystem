using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public struct MsgPackest
    {
        public const byte ProtocolVersion = 0;      // 当前消息版本

        public byte protocolVer;                    // 协议版本
        public byte msgProperty;                    // 消息类型
        public byte isEncryption;                   // 是否加密 0:不加密，1加密
        public byte isCompress;                     // 是否压缩 0：不压缩，1压缩
        public byte compressType;                   // 压缩类型

        public byte[] contents;                     // 消息内容
        public Session session;
        private NetDataReader reader;
        internal ByteOrder byteOrder;

        internal MsgPackest(ByteOrder byteOrder, Session session, byte[] data)
        {
            if (data.Length < 5)
                throw new Exception("消息Data 没有完整的消息头");
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
                throw new Exception("消息版本不一致：" + protocolVer + " Loacal:" + ProtocolVersion);
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