﻿using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [CustomEditor(typeof(EquipmentItem), true)]
    public class EquipmentItemInspector :  UsableItemInspector
    {
        private ReorderableList regionList;
        private SerializedProperty m_OverrideEquipPrefab;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (target == null) return;
            this.m_OverrideEquipPrefab = serializedObject.FindProperty("m_OverrideEquipPrefab");

            this.regionList = new ReorderableList(serializedObject, serializedObject.FindProperty("m_Region"), true, true, true, true);
            this.regionList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, LanguagesMacro.EQUIP_REGION);
            };
            this.regionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                SerializedProperty element = regionList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, element, GUIContent.none, true);
            };
        }

        private void DrawInspector() {
            GUILayout.Space(5f);
            GUILayout.Label(LanguagesMacro.EQUIPMENT, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(this.m_OverrideEquipPrefab);
            EditorGUILayout.HelpBox(LanguagesMacro.EQUIPMENT_REGION_TIPS, MessageType.Info);
            regionList.DoLayoutList();
        }
    }
}