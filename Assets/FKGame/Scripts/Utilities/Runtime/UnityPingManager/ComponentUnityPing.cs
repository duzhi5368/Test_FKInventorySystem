using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ComponentUnityPing : MonoBehaviour
    {
        public List<string> hostLists = new List<string>{
                "www.google.com",
                "baidu.cn",
                "127.0.0.1"
            };
        public int pingTime = 4;
        private List<string> resultString = new List<string>();
        private Vector2 pos;
        private string customURL = "";

        void Start()
        {
            // 在开始就调用在手机上可能会卡一会儿
            foreach (var item in hostLists)
            {
                UnityPingManager.Ping(item, pingTimes: pingTime, resultCallBack: ResultCallBack);
            }
        }

        private void ResultCallBack(string res, UnityPingStatistics arg)
        {
            resultString.Add(res);
        }

        private void OnGUI()
        {
            GUIStyle style = "box";
            style.fontSize = 35;
            GUI.skin.textField.fontSize = 32;
            GUI.skin.label.fontSize = 35;
            GUI.skin.button.fontSize = 30;
            GUILayout.Label("URL：");
            customURL = GUILayout.TextField(customURL, GUILayout.Width(Screen.width), GUILayout.Height(60));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Ping", GUILayout.Height(60)))
            {
                UnityPingManager.Ping(customURL, resultCallBack: ResultCallBack);
            }
            if (GUILayout.Button("Ping List All", GUILayout.Height(60)))
            {
                foreach (var item in hostLists)
                {
                    UnityPingManager.Ping(item, pingTimes: pingTime, resultCallBack: ResultCallBack);
                }
            }
            if (GUILayout.Button("PingGetOptimalItem", GUILayout.Height(60)))
            {
                UnityPingManager.PingGetOptimalItem(hostLists.ToArray(), (res) =>
                {
                    resultString.Add(res.ToString());
                }, pingTimes: pingTime);
            }
            if (GUILayout.Button("Clear", GUILayout.Height(60)))
            {
                resultString.Clear();
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Ping Times：");
            pingTime = int.Parse(GUILayout.TextField(pingTime.ToString(), GUILayout.Width(Screen.width), GUILayout.Height(60)));
            pos = GUILayout.BeginScrollView(pos);
            foreach (var item in resultString)
            {
                GUILayout.Box(item, style);
            }
            GUILayout.EndScrollView();
        }
    }
}