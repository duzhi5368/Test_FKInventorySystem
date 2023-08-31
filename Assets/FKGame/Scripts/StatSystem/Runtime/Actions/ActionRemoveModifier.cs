using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.StatSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Stat System/Add Modifier")]
    [System.Serializable]
    public class RemoveModifier : Action
    {
        [SerializeField]
        private TargetType m_Target = TargetType.Player;

        [InspectorLabel(LanguagesMacro.STAT)]
        [SerializeField]
        protected string m_StatName="Critical Strike";

        private StatsHandler m_Handler;
        public override void OnStart()
        {
            this.m_Handler = this.m_Target == TargetType.Self ? gameObject.GetComponent<StatsHandler>() : playerInfo.gameObject.GetComponent<StatsHandler>();
        }

        public override ActionStatus OnUpdate()
        {
            Stat stat = this.m_Handler.GetStat(this.m_StatName);
            if (stat == null) return ActionStatus.Failure;
            stat.RemoveModifiersFromSource(this.m_Handler.gameObject);
            return ActionStatus.Success;
        }
    }
}
