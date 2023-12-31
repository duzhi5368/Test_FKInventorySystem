﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Debug/Log")]
    [System.Serializable]
    public class DebugLog : Action
    {
        [SerializeField]
        private string m_Message = string.Empty;

        public override ActionStatus OnUpdate()
        {
            Debug.Log(this.m_Message);
            return ActionStatus.Success;
        }
    }
}
