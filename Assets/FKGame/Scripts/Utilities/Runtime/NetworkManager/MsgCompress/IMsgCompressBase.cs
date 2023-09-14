using System.Text;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class IMsgCompressBase
    {
        private static int compressLimit = 1024 * 5;        // �����ֽڳ���

        // ѹ����ʽ����ʹ��Сд
        public abstract string GetCompressType();

        public string CompressString(string msg)
        {
            string result = null;
            var compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(msg);
            if (compressBeforeByte.Length >= compressLimit)
            {
                byte[] compressAfterByte = CompressBytes(compressBeforeByte);
                string compressString = Convert.ToBase64String(compressAfterByte);
                string length = GetCompressType().Length > 9 ? GetCompressType().Length.ToString() : "0" + GetCompressType().Length;
                result = length + GetCompressType() + compressString;
            }
            else
            {
                result = msg;
            }
            return result;
        }

        public abstract byte[] CompressBytes(byte[] data);

        public string DecompressString(string cMsg)
        {
            string msg = null;
            try
            {
                int length = int.Parse(cMsg.Substring(0, 2));
                msg = cMsg.Substring(2 + length);
            }
            catch (Exception)
            {
                return cMsg;
            }
            var compressBeforeByte = Convert.FromBase64String(msg);
            var compressAfterByte = DecompressBytes(compressBeforeByte);
            string decompressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return decompressString;
        }

        public abstract byte[] DecompressBytes(byte[] data);
    }
}