namespace FKGame
{
    // 消息加密,解密
    public abstract class IMsgEncryptionBase
    {
        public int SecurityLevel { get; private set; }
        public IMsgEncryptionBase(int securityLevel)
        {
            SecurityLevel = securityLevel;
        }
        public virtual void Init(ByteOrder byteOrder) { }
        // 加密
        public abstract byte[] Encryption(Session session, byte[] datas);
        // 解密
        public abstract byte[] Decryption(Session session, byte[] datas);
        public virtual void Release() { }
    }
}