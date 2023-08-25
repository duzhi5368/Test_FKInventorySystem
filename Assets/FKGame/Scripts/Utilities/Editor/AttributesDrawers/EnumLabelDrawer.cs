using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(EnumLabelAttribute))]
public class EnumLabelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var att = (EnumLabelAttribute)attribute;
        var enumType = fieldInfo.FieldType;

        string[] enumNames = Enum.GetNames(enumType);
        string[] enumDisplayNames = new string[enumNames.Length];

        for (int i = 0; i < enumNames.Length; i++)
        {
            enumDisplayNames[i] = GetEnumDisplayName(enumType, enumNames[i]);
        }
        EditorGUI.BeginChangeCheck();
        var value = EditorGUI.Popup(position, att.header, property.enumValueIndex, enumDisplayNames);
        if (EditorGUI.EndChangeCheck())
        {
            property.enumValueIndex = value;
        }
    }

    private string GetEnumDisplayName(Type enumType, string enumName)
    {
        var enumField = enumType.GetField(enumName);
        var headerAttributes = enumField.GetCustomAttributes(typeof(HeaderAttribute), false);

        return headerAttributes.Length > 0 ? ((HeaderAttribute)headerAttributes[0]).header : enumName;
    }
}
