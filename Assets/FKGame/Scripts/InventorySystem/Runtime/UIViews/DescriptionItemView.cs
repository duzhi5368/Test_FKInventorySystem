using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class DescriptionItemView : ItemView
    {
        [Tooltip("The text reference to display item description")]
        [SerializeField]
        protected Text m_Description;

        public override void Repaint(Item item)
        {
            if (this.m_Description != null)
            {
                this.m_Description.text = (item != null ? item.Description : string.Empty);
            }
        }
    }
}