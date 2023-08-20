using System.Collections;
using System.Collections.Generic;
using FKGame.UIWidgets;
using UnityEngine;

namespace FKGame.InventorySystem
{
    public class ItemTooltip : Tooltip
    {
        public void Show(Item item){
            Show(UnityTools.ColorString(item.DisplayName, item.Rarity.Color),item.Description,item.Icon,item.GetPropertyInfo());
        }
    }
}