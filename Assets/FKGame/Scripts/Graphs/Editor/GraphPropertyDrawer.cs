﻿using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    public class GraphPropertyDrawer<T> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (GUI.Button(position, label, EditorStyles.objectField)) {
                GraphEditorWindow window = GraphEditorWindow.ShowWindow();
                IGraphProvider behavior = (IGraphProvider)property.GetParent();
                window.Load<T>(behavior, property.serializedObject.targetObject);
            }
            EditorGUI.EndProperty();
        }
    }
}