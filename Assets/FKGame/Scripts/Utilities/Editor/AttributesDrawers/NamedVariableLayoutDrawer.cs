﻿using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [CustomDrawer(typeof(NamedVariable),true)]
    public class NamedVariableLayoutDrawer : CustomDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            NamedVariable variable = value as NamedVariable;
            EditorGUILayout.TextField("Name", variable.Name);
            variable.VariableType = (NamedVariableType)EditorGUILayout.Popup((int)variable.VariableType, variable.VariableTypeNames);
            variable.SetValue(EditorTools.DrawFields(variable.GetValue()));
        }
    }
}