using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��ȡ�豸ΨһID
    public class DeviceUniqueIdentifierHandle
    {
        private const string DUID = "deviceUniqueIdentifier";
        public static string GetUniqueIdentifier()
        {
            string id = SystemInfo.deviceUniqueIdentifier;

#if UNITY_IPHONE || UNITY_IOS
            id = Keychain.GetValue(DUID);
            if (string.IsNullOrEmpty(id))
            {
                id = SystemInfo.deviceUniqueIdentifier;
                Keychain.SetValue(DUID, id);
            }
#endif
            return id;
        }
    }
}