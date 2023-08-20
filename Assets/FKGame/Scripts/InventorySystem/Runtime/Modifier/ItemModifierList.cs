using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame.InventorySystem
{
    [System.Serializable]
    public class ItemModifierList
    {
        public List<ItemModifier> modifiers = new List<ItemModifier>();

        public void Modify(Item item) {
            for (int i = 0; i < modifiers.Count; i++) {
                modifiers[i].Modify(item);
            }
        }
    }
}