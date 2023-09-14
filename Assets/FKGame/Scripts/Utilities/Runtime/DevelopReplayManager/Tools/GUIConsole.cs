using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ����̨GUI����ࡣ����FPS���ڴ�ʹ���������־GUI���
    public class GUIConsole
    {
        struct ConsoleMessage
        {
            public readonly string message;
            public readonly string stackTrace;
            public readonly LogType type;

            public ConsoleMessage(string message, string stackTrace, LogType type)
            {
                this.message = message;
                this.stackTrace = stackTrace;
                this.type = type;
            }
        }

#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
        static bool mTouching = false;
#endif

        public delegate void OnUpdateCallback();        // Update�ص�
        public delegate void OnGUICallback();           // OnGUI�ص�

        static public OnUpdateCallback onUpdateCallback = null;
        static public OnGUICallback onGUICallback = null;
        static public OnGUICallback onGUICloseCallback = null;
        static private FPSCounter fpsCounter = null;    // FPS������
        static private bool showGUI = false;
        static List<ConsoleMessage> entries = new List<ConsoleMessage>();
        static Vector2 scrollPos;
        static int s_page = 0;
        const int c_perPageShowDebug = 50;
        const int margin = 3;
        const int offset = 0;
        static Rect windowRect = new Rect(margin + Screen.width * 0.6f - offset, margin, Screen.width * 0.6f - (2 * margin) + offset, Screen.height - (2 * margin));

        public static void Init()
        {
            fpsCounter = new FPSCounter();
            fpsCounter.Init();
            ApplicationManager.s_OnApplicationUpdate += Update;
            ApplicationManager.s_OnApplicationOnGUI += OnGUI;
            Application.logMessageReceivedThreaded += HandleLog;
        }

        ~GUIConsole()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        static void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.F1))
                showGUI = !showGUI;
#elif UNITY_ANDROID || UNITY_IOS
			if (!mTouching && Input.touchCount >= 6)
			{
				mTouching = true;
				showGUI = !showGUI;
			} else if (Input.touchCount == 0){
				mTouching = false;
			}
#endif
            if (onUpdateCallback != null)
                onUpdateCallback();
        }

        static void OnGUI()
        {
            if (!showGUI)
            {
                if (onGUICloseCallback != null)
                {
                    onGUICloseCallback();
                }
                return;
            }
            GUIUtil.SetGUIStyle();
            if (onGUICallback != null)
                onGUICallback();
            windowRect = new Rect(margin + Screen.width * 0.2f, margin, Screen.width * 0.8f - (2 * margin), Screen.height - (2 * margin));
            GUILayout.Window(100, windowRect, ConsoleWindow, "Console");
        }

        // ��ʾ��־�Ĵ���
        static void ConsoleWindow(int windowID)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            int startIndex = s_page * c_perPageShowDebug;
            int endIndex = startIndex + c_perPageShowDebug;

            if (endIndex > entries.Count)
            {
                endIndex = entries.Count;
            }
            for (int i = startIndex; i < endIndex; i++)
            {
                ConsoleMessage entry = entries[i];
                switch (entry.type)
                {
                    case LogType.Error:
                    case LogType.Exception:
                        GUI.contentColor = Color.red;
                        break;
                    case LogType.Warning:
                        GUI.contentColor = Color.yellow;
                        break;
                    default:
                        GUI.contentColor = Color.white;
                        break;
                }
                if (entry.type == LogType.Exception)
                {
                    GUILayout.Label(entry.message + " || " + entry.stackTrace);
                }
                else
                {
                    GUILayout.Label(entry.message);
                }
            }
            GUI.contentColor = Color.white;
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            GUILayout.Label("��" + (s_page + 1) + "ҳ ��" + Mathf.Ceil(entries.Count / (float)c_perPageShowDebug) + "ҳ");
            if (s_page > 0)
            {
                if (GUILayout.Button("��һҳ"))
                {
                    s_page--;
                }
            }
            if (entries.Count > (s_page + 1) * c_perPageShowDebug)
            {
                if (GUILayout.Button("��һҳ"))
                {
                    s_page++;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("��ҳ"))
            {
                s_page = 0;
            }
            if (GUILayout.Button("ĩҳ"))
            {
                s_page = entries.Count / c_perPageShowDebug;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Clear"))
            {
                entries.Clear();
            }
        }

        static void HandleLog(string message, string stackTrace, LogType type)
        {
            ConsoleMessage entry = new ConsoleMessage(message, stackTrace, type);
            entries.Add(entry);
        }
    }
}