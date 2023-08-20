using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame
{
    [CreateAssetMenu(fileName = "ActionTemplate", menuName = "FKGame/Triggers/Action Template")]
    [System.Serializable]
    public class ActionTemplate : ScriptableObject
    {
        [SerializeReference]
        public List<Action> actions= new List<Action>();
    }
}