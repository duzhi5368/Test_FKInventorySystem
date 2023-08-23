using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.Graphs
{
    public interface IGraphView 
    {
        void OnGUI(Rect position);
        void CenterGraphView();
    }
}