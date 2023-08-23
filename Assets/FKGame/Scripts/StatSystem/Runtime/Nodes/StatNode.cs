using FKGame.StatSystem;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [System.Serializable]
    public abstract class StatNode : FlowNode
    {
        [Input(true, false)]
        public string stat = string.Empty;
        [Output]
        public Stat statValue;

        public StatNode() { }
    }
}