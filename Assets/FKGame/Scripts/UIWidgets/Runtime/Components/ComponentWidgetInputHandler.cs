﻿using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
// 窗口对应快捷键的管理
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
    public class ComponentWidgetInputHandler : MonoBehaviour
    {
        private static Dictionary<KeyCode, List<UIWidget>> m_WidgetKeyBindings;

        void Update()
        {
            if (m_WidgetKeyBindings == null) {
                return;
            }
            foreach (KeyValuePair<KeyCode, List<UIWidget>> kvp in m_WidgetKeyBindings)
            {
                if (Input.GetKeyDown(kvp.Key)){
                    for (int i = 0; i < kvp.Value.Count; i++) {
                        kvp.Value[i].Toggle();
                    }
                }
            }
        }

        public static void RegisterInput(KeyCode key, UIWidget widget) 
        {
            if (m_WidgetKeyBindings == null) {
                ComponentWidgetInputHandler handler = GameObject.FindObjectOfType<ComponentWidgetInputHandler>();
                if (handler == null)
                {
                    GameObject handlerObject = new GameObject("WidgetInputHandler");
                    handlerObject.AddComponent<ComponentWidgetInputHandler>();
                    handlerObject.AddComponent<ComponentSingleInstance>();
                }
                m_WidgetKeyBindings = new Dictionary<KeyCode, List<UIWidget>>();
            }
            if (key == KeyCode.None) {
                return;
            }
            List<UIWidget> widgets;
            if (!m_WidgetKeyBindings.TryGetValue(key, out widgets))
            {
                m_WidgetKeyBindings.Add(key, new List<UIWidget>() { widget });
            }else {
                widgets.RemoveAll(x => x == null);
                widgets.Add(widget);
                m_WidgetKeyBindings[key] = widgets;
            }
        }

        public static void UnregisterInput(KeyCode key, UIWidget widget)
        {
            if (m_WidgetKeyBindings == null)
                return;

            List<UIWidget> widgets;
            if (m_WidgetKeyBindings.TryGetValue(key, out widgets))
            {
                widgets.Remove(widget);
            }
        }
    }
}