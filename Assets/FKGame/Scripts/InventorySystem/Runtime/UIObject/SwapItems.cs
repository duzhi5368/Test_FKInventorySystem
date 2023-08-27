using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class SwapItems : MonoBehaviour
    {
        public KeyCode key = KeyCode.R;
        public ItemSlot first;
        public ItemSlot second;

        private void Update()
        {
            if (Input.GetKeyDown(key)) {
                first.Container.SwapItems(first, second);
            }
        }
    }
}