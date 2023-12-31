﻿using FKGame.UIWidgets;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class PropertyItemView : ItemView
    {
        [SerializeField]
        protected ComponentStringPairSlot m_SlotPrefab;

        protected List<ComponentStringPairSlot> m_SlotCache = new List<ComponentStringPairSlot>();

        public override void Repaint(Item item)
        {
            if (this.m_SlotPrefab != null)
            {
                for (int i = 0; i < this.m_SlotCache.Count; i++)
                {
                    this.m_SlotCache[i].gameObject.SetActive(false);
                }
                if (item != null)
                {
                    List<KeyValuePair<string, string>> pairs = item.GetPropertyInfo();

                    if (pairs != null && pairs.Count > 0)
                    {
                        while (pairs.Count > this.m_SlotCache.Count)
                        {
                            CreateSlot();
                        }
                        for (int i = 0; i < pairs.Count; i++)
                        {
                            ComponentStringPairSlot slot = this.m_SlotCache[i];
                            slot.gameObject.SetActive(true);
                            slot.Target = pairs[i];
                        }
                        this.m_SlotPrefab.transform.parent.gameObject.SetActive(true);
                    }
                }
            }
        }
        protected virtual ComponentStringPairSlot CreateSlot()
        {
            if (this.m_SlotPrefab != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab.gameObject);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotPrefab.transform.parent, false);
                ComponentStringPairSlot slot = go.GetComponent<ComponentStringPairSlot>();
                this.m_SlotCache.Add(slot);

                return slot;
            }
            Debug.LogWarning("[ItemSlot] Please ensure that the slot prefab is set in the inspector.");
            return null;
        }
    }
}