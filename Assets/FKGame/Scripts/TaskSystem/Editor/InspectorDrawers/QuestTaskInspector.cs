﻿using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{

    [CustomEditor(typeof(QuestTask))]
    public class QuestTaskInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Hi");
        }
    }
}