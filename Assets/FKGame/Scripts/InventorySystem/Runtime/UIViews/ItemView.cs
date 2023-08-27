using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public abstract class ItemView : MonoBehaviour
    {
        protected virtual void Start() { }
        public abstract void Repaint(Item item);
        public virtual bool RequiresConstantRepaint() { return false; }
    }
}