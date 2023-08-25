using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Quest")]
    [ComponentMenu(LanguagesMacro.QUEST_SYSTEM + "/" + LanguagesMacro.HAS_QUEST)]
    [System.Serializable]
    public class HasQuest : Action, ICondition
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest requiredQuest;
        [EnumLabel(LanguagesMacro.TASK_STATUS)]
        [SerializeField]
        protected Status m_Status = Status.Completed;

        public override ActionStatus OnUpdate()
        {
            return QuestManager.current.HasQuest(requiredQuest, this.m_Status)?ActionStatus.Success: ActionStatus.Failure;
        }
    }
}