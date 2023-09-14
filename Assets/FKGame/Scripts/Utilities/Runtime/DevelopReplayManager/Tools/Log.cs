using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class Log
    {
        //日志输出线程
        static LogOutPutThread s_LogOutPutThread = new LogOutPutThread();
        public static void Init(bool isOpenLog = true)
        {
            SetUnityDebugSwitch(isOpenLog);
            if (isOpenLog)
            {
                s_LogOutPutThread.Init();
                ApplicationManager.s_OnApplicationQuit += OnApplicationQuit;
                Application.logMessageReceivedThreaded += UnityLogCallBackThread;
                Application.logMessageReceived += UnityLogCallBack;
            }
        }

        private static void SetUnityDebugSwitch(bool open)
        {
            Debug.unityLogger.logEnabled = open;
        }

        private static void OnApplicationQuit()
        {
            Application.logMessageReceivedThreaded -= UnityLogCallBackThread;
            Application.logMessageReceived -= UnityLogCallBack;
            s_LogOutPutThread.Close();
        }

        static void UnityLogCallBackThread(string log, string track, LogType type)
        {
            LogInfo l_logInfo = new LogInfo
            {
                m_logContent = log,
                m_logTrack = track,
                m_logType = type
            };
            s_LogOutPutThread.Log(l_logInfo);
        }

        static void UnityLogCallBack(string log, string track, LogType type)
        {
        }
    }
}