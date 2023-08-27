using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [System.Serializable]
    public abstract class ItemModifier : ScriptableObject, IModifier<Item>
    {
        public abstract void Modify(Item item);
    }
}