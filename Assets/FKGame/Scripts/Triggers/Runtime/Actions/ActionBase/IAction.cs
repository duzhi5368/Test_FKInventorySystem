using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    public interface IAction
    {
        void Initialize(GameObject gameObject, PlayerInfo playerInfo, ComponentBlackboard blackboard);

        bool isActiveAndEnabled { get; }
        void OnSequenceStart();
        void OnStart();
        ActionStatus OnUpdate();
        void Update();
        void OnEnd();
        void OnSequenceEnd();
        void OnInterrupt();
    }
}
