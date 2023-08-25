using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Stat System/Compare")]
    [System.Serializable]
    public class Compare : Action, ICondition
    {
        [SerializeField]
        private TargetType m_Target = TargetType.Player;

        [InspectorLabel(LanguagesMacro.STAT)]
        [SerializeField]
        protected string m_StatName = "Health";

        [SerializeField]
        [EnumLabel(LanguagesMacro.VALUE_TYPE)]
        protected ValueType m_ValueType = ValueType.CurrentValue;

        [SerializeField]
        [EnumLabel(LanguagesMacro.COMPARE_CONDITION)]
        protected ConditionType m_Condition = ConditionType.Greater;

        [SerializeField]
        [InspectorLabel(LanguagesMacro.VALUE)]
        protected float m_Value;

        private StatsHandler m_Handler;

        public override void OnStart()
        {
            this.m_Handler = this.m_Target == TargetType.Self ? gameObject.GetComponent<StatsHandler>() : playerInfo.gameObject.GetComponent<StatsHandler>();
        }

        public override ActionStatus OnUpdate()
        {
            Stat stat = this.m_Handler.GetStat(this.m_StatName) as Stat;
            if (stat == null) return ActionStatus.Failure;

            float value = stat.Value;
            if (this.m_ValueType == ValueType.CurrentValue)
                value = (stat as Attribute).CurrentValue;

            switch (this.m_Condition) {
                case ConditionType.Greater:
                    return value > this.m_Value ? ActionStatus.Success : ActionStatus.Failure;
                case ConditionType.GreaterOrEqual:
                    return value >= this.m_Value ? ActionStatus.Success : ActionStatus.Failure;
                case ConditionType.Less:
                    return value < this.m_Value ? ActionStatus.Success : ActionStatus.Failure;
                case ConditionType.LessOrEqual:
                    return value <= this.m_Value ? ActionStatus.Success : ActionStatus.Failure;
            }

            return ActionStatus.Failure;
        }
    }
}