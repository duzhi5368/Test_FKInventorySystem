using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [System.Serializable]
    [ComponentMenu("Math/Sqrt")]
    [NodeStyle("GraphIcons/Sqrt", false, "Math")]
    public class Sqrt : FlowNode
    {
        [Input(false,true)]
        public float value;
        [Output]
        public float output;

        public override object OnRequestValue(Port port)
        {
            return Mathf.Sqrt(GetInputValue("value", value));
        }
    }
}