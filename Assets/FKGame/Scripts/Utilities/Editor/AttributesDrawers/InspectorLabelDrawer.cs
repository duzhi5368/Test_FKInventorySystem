﻿using UnityEngine;
using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame
{
	[CustomPropertyDrawer (typeof(InspectorLabelAttribute))]
	public class InspectorLabelDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
            // Debug.Log("InspectorLabelDrawer::OnGUI - " + label.text);
            InspectorLabelAttribute attr = attribute as InspectorLabelAttribute;
			EditorGUI.PropertyField (position, property, new GUIContent (attr.label, attr.tooltip));
		}
	}
}