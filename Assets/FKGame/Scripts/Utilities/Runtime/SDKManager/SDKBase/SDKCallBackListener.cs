using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SDKCallBackListener : MonoBehaviour
    {
        Deserializer deserializer = new Deserializer();
        public CallBack<Dictionary<string, string>> sdkCallBack;

        public void OnSDKCallBack(string str)
        {
            Dictionary<string, string> data = deserializer.Deserialize<Dictionary<string, string>>(str);
            sdkCallBack(data);
        }
    }
}