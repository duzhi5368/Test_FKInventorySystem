﻿using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    public class SkillSlot : ItemSlot
    {
        [SerializeField]
        protected Text m_Value;

        public override void Repaint()
        {
            base.Repaint();
            Skill skill = ObservedItem as Skill;
            if (this.m_Value != null)
            {
                this.m_Value.text = (skill != null ? skill.CurrentValue.ToString("F1")+"%" : string.Empty);
            }
        }
    }
}