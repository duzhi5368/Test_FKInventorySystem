﻿using UnityEngine;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            EditorGUI.EndProperty();
        }
    }
}