using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    enum DevMenuEnum
    {
        MainMenu,
        Replay,
        Log,
        PersistentFile
    }

    public class DevelopReplayManager
    {
        public const string c_eventExpandName = "inputEvent";
        public const string c_randomExpandName = "random";

        const string c_directoryName = "DevelopReplay";
        const string c_recordName = "DevelopReplay";
        const string c_qucikLunchKey = "qucikLunch";
        const string c_eventStreamKey = "EventStream";
        const string c_randomListKey = "RandomList";
        const string c_eventNameKey = "e";
        const string c_serializeInfoKey = "s";

        static string showContent = "";
        static string LogPath = "";
        static string LogName = "";
        static string phonePath = "";
        static string s_warnContent = "";

        public static Deserializer Deserializer = new Deserializer();
        private static DevMenuEnum MenuStatus = DevMenuEnum.MainMenu;

        public static bool s_isProfile = true;
        private static bool isUploadReplay = false;
        private static bool isShowLog = false;
        private static bool isShowPersistentFile = false;
        private static bool s_isOpenWarnPanel = false;

        private static string[] FileNameList = new string[0];
        private static Vector2 scrollPos = Vector2.one;
        private static int margin = 3;
        private static Rect consoleRect = new Rect(margin, margin, Screen.width * 0.2f - margin, Screen.height - 2 * margin);
        private static Rect windowRect = new Rect(Screen.width * 0.2f, Screen.height * 0.05f, Screen.width * 0.6f, Screen.height * 0.9f);
        private static Rect s_warnPanelRect = new Rect(Screen.width * 0.1f, Screen.height * 0.25f, Screen.width * 0.8f, Screen.height * 0.5f);
        private static List<IInputEventBase> s_eventStream;
        private static List<int> s_randomList;
        private static StreamWriter m_EventWriter = null;
        private static StreamWriter m_RandomWriter = null;
        private static CallBack s_warnCallBack;
        public static CallBack s_ProfileGUICallBack;

        private static System.Action s_onLunchCallBack;
        public static System.Action OnLunchCallBack
        {
            get { return s_onLunchCallBack; }
            set { s_onLunchCallBack = value; }
        }

        private static bool s_isReplay = false;
        public static bool IsReplay
        {
            get{ return s_isReplay; }
        }

        private static float s_currentTime = 0;
        public static float CurrentTime
        {
            get { return DevelopReplayManager.s_currentTime; }
        }

        public static void Init(bool isQuickLunch)
        {
#if UNITY_EDITOR
            phonePath = Application.dataPath.Replace("Assets", "Logs") + "/";
#else
            phonePath = "/storage/emulated/0/" + Application.productName + "/";
#endif
            if (isQuickLunch)
            {
                // ����ģʽ�����ֶ�����
                if (!RecordManager.GetData(c_recordName).GetRecord(c_qucikLunchKey, true))
                {
                    isQuickLunch = false;
                }
            }
            if (isQuickLunch)
            {
                ChoseReplayMode(false);
            }
            else
            {
                Time.timeScale = 0;
                ApplicationManager.s_OnApplicationOnGUI += ReplayMenuGUI;
            }
        }

        static void ChoseReplayMode(bool isReplay, string replayFileName = null)
        {
            s_isReplay = isReplay;
            if (s_isReplay)
            {
                LoadReplayFile(replayFileName);
                ApplicationManager.s_OnApplicationUpdate += OnReplayUpdate;
                GUIConsole.onGUICallback += ReplayModeGUI;
                GUIConsole.onGUICloseCallback += ProfileGUI;
                // �����������
                RandomService.SetRandomList(s_randomList);
                // �ر��������룬��֤�ط�����׼ȷ
                IInputProxyBase.IsActive = false;
            }
            else
            {
                Log.Init(true); // ��־��¼����
                ApplicationManager.s_OnApplicationUpdate += OnRecordUpdate;
                GUIConsole.onGUICallback += RecordModeGUI;
                GUIConsole.onGUICloseCallback += ProfileGUI;
                if (ApplicationManager.Instance.m_recordInput)
                {
                    // ��¼�������
                    RandomService.OnRandomCreat += OnGetRandomCallBack;
                    InputManager.OnEveryEventDispatch += OnEveryEventCallBack; //��¼����
                    OpenWriteFileStream(GetReplayFileName()); //�����ļ���
                }
            }

            ApplicationManager.s_OnApplicationOnGUI -= ReplayMenuGUI;
            Time.timeScale = 1;
            try
            {
                s_onLunchCallBack();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public static void OnEveryEventCallBack(string eventName, IInputEventBase inputEvent)
        {
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            tmp.Add(c_eventNameKey, inputEvent.GetType().Name);
            tmp.Add(c_serializeInfoKey, inputEvent.Serialize());
            try
            {
                WriteInputEvent(Serializer.Serialize(tmp));
            }
            catch (Exception e)
            {
                Debug.LogError("Write Dev Log Error! : " + e.ToString());
            }
        }

        public static void OnGetRandomCallBack(int random)
        {
            WriteRandomRecord(random);
        }

        static void OpenWriteFileStream(string fileName)
        {
            try
            {
                string path = PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(c_directoryName, fileName, c_randomExpandName));
                string dirPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                m_RandomWriter = new StreamWriter(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(c_directoryName, fileName, c_randomExpandName)));
                m_RandomWriter.AutoFlush = true;
                m_EventWriter = new StreamWriter(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(c_directoryName, fileName, c_eventExpandName)));
                m_EventWriter.AutoFlush = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        static void WriteRandomRecord(int random)
        {
            if (m_RandomWriter != null)
            {
                m_RandomWriter.WriteLine(random.ToString());
            }
        }

        static void WriteInputEvent(string EventSerializeContent)
        {
            if (m_EventWriter != null)
            {
                m_EventWriter.WriteLine(EventSerializeContent);
            }
        }

        public static void LoadReplayFile(string fileName)
        {
            string eventContent = ResourceIOTool.ReadStringByFile(GetReplayEventFilePath(fileName));
            string randomContent = ResourceIOTool.ReadStringByFile(GetReplayRandomFilePath(fileName));
            LoadEventStream(eventContent.Split('\n'));
            LoadRandomList(randomContent.Split('\n'));
        }

        public static string GetReplayEventFilePath(string fileName)
        {
            return PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(c_directoryName, fileName, c_eventExpandName));
        }

        public static string GetReplayRandomFilePath(string fileName)
        {
            return PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(c_directoryName, fileName, c_randomExpandName));
        }

        static void LoadEventStream(string[] content)
        {
            s_eventStream = new List<IInputEventBase>();
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] != "")
                {
                    Dictionary<string, object> info = (Deserializer.Deserialize<Dictionary<string, object>>(content[i]));
                    IInputEventBase eTmp = (IInputEventBase)Deserializer.Deserialize(Type.GetType(info[c_eventNameKey].ToString()), info[c_serializeInfoKey].ToString());
                    s_eventStream.Add(eTmp);
                }
            }
        }

        static void LoadRandomList(string[] content)
        {
            s_randomList = new List<int>();
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] != "")
                {
                    s_randomList.Add(int.Parse(content[i].ToString()));
                }
            }
        }

        static void SaveScreenShot(string fileName)
        {
            FileTool.CreatFilePath(fileName);
            UnityEngine.ScreenCapture.CaptureScreenshot(fileName);
        }

        static void ReplayMenuGUI()
        {
            GUIUtil.SetGUIStyle();
            GUILayout.Window(1, windowRect, MenuWindow, "Develop Menu");
            if (s_isOpenWarnPanel)
            {
                GUILayout.Window(2, s_warnPanelRect, WarnWindow, "Warning");
            }
        }

        static void MenuWindow(int windowID)
        {
            if (MenuStatus == DevMenuEnum.MainMenu)
            {
                if (GUILayout.Button("��������", GUILayout.ExpandHeight(true)))
                {
                    ChoseReplayMode(false);
                }
                if (GUILayout.Button("����ģʽ", GUILayout.ExpandHeight(true)))
                {
                    MenuStatus = DevMenuEnum.Replay;
                    FileNameList = GetRelpayFileNames();
                }
                if (GUILayout.Button("�鿴��־", GUILayout.ExpandHeight(true)))
                {
                    MenuStatus = DevMenuEnum.Log;
                    FileNameList = LogOutPutThread.GetLogFileNameList();
                }
                if (GUILayout.Button("�鿴�־��ļ�", GUILayout.ExpandHeight(true)))
                {
                    MenuStatus = DevMenuEnum.PersistentFile;
                    FileNameList = PersistentFileManager.GetFileList();
                }
            }
            else if (MenuStatus == DevMenuEnum.Replay)
            {
                ReplayListGUI();
            }
            else if (MenuStatus == DevMenuEnum.Log)
            {
                LogGUI();
            }
            else if (MenuStatus == DevMenuEnum.PersistentFile)
            {
                PersistentFileGUI();
            }
        }

        static void ReplayListGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < FileNameList.Length; i++)
            {
                if (!isUploadReplay)
                {
                    if (GUILayout.Button(FileNameList[i]))
                    {
                        ChoseReplayMode(true, FileNameList[i]);
                    }
                }
                else
                {
                    if (GUILayout.Button("�ϴ� " + FileNameList[i]))
                    {
                        string replayPath = GetReplayEventFilePath(FileNameList[i]);
                        string randomPath = GetReplayRandomFilePath(FileNameList[i]);
                        HTTPTool.Upload_Request_Thread(URLManager.GetURL("ReplayFileUpLoadURL"), replayPath, UploadCallBack);
                        HTTPTool.Upload_Request_Thread(URLManager.GetURL("ReplayFileUpLoadURL"), randomPath, UploadCallBack);
                    }
                }
            }

            GUILayout.EndScrollView();
            if (GUILayout.Button("�����¼"))
            {
                OpenWarnWindow("ȷ��Ҫɾ�����м�¼��", () =>
                {
                    Debug.Log("��ɾ�����м�¼");
                    FileTool.SafeDeleteDirectory(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, c_directoryName));
                    FileNameList = new string[0];
                });
            }
            if (URLManager.GetURL("ReplayFileUpLoadURL") != null)
            {
                if (GUILayout.Button("�ϴ�ģʽ �� " + isUploadReplay))
                {
                    isUploadReplay = !isUploadReplay;
                }
            }
            else
            {
                GUILayout.Label("�ϴ��־�������Ҫ�� URLConfig -> ReplayFileUpLoadURL �����ϴ�Ŀ¼");
            }
            if (GUILayout.Button("�����ϲ�"))
            {
                MenuStatus = DevMenuEnum.MainMenu;
            }
        }

        static void LogGUI()
        {
            if (isShowLog)
            {
                ShowLog();
            }
            else
            {
                ShowLogList();
            }
        }

        static void ShowLogList()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < FileNameList.Length; i++)
            {
                LogName = FileNameList[i];
                if (GUILayout.Button(LogName))
                {
                    isShowLog = true;
                    scrollPos = Vector2.zero;
                    showContent = LogOutPutThread.LoadLogContent(FileNameList[i]);
                    LogPath = LogOutPutThread.GetPath(FileNameList[i]);
                }
            }

            GUILayout.EndScrollView();
            if (GUILayout.Button("���Ƶ��豸"))
            {
                for (int i = 0; i < FileNameList.Length; i++)
                {
                    string name = FileNameList[i];
                    string path = phonePath + name + ".txt";
                    string LogPath = LogOutPutThread.GetPath(name);
                    FileTool.CreatFilePath(path);
                    File.Copy(LogPath, path, true);
                }
                GUIUtil.ShowTips("���Ƴɹ�");
            }
            if (GUILayout.Button("�����־"))
            {
                OpenWarnWindow("ȷ��Ҫɾ��������־��", () =>
                {
                    Debug.Log("��ɾ��������־");
                    FileTool.SafeDeleteDirectory(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, LogOutPutThread.LogPath));
                    FileNameList = new string[0];
                });
            }
            if (GUILayout.Button("�����ϲ�"))
            {
                MenuStatus = DevMenuEnum.MainMenu;
            }
        }

        static void ShowLog()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            try
            {
                GUIUtil.SafeTextArea(showContent);
            }
            catch (Exception e)
            {
                GUILayout.TextArea(e.ToString());
            }

            GUILayout.EndScrollView();
            if (URLManager.GetURL("LogUpLoadURL") != null)
            {
                if (GUILayout.Button("�ϴ���־"))
                {
                    HTTPTool.Upload_Request_Thread(URLManager.GetURL("LogUpLoadURL"), LogPath, UploadCallBack);
                }
            }
            else
            {
                GUILayout.Label("�ϴ���־��Ҫ�� URLConfig -> LogUpLoadURL �����ϴ�Ŀ¼");
            }

#if UNITY_ANDROID
        if (GUILayout.Button("�������豸"))
        {
            try
            {
                string path = phonePath+ LogName + ".txt";
                FileTool.CreatFilePath(path);
                File.Copy(LogPath, path,true);
                GUIUtil.ShowTips("���Ƴɹ�");
            }
            catch (Exception e)
            {
                GUIUtil.ShowTips(e.ToString());
            }
        }
#endif
            if (GUILayout.Button("���Ƶ�������"))
            {
                TextEditor tx = new TextEditor();
                tx.text = showContent;
                tx.OnFocus();
                tx.Copy();
            }
            if (GUILayout.Button("�����ϲ�"))
            {
                isShowLog = false;
            }
        }

        static void PersistentFileGUI()
        {
            if (isShowPersistentFile)
            {
                ShowPersistentFile();
            }
            else
            {
                ShowPersistentFileList();
            }
        }

        static void ShowPersistentFile()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            try
            {
                GUIUtil.SafeTextArea(showContent);
            }
            catch (Exception e)
            {
                GUILayout.TextArea(e.ToString());
            }

            GUILayout.EndScrollView();
            if (URLManager.GetURL("PersistentFileUpLoadURL") != null)
            {
                if (GUILayout.Button("�ϴ��־�����"))
                {
                    HTTPTool.Upload_Request_Thread(URLManager.GetURL("PersistentFileUpLoadURL"), LogPath, UploadCallBack);
                }
            }
            else
            {
                GUILayout.Label("�ϴ��־�������Ҫ�� URLConfig -> PersistentFileUpLoadURL �����ϴ�Ŀ¼");
            }
            if (GUILayout.Button("���Ƶ�������"))
            {
                TextEditor tx = new TextEditor();
                tx.text = showContent;
                tx.OnFocus();
                tx.Copy();
            }
            if (GUILayout.Button("�����ϲ�"))
            {
                isShowPersistentFile = false;
            }
        }

        static void ShowPersistentFileList()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < FileNameList.Length; i++)
            {
                if (GUILayout.Button(FileNameList[i]))
                {
                    string path = PersistentFileManager.GetPath(FileNameList[i]);
                    HTTPTool.Upload_Request_Thread(URLManager.GetURL("PersistentFileUpLoadURL"), path, UploadCallBack);
                }
            }

            GUILayout.EndScrollView();
            if (URLManager.GetURL("PersistentFileUpLoadURL") != null)
            {
                if (GUILayout.Button("�ϴ����г־������ļ�"))
                {
                    for (int i = 0; i < FileNameList.Length; i++)
                    {
                        string path = PersistentFileManager.GetPath(FileNameList[i]);
                        HTTPTool.Upload_Request_Thread(URLManager.GetURL("PersistentFileUpLoadURL"), path, UploadCallBack);
                    }
                }
            }
            else
            {
                GUILayout.Label("�ϴ��־������ļ���Ҫ�� URLConfig -> PersistentFileUpLoadURL �����ϴ�Ŀ¼");
            }
            if (GUILayout.Button("����־������ļ�"))
            {
                OpenWarnWindow("ȷ��Ҫɾ�����г־������ļ���", () =>
                {
                    Debug.Log("��ɾ�����г־������ļ�");
                    FileTool.SafeDeleteDirectory(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PersistentFileManager.c_directoryName));
                    FileNameList = new string[0];
                });
            }
            if (GUILayout.Button("�����ϲ�"))
            {
                MenuStatus = DevMenuEnum.MainMenu;
            }
        }


        static void OpenWarnWindow(string content, CallBack callBack)
        {
            s_isOpenWarnPanel = true;
            s_warnContent = content;
            s_warnCallBack = callBack;
        }

        static void WarnWindow(int windowID)
        {
            GUILayout.Label(s_warnContent, GUILayout.ExpandHeight(true));
            if (GUILayout.Button("ȡ��", GUILayout.ExpandHeight(true)))
            {
                s_isOpenWarnPanel = false;
                s_warnContent = "";
            }
            if (GUILayout.Button("ȷ��", GUILayout.ExpandHeight(true)))
            {
                s_isOpenWarnPanel = false;
                s_warnContent = "";

                if (s_warnCallBack != null)
                {
                    s_warnCallBack();
                }
            }
        }

        static void SwitchProfileGUI()
        {
            if (s_isProfile)
            {
                s_ProfileGUICallBack();

                if (GUILayout.Button("�ر� ��������" + GetHotKey(2), GUILayout.ExpandHeight(true)))
                {
                    s_isProfile = false;
                }
            }
            else
            {
                if (GUILayout.Button("���� ��������" + GetHotKey(2), GUILayout.ExpandHeight(true)))
                {
                    s_isProfile = true;
                }
            }
        }

        static void ProfileGUI()
        {
            if (s_isProfile)
            {
                s_ProfileGUICallBack();
            }
        }

        static void DevelopHotKeyLogic()
        {
            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    s_isProfile = !s_isProfile;
                }
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    if (Time.timeScale != 0)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                }
                if (Input.GetKeyDown(KeyCode.F4))
                {
                    if (Time.timeScale == 0)
                    {
                        Time.timeScale = 1;
                    }
                    Time.timeScale *= 2;
                }
                if (Input.GetKeyDown(KeyCode.F5))
                {
                    Debug.Log("Time.timeScale " + Time.timeScale);
                    if (Time.timeScale == 0)
                    {
                        Time.timeScale = 1;
                    }
                    Time.timeScale *= 0.5f;
                }
                if (Input.GetKeyDown(KeyCode.F10))
                {
                    string name = GetScreenshotFileName();
                    Debug.Log("�ѱ��� ��Ļ��ͼ " + name);
                    SaveScreenShot(name);
                }
            }
        }

        static void RecordModeGUI()
        {
            GUILayout.Window(2, consoleRect, RecordModeGUIWindow, "Develop Control Panel");
        }

        static void RecordModeGUIWindow(int id)
        {
            SwitchProfileGUI();
            if (RecordManager.GetData(c_recordName).GetRecord(c_qucikLunchKey, true))
            {
                if (GUILayout.Button("������̨", GUILayout.ExpandHeight(true)))
                {
                    RecordManager.SaveRecord(c_recordName, c_qucikLunchKey, false);
                }
            }
            else
            {
                if (GUILayout.Button("�رպ�̨", GUILayout.ExpandHeight(true)))
                {
                    RecordManager.SaveRecord(c_recordName, c_qucikLunchKey, true);
                }
            }
        }

        // �ط�ģʽ��GUI
        static void ReplayModeGUI()
        {
            GUILayout.Window(2, consoleRect, ReplayModeGUIWindow, "Replay Control Panel");
        }

        static void ReplayModeGUIWindow(int id)
        {
            ReplayProgressGUI();
            SwitchProfileGUI();
            if (GUILayout.Button("��ͣ" + GetHotKey(3), GUILayout.ExpandHeight(true)))
            {
                Time.timeScale = 0f;
            }
            if (GUILayout.Button("�����ٶ�" + GetHotKey(3, false), GUILayout.ExpandHeight(true)))
            {
                Time.timeScale = 1;
            }
            if (GUILayout.Button("�ٶȼӱ�" + GetHotKey(4), GUILayout.ExpandHeight(true)))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                Time.timeScale *= 2f;
            }
            if (GUILayout.Button("�ٶȼ���" + GetHotKey(5), GUILayout.ExpandHeight(true)))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                Time.timeScale *= 0.5f;
            }
        }

        static void ReplayProgressGUI()
        {
            string timeContent = "��ǰʱ�䣺" + Time.time.ToString("F");
            timeContent = timeContent.PadRight(20);
            string eventContent = "ʣ�����룺" + s_eventStream.Count;
            eventContent = eventContent.PadRight(20);
            string RandomContent = "ʣ���������" + RandomService.GetRandomListCount();
            RandomContent = RandomContent.PadRight(20);
            string speedContent = "��ǰ�ٶȣ�X" + Time.timeScale;
            speedContent = speedContent.PadRight(20);
            GUILayout.TextField(timeContent + eventContent + RandomContent + speedContent);
        }

        static string GetHotKey(int fn, bool isShowInRun = true)
        {
#if UNITY_EDITOR
            if (fn == 3)
            {
                if ((Time.timeScale != 0 && isShowInRun) || (Time.timeScale == 0 && !isShowInRun))
                {
                    return "(F" + fn + ")";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "(F" + fn + ")";
            }
#else
        return "";
#endif
        }

        public static void OnReplayUpdate()
        {
            DevelopHotKeyLogic();
            for (int i = 0; i < s_eventStream.Count; i++)
            {
                if (s_eventStream[i].m_t < Time.time)
                {
                    InputManager.Dispatch(s_eventStream[i].GetType().Name, s_eventStream[i]);
                    s_eventStream.RemoveAt(i);
                    i--;
                }
            }
        }

        static void OnRecordUpdate()
        {
            DevelopHotKeyLogic();
            s_currentTime += Time.deltaTime;
        }

        public static string[] GetRelpayFileNames()
        {
            FileTool.CreatPath(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, c_directoryName));
            List<string> relpayFileNames = new List<string>();
            string[] allFileName = Directory.GetFiles(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, c_directoryName));
            foreach (var item in allFileName)
            {
                if (item.EndsWith("." + c_eventExpandName))
                {
                    string configName = FileTool.RemoveExpandName(FileTool.GetFileNameByPath(item));
                    relpayFileNames.Add(configName);
                }
            }
            return relpayFileNames.ToArray() ?? new string[0];
        }

        static string GetReplayFileName()
        {
            DateTime now = System.DateTime.Now;
            string logName = string.Format("Replay{0}-{1:D2}-{2:D2}#{3:D2}-{4:D2}-{5:D2}",
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            return logName;
        }

        static string GetScreenshotFileName()
        {
            DateTime now = System.DateTime.Now;
            string screenshotName = string.Format("ScreenShot_{0}x{1}_{2}", Screen.width, Screen.height,
                System.DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            string path = PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath("ScreenShot", screenshotName,"jpg"));
            return path;
        }

        static void UploadCallBack(string result)
        {
            GUIUtil.ShowTips(result);
        }
    }
}