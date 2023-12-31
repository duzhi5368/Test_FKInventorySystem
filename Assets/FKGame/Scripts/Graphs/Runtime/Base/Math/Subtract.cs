﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{

    [System.Serializable]
    [ComponentMenu("Math/Subtract")]
    [NodeStyle("GraphIcons/Subtract", false,"Math")]
    public class Subtract : FlowNode
    {
        [Input(false, true)]
        public float a;
        [Input(false, true)]
        public float b;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return GetInputValue("a", a) - GetInputValue("b", b);
        }
    }
}