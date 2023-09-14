using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ֧��ƽ̨��Ϣ
    public class PayPlatformInfo
    {
        public string SDKName = "";             // ��ʹ�õ�SDK
        public string payPlatformTag = "";      // ����֧��ƽ̨Tag

        public PayPlatformInfo()
        {
            SDKName = "";
            payPlatformTag = "";
        }

        public PayPlatformInfo(string sDKKey, string payPlatformTag)
        {
            SDKName = sDKKey;
            this.payPlatformTag = payPlatformTag;
        }

        // ʹ������SDKInterface ��ֵ���й��졣��ʽ��Payssion@gash_HK  ( SDKKey = Payssion, payPlatformTag = gash_HK)
        public PayPlatformInfo(string valueFromSDKInterface)
        {
            if (string.IsNullOrEmpty(valueFromSDKInterface))
            {
                Debug.LogError("PayPlatformInfo init error :" + valueFromSDKInterface);
                return;
            }
            else
            {
                string[] result = valueFromSDKInterface.Split('@');
                if (result.Length > 0)
                {
                    SDKName = result[0];
                }
                if (result.Length > 1)
                {
                    SDKName = result[0];
                    payPlatformTag = result[1];
                }
            }
        }

        // ������sdkinterface ��֧��ƽ̨��Ϣ��������ࡣ��ʽ���磺 GooglePay|Payssion@gash_tw|Payssion@gash_HK
        public static List<PayPlatformInfo> GetAllPayPlatform(string valueFromSDKInterface)
        {
            List<PayPlatformInfo> result = new List<PayPlatformInfo>();
            if (string.IsNullOrEmpty(valueFromSDKInterface))
            {
                Debug.LogError("GetAllPayPlatform valueFromSDKInterface is null");
            }
            else
            {
                string[] values = valueFromSDKInterface.Split('|');
                for (int i = 0; i < values.Length; i++)
                {
                    PayPlatformInfo payPlatformInfo = new PayPlatformInfo(values[i]);
                    result.Add(payPlatformInfo);
                }
            }
            return result;
        }
    }
}