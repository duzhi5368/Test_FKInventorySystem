using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(GameObject))]
    [ComponentMenu("GameObject/Compare Tag")]
    public class ComponentCompareTag : Action, ICondition
    {
        [InspectorLabel(LanguagesMacro.EFFECT_TARGET)]
        [SerializeField]
        private TargetType m_Target = TargetType.Self;
        [InspectorLabel(LanguagesMacro.CHECK_TAG)]
        [SerializeField]
        private string m_Tag = "Player";

        public override ActionStatus OnUpdate()
        {
            GameObject target = GetTarget(this.m_Target);
            return target.CompareTag(this.m_Tag) ? ActionStatus.Success : ActionStatus.Failure;
        }
    }
}