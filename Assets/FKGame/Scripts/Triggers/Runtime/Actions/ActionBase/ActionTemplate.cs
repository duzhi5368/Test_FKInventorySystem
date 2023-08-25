using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [CreateAssetMenu(fileName = "ActionTemplate", menuName = "FKGame/触发器/行为组")]
    [System.Serializable]
    public class ActionTemplate : ScriptableObject
    {
        [SerializeReference]
        public List<Action> actions= new List<Action>();
    }
}