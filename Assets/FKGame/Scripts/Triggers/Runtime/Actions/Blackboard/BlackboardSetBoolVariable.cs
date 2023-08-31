using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [ComponentMenu("Blackboard/Set Bool Variable")]
    public class BlackboardSetBoolVariable : Action
    {
        [SerializeField]
        private string m_VariableName = "";
        [SerializeField]
        private bool m_Value = true;

        public override ActionStatus OnUpdate()
        {
            blackboard.SetValue<bool>(this.m_VariableName, this.m_Value);
            return ActionStatus.Success;
        }
    }
}
