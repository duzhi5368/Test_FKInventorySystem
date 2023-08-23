using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [System.Serializable]
    public class Actions 
    {
        [SerializeReference]
        public List<Action> actions = new List<Action>();
    }
}