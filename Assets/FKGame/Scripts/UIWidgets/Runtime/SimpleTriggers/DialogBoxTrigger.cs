﻿using UnityEngine;
using FKGame.UIWidgets;
//------------------------------------------------------------------------
// 弹出Dialog窗口
//------------------------------------------------------------------------
public class DialogBoxTrigger : MonoBehaviour
{
    public string title;
    [TextArea]
    public string text;
    public Sprite icon;
    public string[] options;

    private UIDialogBox m_DialogBox;

    private void Start()
    {
        this.m_DialogBox = FindObjectOfType<UIDialogBox>();   
    }

    public void Show() {
        m_DialogBox.Show(title, text, icon, null, options);
    }

    public void ShowWithCallback()
    {
        m_DialogBox.Show(title, text, icon, OnDialogResult, options);
    }

    private void OnDialogResult(int index)
    {
        m_DialogBox.Show("Result", "Callback Result: "+options[index], icon, null, "OK");
    }
}
