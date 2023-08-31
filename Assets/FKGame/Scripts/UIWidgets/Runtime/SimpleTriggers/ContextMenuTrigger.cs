using UnityEngine;
using FKGame.UIWidgets;
using UIContextMenu = FKGame.UIWidgets.UIContextMenu;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
// 弹出子菜单
//------------------------------------------------------------------------
public class ContextMenuTrigger : MonoBehaviour, IPointerDownHandler
{
    private UIContextMenu m_ContextMenu;
    public string[] menu;

    private void Start()
    {
        // 性能开销不小，要注意
        this.m_ContextMenu = WidgetUtility.Find<UIContextMenu>("ContextMenu");
    }

    // 注意，需要在camera上添加physical raycast
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.m_ContextMenu.Clear();
            for (int i = 0; i < menu.Length; i++)
            {
                string menuItem = menu[i];
                m_ContextMenu.AddMenuItem(menuItem, delegate { Debug.Log("Used - " + menuItem); });
            }
            this.m_ContextMenu.Show();
        }
    }
}
