using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 日志输出线程
    public class LogOutPutThread
    {
        public const string LogPath = "Log";
        public const string expandName = "txt";
        private StreamWriter mLogWriter = null;

        public void Init()
        {
            try
            {
                string prefix = Application.productName;
#if UNITY_EDITOR
                prefix += "_Editor_" + SystemInfo.deviceName;
#else
    #if UNITY_ANDROID
                    prefix += "_Android_" + SystemInfo.deviceUniqueIdentifier;
    #else
                    prefix += "_Ios_" + SystemInfo.deviceName;
    #endif
#endif
                DateTime now = DateTime.Now;
                string logName = string.Format(prefix + "_{0}{1:D2}{2:D2}#{3:D2}_{4:D2}_{5:D2}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                string logPath = PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(LogPath, logName, expandName));
                UpLoadLogic(logPath);

                if (File.Exists(logPath))
                    File.Delete(logPath);
                string logDir = Path.GetDirectoryName(logPath);

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                mLogWriter = new StreamWriter(logPath);
                mLogWriter.AutoFlush = true;
            }
            catch (Exception e)
            {
                Debug.LogError("LogOutPutThread Init Exception:" + e.ToString());
            }
        }

        public void Log(LogInfo log)
        {
            if (log.m_logType == LogType.Error
                                || log.m_logType == LogType.Exception
                                || log.m_logType == LogType.Assert
                                )
            {
                this.mLogWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                this.mLogWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + log.m_logContent + "\n");
                this.mLogWriter.WriteLine(log.m_logTrack);
                this.mLogWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            }
            else
            {
                this.mLogWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + log.m_logContent);
            }
        }

        public void Close()
        {
            ExitLogic();
            this.mLogWriter.Close();
        }

        public void Pause()
        {
            ExitLogic();
        }

        public void UpLoadLogic(string logPath)
        {
        }

        public void ExitLogic()
        {
        }

        public static string[] GetLogFileNameList()
        {
            FileTool.CreatPath(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, LogPath));
            List<string> relpayFileNames = new List<string>();
            string[] allFileName = Directory.GetFiles(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, LogPath));
            foreach (var item in allFileName)
            {
                if (item.EndsWith(".txt"))
                {
                    string configName = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(item));
                    relpayFileNames.Add(configName);
                }
            }
            return relpayFileNames.ToArray() ?? new string[0];
        }

        public static string LoadLogContent(string logName)
        {
            return ResourceIOTool.ReadStringByFile(GetPath(logName));
        }

        public static string GetPath(string logName)
        {
            return PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(LogPath, logName, expandName));
        }
    }
}