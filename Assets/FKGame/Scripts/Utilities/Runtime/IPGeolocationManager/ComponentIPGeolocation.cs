using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ComponentIPGeolocation : MonoBehaviour
    {
        private string showDetail = "";
        private Vector2 pos;
        private string logStr = "";

        void Start()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            logStr += "[" + type + "]" + condition + "\n";
        }

        void OnGUI()
        {
            GUIStyle style = "box";
            style.fontSize = 35;
            style.alignment = TextAnchor.UpperLeft;
            style.wordWrap = true;
            if (GUILayout.Button("Get IPGeolocation", GUILayout.Height(75), GUILayout.Width(Screen.width)))
            {
                showDetail = "Start Get IP...";
                IPGeolocationManager.GetIPGeolocation((detail) =>
                {
                    if (detail == null)
                        showDetail = "Get IP failed!";
                    else
                        showDetail = JsonSerializer.ToJson(detail);
                });
            }
            if (GUILayout.Button("Clear", GUILayout.Height(75), GUILayout.Width(Screen.width)))
            {
                showDetail = "";
                logStr = "";
            }

            pos = GUILayout.BeginScrollView(pos);
            GUILayout.Box(showDetail, style);
            GUILayout.Label(logStr, style);
            GUILayout.EndScrollView();
        }
    }
}