using FKGame.Macro;
using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
// 绘制 物品格Slot 组件的检查器
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [CustomEditor(typeof(Slot),true)]
    public class SlotInspector : CallbackHandlerInspector
    {
        public override void OnInspectorGUI()
        {
            ScriptGUI();
            serializedObject.Update();
            for (int i = 0; i < m_DrawInspectors.Count; i++)
            {
                this.m_DrawInspectors[i].Invoke();
            }
            DrawPropertiesExcluding(serializedObject, this.m_PropertiesToExcludeForChildClasses);
            if (EditorTools.RightArrowButton(new GUIContent(LanguagesMacro.RESTRICTIONS, LanguagesMacro.SLOT_RESTRICTIONS)))
            {
                AssetWindow.ShowWindow(LanguagesMacro.SLOT_RESTRICTIONS, serializedObject.FindProperty("restrictions"));
            }
            TriggerGUI();
            serializedObject.ApplyModifiedProperties();
            if (EditorWindow.mouseOverWindow != null)
            {
                EditorWindow.mouseOverWindow.Repaint();
            }
        }
    }
}