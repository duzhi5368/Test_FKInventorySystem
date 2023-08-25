﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [System.Serializable]
    [ComponentMenu("Math/Pow")]
    [NodeStyle("GraphIcons/Pow", false, "Math")]
    public class Pow : FlowNode
    {
        [Input(false,true)]
        public float f;
        [Input(false, true)]
        public float p;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return Mathf.Pow(GetInputValue("f", f),GetInputValue("p",p));
        }
    }
}