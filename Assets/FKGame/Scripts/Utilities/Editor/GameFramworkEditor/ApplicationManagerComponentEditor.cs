using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [CustomEditor(typeof(ApplicationManager))]
    public class ApplicationManagerComponentEditor : Editor
    {
        private ApplicationManager applicationManager;
        private int currentSelectIndex = 0;
        private string[] statusList;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            applicationManager = (ApplicationManager)target;
            statusList = GetStatusList();
            currentSelectIndex = GetStatusIndex();
            currentSelectIndex = EditorGUILayout.Popup("First Status:", currentSelectIndex, statusList);
            applicationManager.m_Status = statusList[currentSelectIndex];
            applicationManager.m_globalLogic = GetGlobaLogic();
            if (Application.isPlaying)
            {
                GUILayout.Label("CurrentStatus: " + applicationManager.currentStatus);
            }
            GUILayout.Space(10);
            if (applicationManager.m_globalLogic.Count != 0)
            {
                GUILayout.Label("Global Logic:");
                for (int i = 0; i < applicationManager.m_globalLogic.Count; i++)
                {
                    GUILayout.Label("  " + applicationManager.m_globalLogic[i]);
                }
            }
        }

        public string[] GetStatusList()
        {
            List<string> listTmp = new List<string>();
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(typeof(IApplicationStatus)))
                {
                    listTmp.Add(types[i].Name);
                }
            }
            if (listTmp.Count == 0)
            {
                listTmp.Add("None");
            }
            return listTmp.ToArray();
        }

        public List<string> GetGlobaLogic()
        {
            List<string> listTmp = new List<string>();
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(typeof(IApplicationGlobalLogic)))
                {
                    listTmp.Add(types[i].Name);
                }
            }
            return listTmp;
        }

        public int GetStatusIndex()
        {
            for (int i = 0; i < statusList.Length; i++)
            {
                if (applicationManager.m_Status.Equals(statusList[i]))
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
