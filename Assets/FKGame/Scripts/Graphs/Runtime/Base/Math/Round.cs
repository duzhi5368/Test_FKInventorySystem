using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [System.Serializable]
    [ComponentMenu("Math/Round")]
    [NodeStyle("GraphIcons/Round", false, "Math")]
    public class Round : FlowNode
    {
        [Input(false,true)]
        public float value;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return Mathf.Round(GetInputValue("value", value));
        }
    }
}