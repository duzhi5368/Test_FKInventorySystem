using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Quest")]
    [ComponentMenu(LanguagesMacro.QUEST_SYSTEM + "/" + LanguagesMacro.HAS_QUEST_TASK)]
    [System.Serializable]
    public class ActionHasQuestTask : Action, ICondition
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest m_Quest;
        [InspectorLabel(LanguagesMacro.TASK_NAME)]
        [SerializeField]
        protected string m_Task="";
        [EnumLabel(LanguagesMacro.TASK_STATUS)]
        [SerializeField]
        protected Status m_Status = Status.Completed;

        public override ActionStatus OnUpdate()
        {
            Quest instance;
            if (QuestManager.current.HasQuest(this.m_Quest, out instance)) {
                return instance.tasks.Find(x => x.Name == this.m_Task && x.Status == this.m_Status) != null ? ActionStatus.Success : ActionStatus.Failure;
            }
            return ActionStatus.Failure;
        }
    }
}