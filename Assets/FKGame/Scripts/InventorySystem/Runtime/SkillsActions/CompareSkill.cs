using FKGame.UIWidgets;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [Icon("Item")]
    [ComponentMenu("Inventory System/Compare Skill")]
    public class CompareSkill : Action, ICondition
    {
        [SerializeField]
        private Skill m_Skill = null;
        [SerializeField]
        private ActionConditionType m_Condition = ActionConditionType.Greater;
        [Range(0f,100f)]
        [SerializeField]
        private float m_Value = 0f;
        [SerializeField]
        private UINotificationOptions m_SuccessNotification = null;
        [SerializeField]
        private UINotificationOptions m_FailureNotification = null;

        public override ActionStatus OnUpdate()
        {
            Skill skill = ItemContainer.GetItem(this.m_Skill.Id) as Skill;
            if (skill != null && Compare(skill.CurrentValue)) {
                if (this.m_SuccessNotification != null && !string.IsNullOrEmpty(this.m_SuccessNotification.text))
                    this.m_SuccessNotification.Show();
                return ActionStatus.Success;
            }
            if (this.m_FailureNotification != null && !string.IsNullOrEmpty(this.m_FailureNotification.text))
                this.m_FailureNotification.Show();
            return ActionStatus.Failure;
        }

        private bool Compare(float value) 
        {
            switch (this.m_Condition)
            {
                case ActionConditionType.Greater:
                    return value > this.m_Value;
                case ActionConditionType.GreaterOrEqual:
                    return value >= this.m_Value;
                case ActionConditionType.Less:
                    return value < this.m_Value;
                case ActionConditionType.LessOrEqual:
                    return value <= this.m_Value;
            }
            return false;
        }
    }
}
