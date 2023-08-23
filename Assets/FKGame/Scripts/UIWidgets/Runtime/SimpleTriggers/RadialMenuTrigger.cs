﻿using UnityEngine;
using FKGame.UIWidgets;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
// 弹出二次面板
//------------------------------------------------------------------------
public class RadialMenuTrigger : MonoBehaviour, IPointerDownHandler
{ 
    public Sprite[] menuIcons;
    private RadialMenu m_RadialMenu;

    private void Start()
    {
        this.m_RadialMenu = WidgetUtility.Find<RadialMenu>("RadialMenu");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_RadialMenu.Show(gameObject, menuIcons, delegate (int index) { Debug.Log("Used index - " + index); });
    }
}
