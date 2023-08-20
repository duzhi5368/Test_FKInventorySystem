using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame.Graphs
{
    public interface IGraphProvider 
    {
        Graph GetGraph();
    }
}