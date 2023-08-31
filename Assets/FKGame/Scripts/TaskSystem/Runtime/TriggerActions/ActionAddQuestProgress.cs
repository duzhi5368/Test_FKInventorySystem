using FKGame.Macro;
using System.Linq;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [Icon("Quest")]
    [ComponentMenu(LanguagesMacro.QUEST_SYSTEM + "/" + LanguagesMacro.SHOW_WINDOW)]
    [System.Serializable]
    public class ActionAddQuestProgress : Action
    {
        [QuestPicker(true)]
        [SerializeField]
        protected Quest quest;
        [InspectorLabel("Task")]
        [SerializeField]
        protected string m_TaskName;
        [SerializeField]
        protected float m_Progress = 1f;

        public override ActionStatus OnUpdate()
        {
            Quest current = QuestManager.current.GetQuest(quest.Name);

            if (current != null && current.Status== Status.Active)
            {
                QuestTask task = current.tasks.FirstOrDefault(x => x.Name == m_TaskName);
                if (task.Status == Status.Active)
                {
                    task.AddProgress(this.m_Progress);
                    return ActionStatus.Success;
                }
            }
            return ActionStatus.Failure;
        }
    }
}
