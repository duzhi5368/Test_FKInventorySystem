﻿using UnityEngine;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
    [CustomEditor(typeof(BehaviorTrigger), true)]
    public class BehaviorTriggerInspector : BaseTriggerInspector
    {
        private void DrawInspector()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Interruptable"));
            SerializedProperty actionTemplate = serializedObject.FindProperty("actionTemplate");
            EditorGUILayout.PropertyField(actionTemplate);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying || actionTemplate.objectReferenceValue != null);

            if (EditorTools.RightArrowButton(new GUIContent("Edit Behavior", "Trigger use behavior"), GUILayout.Height(20f)))
            {
                SerializedProperty actionList = serializedObject.FindProperty("actions");
                
                ObjectWindow.ShowWindow("Edit Behavior",serializedObject, actionList);
            }
            EditorGUI.EndDisabledGroup(); 
        }
    }
}
