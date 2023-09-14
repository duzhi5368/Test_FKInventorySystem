using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class EncryptionService : MonoBehaviour
    {
        public const string c_EncryptionConfig = "EncryptionConfig";
        public const string c_publickey = "publickey";
        public const string c_isSecretKey = "isSecret";
        private static bool isInit = false;
        private static string publickey = "";
        private static bool isSecret = false;
        public static bool IsSecret
        {
            get
            {
                Init();
                return isSecret;
            }
            set
            {
                isSecret = value;
            }
        }

        //���ܷ���  
        public static string Encrypt(string msg)
        {
            //�������key
            string key = Random.Range(10000000, 99999999).ToString().Substring(0, 8);
            //��key���ܱ���
            string securityData = DESService.Encode(msg, key);
            //����key
            string securityKey = RSAService.encriptByPublicKey(key, publickey);
            //����string
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("securityKey", securityKey);
            data.Add("securityData", securityData);
            return MiniJSON.Serialize(data);
        }

        //���ܷ���  
        public static string Decrypt(string msg)
        {
            //����json
            Dictionary<string, object> json = (Dictionary<string, object>)MiniJSON.Deserialize(msg);
            //���δ����ֱ�ӷ���
            if (!json.ContainsKey("sign"))
            {
                return msg;
            }

            string sign = (string)json["sign"];
            string conetnt = (string)json["msg"];
            //��֤ǩ��
            if (RSAService.VerifySignedHash(conetnt, sign, publickey))
            {
                return conetnt;
            }
            else
            {
                throw new System.Exception("ǩ����֤ʧ�� " + msg);
            }
        }

        public static void Init()
        {
            if (!isInit)
            {
                isInit = true;

                if (ConfigManager.GetIsExistConfig(c_EncryptionConfig))
                {
                    isSecret = ConfigManager.GetData(c_EncryptionConfig, c_isSecretKey).GetBool();
                    publickey = ConfigManager.GetData(c_EncryptionConfig, c_publickey).GetString();
                }
                else
                {
                    isSecret = false;
                }
            }
        }
    }
}