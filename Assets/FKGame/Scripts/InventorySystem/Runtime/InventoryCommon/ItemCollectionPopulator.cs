using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [RequireComponent(typeof(ItemCollection))]
    public class ItemCollectionPopulator : MonoBehaviour
    {
        [SerializeField]
        public ItemGroup m_ItemGroup;

        private void Start() {}
    }
}