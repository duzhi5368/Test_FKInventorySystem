namespace FKGame
{
    // ��Ϣ����,����
    public abstract class IMsgEncryptionBase
    {
        public int SecurityLevel { get; private set; }
        public IMsgEncryptionBase(int securityLevel)
        {
            SecurityLevel = securityLevel;
        }
        public virtual void Init(ByteOrder byteOrder) { }
        // ����
        public abstract byte[] Encryption(Session session, byte[] datas);
        // ����
        public abstract byte[] Decryption(Session session, byte[] datas);
        public virtual void Release() { }
    }
}