using UnityEditor;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    [CustomPropertyDrawer(typeof(FlowGraph),true)]
    public class FlowGraphPropertyDrawer : GraphPropertyDrawer<FlowGraphView>{}
}