using System.Collections.Generic;
using System;
using UnityEngine;

namespace FKGame
{
    public class LogService : CustomServiceBase
    {
        public override string FunctionName
        {
            get
            {
                return "Log";
            }
        }

        private static int indexCounter = 0;
        private static List<LogData> logDatas = new List<LogData>();
        private static Action<LogData> OnLogEvent;

        public static List<LogData> GetLogDatas()
        {
            return logDatas;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LogEventRegister()
        {
            Application.logMessageReceivedThreaded += LogMessageReceived;
        }

        public override void OnStart()
        {
            OnLogEvent += SendAllPlayerLog;
            isOpenFunction = GetSaveDebugState();
            if (IsOpenFunction != GetUnityDebugSwitch())
            {
                SetUnityDebugSwitch(IsOpenFunction);
            }
            msgManager.RegisterMsgEvent<ClearLog2Server>(OnClearLogEvent);
        }

        private void OnClearLogEvent(NetMessageData msgHandler)
        {
            logDatas.Clear();
        }

        private static void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            LogData data = new LogData(indexCounter, type, condition, stackTrace);
            logDatas.Add(data);
            indexCounter++;
            if (OnLogEvent != null)
            {
                OnLogEvent(data);
            }
        }

        protected override void OnPlayerLoginAfter(Player player)
        {
            List<LogData> list = new List<LogData>(logDatas);
            foreach (var data in list)
            {
                SendLog(player, data);
            }
        }
        protected override void OnFunctionClose()
        {
            SetUnityDebugSwitch(false);
            SetSaveDebugState(false);
        }

        protected override void OnFunctionOpen()
        {
            SetUnityDebugSwitch(true);
            SetSaveDebugState(true);
        }

        internal override void OnUpdate(float deltaTime)
        {
            if (GetUnityDebugSwitch() != IsOpenFunction)
            {
                isOpenFunction = GetUnityDebugSwitch();
                Player[] players = PlayerManager.GetAllPlayers();
                foreach (Player player in players)
                {
                    SendSwitchState2Client(player.session);
                }
            }
        }

        private void SendAllPlayerLog(LogData data)
        {
            Player[] players = PlayerManager.GetAllPlayers();
            foreach (Player player in players)
            {
                SendLog(player, data);
            }
        }

        private void SendLog(Player player, LogData data)
        {
            LogData2Client msg = new LogData2Client();
            msg.logData = data;
            netManager.Send(player.session, msg);
        }

        public bool GetSaveDebugState()
        {
            int code = PlayerPrefs.GetInt(FunctionName, -1);
            bool isOpen = false;
            if (code == -1)
            {
                isOpen = GetUnityDebugSwitch();
            }
            else
            {
                isOpen = code == 0 ? false : true;
            }
            return isOpen;
        }

        public void SetSaveDebugState(bool isOpen)
        {
            int code = isOpen ? 1 : 0;
            PlayerPrefs.SetInt(FunctionName, code);
            PlayerPrefs.Save();
        }

        private void SetUnityDebugSwitch(bool open)
        {
            Debug.unityLogger.logEnabled = open;
        }

        private bool GetUnityDebugSwitch()
        {
            return Debug.unityLogger.logEnabled;
        }
    }
}