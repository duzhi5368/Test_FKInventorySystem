using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RSAService
    {
        // ���ݹ�Կ����
        static public string encriptByPublicKey(string originalString, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string res = "";
            try
            {
                rsa.FromXmlString(publicKey);
                byte[] tempByte = rsa.Encrypt(Encoding.UTF8.GetBytes(originalString), false);
                res = Convert.ToBase64String(tempByte);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return res;
        }

        // ����˽Կ����
        static public string decriptByPrivateKey(string encriptedString, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            byte[] tempByte = Convert.FromBase64String(encriptedString);
            return Encoding.UTF8.GetString(rsa.Decrypt(tempByte, false));
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        // ��֤ǩ��
        public static bool VerifySignedHash(string str_DataToVerify, string str_SignedData, string str_Public_Key)
        {
            byte[] SignedData = Convert.FromBase64String(str_SignedData);
            UTF8Encoding ByteConverter = new UTF8Encoding();
            byte[] DataToVerify = ByteConverter.GetBytes(str_DataToVerify);
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.FromXmlString(str_Public_Key);
                return RSAalg.VerifyData(DataToVerify, new MD5CryptoServiceProvider(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}