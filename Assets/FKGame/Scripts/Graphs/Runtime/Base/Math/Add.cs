﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [System.Serializable]
    [ComponentMenu("Math/Add")]
    [NodeStyle("GraphIcons/Add", false, "Math")]
    public class Add : FlowNode
    {
        [Input(false,true)]
        public float a;
        [Input(false,true)]
        public float b;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return GetInputValue("a", a) + GetInputValue("b", b);
        }
    }
}
