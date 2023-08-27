﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem.ItemActions
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [System.Serializable]
    public abstract class ItemAction : Action
    {
        [HideInInspector]
        public Item item;
    }
}