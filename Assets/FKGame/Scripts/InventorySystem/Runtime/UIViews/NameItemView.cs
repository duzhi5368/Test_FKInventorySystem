using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class NameItemView : ItemView
    {
        [Tooltip("Text reference to display item name.")]
        [InspectorLabel("Name")]
        [SerializeField]
        protected Text m_ItemName;
        [Tooltip("Should the name use rarity color?")]
        [SerializeField]
        protected bool m_UseRarityColor = true;

        public override void Repaint(Item item)
        {
            if (this.m_ItemName != null)
            {
                this.m_ItemName.text = (item != null ? (this.m_UseRarityColor ? UnityTools.ColorString(item.DisplayName, item.Rarity.Color) : item.DisplayName) : string.Empty);
            }
        }
    }
}