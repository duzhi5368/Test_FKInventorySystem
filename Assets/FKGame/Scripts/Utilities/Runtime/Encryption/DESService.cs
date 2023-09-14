using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DESService
    {
        // �ַ���DES���ܺ���
        public static string Encode(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                StringBuilder builder = new StringBuilder();
                foreach (byte num in stream.ToArray())
                {
                    builder.AppendFormat("{0:X2}", num);
                }
                stream.Close();
                return builder.ToString();
            }
            catch (Exception) { return "xxxx"; }
        }

        // �ַ���DES���ܺ���  
        public static string Decode(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] buffer = new byte[str.Length / 2];
                for (int i = 0; i < (str.Length / 2); i++)
                {
                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                    buffer[i] = (byte)num2;
                }
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                stream.Close();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception) { return ""; }
        }
    }
}