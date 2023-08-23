using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
//------------------------------------------------------------------------
// UI插槽的容器
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
	public class UIContainer<T> : UIWidget where T : class
	{
        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnAddItem");
                callbacks.Add("OnRemoveItem");
                return callbacks.ToArray();
            }
        }

        [Header ("Behaviour")]
        [SerializeField]
        protected bool m_DynamicContainer = false;

        [SerializeField]
        protected Transform m_SlotParent;

        [SerializeField]
        protected GameObject m_SlotPrefab;

        protected List<UISlot<T>> m_Slots = new List<UISlot<T>>();
        public ReadOnlyCollection<UISlot<T>> Slots
        {
            get
            {
                return this.m_Slots.AsReadOnly();
            }
        }

        protected List<T> m_Collection;
  
        protected override void OnAwake ()
		{
			base.OnAwake ();
            this.m_Collection = new List<T>();
			RefreshSlots ();
		}

        public virtual bool AddItem(T item)
        {
            UISlot<T> slot = null;
            if (CanAddItem(item, out slot, true))
            {
                ReplaceItem(slot.Index, item);
                return true;
            }
            return false;
        }

        public virtual bool RemoveItem(int index)
        {
            if (index < this.m_Slots.Count)
            {
                UISlot<T> slot = this.m_Slots[index];
                T item = slot.ObservedItem;

                if (item != null)
                {
                    this.m_Collection.Remove(item);
                    slot.ObservedItem = null;
                    return true;
                }
            }
            return false;
        }

        public virtual T ReplaceItem(int index, T item)
        {
            if (index < this.m_Slots.Count)
            {
                UISlot<T> slot = this.m_Slots[index];
                if (!slot.CanAddItem(item)) {
                    return item;
                }
                if (item != null)
                {
                    this.m_Collection.Add(item);

                    T current = slot.ObservedItem;
                    if (current != null)
                    {
                        RemoveItem(slot.Index);
                    }
                    slot.ObservedItem = item;
                    return current;
                }
            }
            return item;
        }

        public virtual bool CanAddItem(T item, out UISlot<T> slot, bool createSlot = false)
        {
            slot = null;
            if (item == null) { return true; }

            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                if (this.m_Slots[i].IsEmpty && this.m_Slots[i].CanAddItem(item))
                {
                    slot = this.m_Slots[i];
                    return true;
                }
            }

            if (this.m_DynamicContainer)
            {
                if (createSlot)
                {
                    slot = CreateSlot();
                }
                return true;
            }
            return false;
        }

        public void RefreshSlots()
        {
            if (this.m_DynamicContainer && this.m_SlotParent != null)
            {
                this.m_Slots = this.m_SlotParent.GetComponentsInChildren<UISlot<T>>(true).ToList();
                this.m_Slots.Remove(this.m_SlotPrefab.GetComponent<UISlot<T>>());
            }
            else
            {
                this.m_Slots = GetComponentsInChildren<UISlot<T>>(true).ToList();
            }

            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                UISlot<T> slot = this.m_Slots[i];
                slot.Index = i;
                slot.Container = this;
            }
        }

        protected virtual UISlot<T> CreateSlot()
        {
            if (this.m_SlotPrefab != null && this.m_SlotParent != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotParent, false);
                UISlot<T> slot = go.GetComponent<UISlot<T>>();
                this.m_Slots.Add(slot);
                slot.Index = Slots.Count - 1;
                slot.Container = this;
                return slot;
            }
            Debug.LogWarning("Please ensure that the slot prefab and slot parent is set in the inspector.");
            return null;
        }

        protected virtual void DestroySlot(int index)
        {
            if (index < this.m_Slots.Count)
            {
                DestroyImmediate(this.m_Slots[index].gameObject);
                RefreshSlots();
            }
        }
    }
}