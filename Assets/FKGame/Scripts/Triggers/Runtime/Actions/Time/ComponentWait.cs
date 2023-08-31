using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Time/Wait")]
    [Icon("Time")]
    public class ComponentWait : Action
    {
        [InspectorLabel(LanguagesMacro.WAIT_TIME)]
        [SerializeField]
        private float duration = 1f;

        private float m_Time = 0f;

        public override void OnStart()
        {
            this.m_Time = 0f;
        }

        public override ActionStatus OnUpdate()
        {
            this.m_Time += Time.deltaTime;
            if (this.m_Time > duration)
            {
                return ActionStatus.Success;
            }
            return ActionStatus.Running;
        }
    }
}
