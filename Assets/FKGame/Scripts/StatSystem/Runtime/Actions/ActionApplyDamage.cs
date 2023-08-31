using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Stat System/Apply Damage")]
    [System.Serializable]
    public class ActionApplyDamage : Action
    {
        [SerializeField]
        private TargetType m_Target = TargetType.Player;

        [InspectorLabel(LanguagesMacro.STAT)]
        [SerializeField]
        protected string m_StatName="Health";
        [SerializeField]
        protected float m_Value = 50f;
       
        private StatsHandler m_Handler;

        public override void OnStart()
        {
            this.m_Handler = this.m_Target == TargetType.Self ? gameObject.GetComponent<StatsHandler>() : playerInfo.gameObject.GetComponent<StatsHandler>();
        }

        public override ActionStatus OnUpdate()
        {
            this.m_Handler.ApplyDamage(this.m_StatName, this.m_Value);
            return ActionStatus.Success;
        }
    }
}
