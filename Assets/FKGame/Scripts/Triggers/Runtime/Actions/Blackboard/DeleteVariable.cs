﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [ComponentMenu("Blackboard/Delete Variable")]
    public class DeleteVariable : Action
    {
        [SerializeField]
        private string m_VariableName = "";

        public override ActionStatus OnUpdate()
        {
            blackboard.DeleteVariable(this.m_VariableName);
            return ActionStatus.Success;
        }
    }
}
