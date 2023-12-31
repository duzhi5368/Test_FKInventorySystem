using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class RemoteInvoking_SystemFunction
    {
        [RemoteInvoking(methodType = RemoteInvokingAttribute.MethodType_System, name = "Log", description = "Print a Log")]
        [ParamsDescription(paramName = "content", paramsDescriptionName = "Log content")]
        private static void Log(string content, LogType logType)
        {
            Debug.unityLogger.Log(logType, content);
        }

        [RemoteInvoking(methodType = RemoteInvokingAttribute.MethodType_System, name = "Clear PlayerPrefs", description = "Delete All PlayerPrefs")]
        private static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}