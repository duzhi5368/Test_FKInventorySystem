using FKGame.Macro;
using FKGame.UIWidgets;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [System.Serializable]
    public class CraftingRecipe : ScriptableObject, INameable
    {
        [SerializeField]
        [InspectorLabel(LanguagesMacro.NAME)]
        private new string name = LanguagesMacro.NEW_CRAFTING_RECIPE;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [Tooltip("制作需要的时间，该值需要显示到UI上")]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.CRAFT_DURATION)]
        private float m_Duration = 2f;
        public float Duration
        {
            get { return this.m_Duration; }
        }

        [Tooltip("制造时，在动画控制器中播放的动画状态。如无需要播放的动画，则设为空")]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.CRAFT_ANIMATOR_STATE)]
        private string m_AnimatorState = "";
        public string AnimatorState
        {
            get { return this.m_AnimatorState; }
        }

        [Tooltip("制造技能")]
        [AcceptNull]
        [SerializeField]
        private Skill m_Skill = null;
        public Skill Skill
        {
            get { return this.m_Skill; }
        }

        [Tooltip("制造失败时是否移除原材料")]
        [SerializeField]
        [InspectorLabel(LanguagesMacro.IS_REMOVE_INGREDIENTS)]
        private bool m_RemoveIngredientsWhenFailed = false;
        public bool RemoveIngredientsWhenFailed
        {
            get { return this.m_RemoveIngredientsWhenFailed; }
        }

        [Tooltip("制作所需原材料")]
        [SerializeField]
        private List<ItemAmountDefinition> m_Ingredients = new List<ItemAmountDefinition>();
        public List<ItemAmountDefinition> Ingredients {
            get { return this.m_Ingredients; }
        }

        [SerializeField]
        private ItemModifierList m_CraftingModifier = new ItemModifierList();
        public ItemModifierList CraftingModifier
        {
            get { return this.m_CraftingModifier; }
            set { this.m_CraftingModifier = value; }
        }

        [SerializeReference]
        public List<ICondition> conditions = new List<ICondition>();
        public bool CheckConditions()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                ICondition condition = conditions[i];
                condition.Initialize(InventoryManager.current.PlayerInfo.gameObject, InventoryManager.current.PlayerInfo, InventoryManager.current.PlayerInfo.gameObject.GetComponent<ComponentBlackboard>());
                condition.OnStart();
                if (condition.OnUpdate() == ActionStatus.Failure)
                {
                    condition.OnEnd();
                    return false;
                }
            }
            return true;
        }

        [System.Serializable]
        public class ItemAmountDefinition
        {
            public Item item;
            public int amount = 1;
        }
    }
}