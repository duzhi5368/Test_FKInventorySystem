using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class StackItemView : ItemView
    {
        [Tooltip("The text reference to display item stack.")]
        [SerializeField]
        protected Text m_Stack;

        protected override void Start()
        {
            if (this.m_Stack != null)
                this.m_Stack.raycastTarget = false;
        }

        public override void Repaint(Item item)
        {
            if (this.m_Stack != null)
            {
                if (item != null && item.MaxStack > 1)
                {
                    this.m_Stack.text = item.Stack.ToString();
                    this.m_Stack.enabled = true;
                }
                else
                {
                    this.m_Stack.enabled = false;
                }
            }
        }
    }
}