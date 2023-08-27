using FKGame.UIWidgets;
using UnityEngine;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class DisplayCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected Sprite m_Sprite;
        [SerializeField]
        protected Vector2 m_Size = new Vector2(32f, 32f);
        [SerializeField]
        protected string m_AnimatorState = "Cursor";

        protected virtual void DoDisplayCursor(bool state)
        {
            if (state)
            {
                UICursor.Set(this.m_Sprite, this.m_Size, false, this.m_AnimatorState);
            }
            else
            {
                UICursor.Clear();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!UnityTools.IsPointerOverUI())
            {
                DoDisplayCursor(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!UnityTools.IsPointerOverUI())
            {
                DoDisplayCursor(false);
            }
        }
    }
}