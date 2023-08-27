using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [CreateAssetMenu(fileName = "SimpleStackModifier", menuName = "FKGame/物品系统/随机数量调整器")]
    [System.Serializable]
    public class StackModifier : ItemModifier
    {
        [SerializeField]
        protected int m_Min = 1;
        [SerializeField]
        protected int m_Max = 2;

        public override void Modify(Item item)
        {
            int stack = Random.Range(this.m_Min, this.m_Max);
            item.Stack = stack;
        }
    }
}