﻿using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(GameObject))]
    [ComponentMenu("GameObject/SendMessage")]
    public class ComponentSendMessage : Action
    {
        [InspectorLabel(LanguagesMacro.EFFECT_TARGET)]
        [SerializeField]
        private TargetType m_Target = TargetType.Player;
        [InspectorLabel(LanguagesMacro.METHOD_NAME)]
        [SerializeField]
        private string methodName = string.Empty;
        [SerializeField]
        private ArgumentVariable m_Argument = null;
        [InspectorLabel(LanguagesMacro.SEND_MESSAGE_OPTIONS)]
        [SerializeField]
        private SendMessageOptions m_Options = SendMessageOptions.DontRequireReceiver;

        private GameObject m_TargetÓbject;

        public override void OnStart()
        {
            this.m_TargetÓbject = GetTarget(this.m_Target);
        }

        public override ActionStatus OnUpdate()
        {
            if (m_Argument.ArgumentType != ArgumentType.None)
            {
                this.m_TargetÓbject.SendMessage(methodName, m_Argument.GetValue(), m_Options);
            }
            else
            {
                this.m_TargetÓbject.SendMessage(methodName, m_Options);
            }
            return ActionStatus.Success;
        }
    }
}