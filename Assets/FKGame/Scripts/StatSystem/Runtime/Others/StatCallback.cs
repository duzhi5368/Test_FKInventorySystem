using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [System.Serializable]
    public class StatCallback
    {
        [InspectorLabel(LanguagesMacro.VALUE_TYPE)]
        [SerializeField]
        protected ValueType m_ValueType = ValueType.CurrentValue;
        [InspectorLabel(LanguagesMacro.CONDITION)]
        [SerializeField]
        protected ConditionType m_Condition;
        [InspectorLabel(LanguagesMacro.VALUE)]
        [SerializeField]
        protected float m_Value = 0f;
        [SerializeField]
        protected Actions m_Actions;

        protected Stat m_Stat;
        protected StatsHandler m_Handler;
        protected Sequence m_Sequence;

        public virtual void Initialize(StatsHandler handler, Stat stat) {
            this.m_Handler = handler;
            this.m_Stat = stat;
            switch (this.m_ValueType)
            {
                case ValueType.Value:
                    stat.onValueChange += OnValueChange;
                    break;
                case ValueType.CurrentValue:
                    if (stat is Attribute attribute)
                    {
                        attribute.onCurrentValueChange += OnCurrentValueChange;
                    }
                    break;
            }
            this.m_Sequence = new Sequence(handler.gameObject, new PlayerInfo("Player"),handler.GetComponent<Blackboard>(), this.m_Actions.actions.ToArray());
            this.m_Handler.onUpdate += Update;
        }

        private void Update() 
        {
            if (this.m_Sequence != null)
            {
                this.m_Sequence.Tick();
            }
        }

        private void OnValueChange()
        {
            if (TriggerCallback(this.m_Stat.Value))
            {
                this.m_Sequence.Start();
            }
        }

        private void OnCurrentValueChange()
        {
            if (TriggerCallback((this.m_Stat as Attribute).CurrentValue))
            {
                this.m_Sequence.Start();
            }
        }

        private bool TriggerCallback(float value)
        {
            switch (this.m_Condition)
            {
                case ConditionType.Greater:
                    return value > this.m_Value;
                case ConditionType.GreaterOrEqual:
                    return value >= this.m_Value;
                case ConditionType.Less:
                    return value < this.m_Value;
                case ConditionType.LessOrEqual:
                    return value <= this.m_Value;
            }
            return false;
        }
    }

    public enum ValueType
    {
        Value, CurrentValue
    }

    public enum ConditionType
    {
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual,
    }
}