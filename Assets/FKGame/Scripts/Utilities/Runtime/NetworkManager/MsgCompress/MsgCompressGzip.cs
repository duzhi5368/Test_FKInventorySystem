namespace FKGame
{
    public class MsgCompressGzip : IMsgCompressBase
    {
        public override byte[] CompressBytes(byte[] data)
        {
            return ZipUtils.CompressBytes(data);
        }

        public override byte[] DecompressBytes(byte[] data)
        {
            return ZipUtils.DecompressBytes(data);
        }

        public override string GetCompressType()
        {
            return "gzip";
        }
    }
}