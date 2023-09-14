using System.Collections.Generic;
using System.Text;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RemoteDeviceInfo
    {
        public string appName;
        public string appVersion;
        public string deviceName;       // eg.Tom's iPhone
        public string deviceModel;      // eg. iPhone 7,1
        public RuntimePlatform platform;
        public Dictionary<string, string> otherData = new Dictionary<string, string>();

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(appName);
            builder.Append(appVersion);
            builder.Append(deviceName);
            builder.Append(deviceModel);
            builder.Append(platform);
            return builder.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is RemoteDeviceInfo)
            {
                RemoteDeviceInfo other = (RemoteDeviceInfo)obj;
                if (appName == other.appName &&
                    appVersion == other.appVersion &&
                    deviceName == other.deviceName &&
                    deviceModel == other.deviceModel &&
                    platform == other.platform)
                    return true;
            }
            return false;
        }

        public static RemoteDeviceInfo GetLocalDeviceInfo()
        {
            RemoteDeviceInfo deviceInfo = new RemoteDeviceInfo();
            deviceInfo.appName = Application.productName;
            deviceInfo.appVersion = Application.version;
            deviceInfo.deviceModel = SystemInfo.deviceModel;
            deviceInfo.deviceName = SystemInfo.deviceName;
            deviceInfo.platform = Application.platform;
            return deviceInfo;
        }
    }
}