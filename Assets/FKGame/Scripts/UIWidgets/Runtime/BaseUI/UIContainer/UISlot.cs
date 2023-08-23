using UnityEngine;
//------------------------------------------------------------------------
// UI插槽
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
    public class UISlot<T> : MonoBehaviour where T : class
    {
        private UIContainer<T> m_Container;
        public UIContainer<T> Container
        {
            get { return this.m_Container; }
            set { this.m_Container = value; }
        }

        private int m_Index = -1;
        public int Index
        {
            get { return this.m_Index; }
            set { this.m_Index = value; }
        }

        private T m_Item;
        public virtual T ObservedItem
        {
            get
            {
                return this.m_Item;
            }
            set
            {
                this.m_Item = value;
                Repaint();
            }
        }

        public bool IsEmpty
        {
            get { return ObservedItem == null; }
        }

        public virtual void Repaint()
        {
        }

        public virtual bool CanAddItem(T item)
        {
            return true;
        }
    }
}