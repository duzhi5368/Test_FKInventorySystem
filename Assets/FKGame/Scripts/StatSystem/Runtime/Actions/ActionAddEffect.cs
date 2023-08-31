using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Stat System/Add Effect")]
    [System.Serializable]
    public class ActionAddEffect : Action
    {
        [InspectorLabel(LanguagesMacro.EFFECT_TARGET)]
        [SerializeField]
        private TargetType m_Target = TargetType.Player;
        [InspectorLabel(LanguagesMacro.EFFECT_TYPE)]
        [SerializeField]
        protected StatEffect m_Effect;
        private StatsHandler m_Handler;

        public override void OnStart()
        {
            this.m_Handler = this.m_Target == TargetType.Self ? gameObject.GetComponent<StatsHandler>() : playerInfo.gameObject.GetComponent<StatsHandler>();
        }

        public override ActionStatus OnUpdate()
        {
            this.m_Handler.AddEffect(this.m_Effect);
            return ActionStatus.Success;
        }
    }
}
