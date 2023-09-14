using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 支付平台信息
    public class PayPlatformInfo
    {
        public string SDKName = "";             // 所使用的SDK
        public string payPlatformTag = "";      // 具体支付平台Tag

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

        // 使用来自SDKInterface 的值进行构造。格式：Payssion@gash_HK  ( SDKKey = Payssion, payPlatformTag = gash_HK)
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

        // 将来自sdkinterface 的支付平台信息，构造成类。格式例如： GooglePay|Payssion@gash_tw|Payssion@gash_HK
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