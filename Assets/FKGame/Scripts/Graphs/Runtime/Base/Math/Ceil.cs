﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [System.Serializable]
    [ComponentMenu("Math/Add")]
    [NodeStyle(true, "Math")]
    public class Ceil : FlowNode
    {
        [Input(false,true)]
        public float value;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return Mathf.Ceil(GetInputValue("value", value));
        }
    }
}