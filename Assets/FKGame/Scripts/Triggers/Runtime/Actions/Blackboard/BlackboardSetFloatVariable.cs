﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [ComponentMenu("Blackboard/Set Float Variable")]
    public class BlackboardSetFloatVariable : Action
    {
        [SerializeField]
        private string m_VariableName = "Energy";
        [SerializeField]
        private float m_Value = 0f;
        [SerializeField]
        private float m_DampTime = 1f;

        public override ActionStatus OnUpdate()
        {
            if (this.m_DampTime > 0f)
                return ActionStatus.Success;

            blackboard.SetValue<float>(this.m_VariableName, this.m_Value);
            return ActionStatus.Success;
        }

        public override void Update()
        {
            if (this.m_DampTime <= 0f)
                return;

            float current = blackboard.GetValue<float>(this.m_VariableName);
            current = Mathf.Lerp(current, this.m_Value, Time.deltaTime * this.m_DampTime);
            blackboard.SetValue<float>(this.m_VariableName, current);
        }
    }
}
