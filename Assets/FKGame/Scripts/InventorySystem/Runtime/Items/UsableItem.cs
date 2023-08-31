using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FKGame.InventorySystem.ItemActions;
using System.Linq;
using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [System.Serializable]
	public class UsableItem : Item
	{
        [Tooltip("类型公用冷却，例如多个药品，会有公用冷却CD。如不使用公用冷却，则可使用自身冷却")]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.IS_USE_CATEGORY_COOLDOWN)]
        private bool m_UseCategoryCooldown = true;
        [SerializeField]
        [InspectorLabel(LanguagesMacro.ITEM_COOLDOWN)]
        private float m_Cooldown = 1f;
        public float Cooldown {
            get {
                return this.m_UseCategoryCooldown ? Category.Cooldown : this.m_Cooldown;
            }
        }

        [SerializeReference]
        public List<Action> actions = new List<Action>();

        private Sequence m_ActionSequence;
        private IEnumerator m_ActionBehavior;

        protected override void OnEnable()
        {
            base.OnEnable();
           
            for (int i = 0; i < actions.Count; i++) {
                if (actions[i] is ItemAction)
                {
                    ItemAction action = actions[i] as ItemAction;
                    action.item = this;
                }
            }
        }

        public override void Use()
        {
            if (this.m_ActionSequence == null) { 
                GameObject gameObject = InventoryManager.current.PlayerInfo.gameObject;
                this.m_ActionSequence = new Sequence(gameObject, InventoryManager.current.PlayerInfo, gameObject!= null?gameObject.GetComponent<ComponentBlackboard>():null, actions.Cast<IAction>().ToArray());
            }
            if (this.m_ActionBehavior != null) {
                UnityTools.StopCoroutine(m_ActionBehavior);
            }
            this.m_ActionBehavior = SequenceCoroutine();
            UnityTools.StartCoroutine(this.m_ActionBehavior);
        }

        protected IEnumerator SequenceCoroutine() {
            this.m_ActionSequence.Start();
            while (this.m_ActionSequence.Tick()) {
                yield return null;
            }
        }
    }
}