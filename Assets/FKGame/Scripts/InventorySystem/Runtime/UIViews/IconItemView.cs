using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class IconItemView : ItemView
    {
        [Tooltip("Image reference to display the icon.")]
        [SerializeField]
        protected Image m_Ícon;

        public override void Repaint(Item item)
        {
            if (this.m_Ícon != null)
            {
                if (item != null)
                {
                    this.m_Ícon.overrideSprite = item.Icon;
                    this.m_Ícon.enabled = true;
                }
                else
                {
                    this.m_Ícon.enabled = false;
                }
            }
        }
    }
}