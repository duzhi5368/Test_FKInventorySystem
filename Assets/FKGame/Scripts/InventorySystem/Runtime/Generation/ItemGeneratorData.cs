using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame.InventorySystem
{
    [System.Serializable]
    public class ItemGeneratorData
    {
        public Item item;
        public int minStack = 1;
        public int maxStack = 1;
       // public float propertyRandomizer = 0.15f;
        [Range(0f, 1f)]
        public float chance = 1.0f;
        public ItemModifierList modifiers;
    }
}