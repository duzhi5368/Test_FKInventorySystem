namespace FKGame
{
    public abstract class INetworkCoreMsgCompressBase
    {
        public abstract byte CompressType
        {
            get;
        }
        public abstract string CompressTypeName
        {
            get;
        }

        public abstract void Init();
        public abstract byte[] Compress(byte[] datas);
        public abstract byte[] Decompress(byte[] datas);
        public abstract void Release();
    }
}