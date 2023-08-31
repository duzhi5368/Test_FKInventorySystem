using FKGame.Macro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    [System.Serializable]
    public class Quest : ScriptableObject, INameable, IJsonSerializable
    {
        public delegate void StatusChanged(Quest quest);
        public delegate void TaskStatusChanged(Quest quest, QuestTask task);
        public delegate void TaskProgressChanged(Quest quest, QuestTask task);
        public delegate void TaskTimerTick(Quest quest, QuestTask task);

        public event StatusChanged OnStatusChanged;
        public event TaskStatusChanged OnTaskStatusChanged;
        public event TaskProgressChanged OnTaskProgressChanged;
        public event TaskTimerTick OnTaskTimerTick;

        [HeaderLine(LanguagesMacro.QUEST_DEFAULT_TITLE)]
        [InspectorLabel(LanguagesMacro.QUEST_NAME)]
        [SerializeField]
        private string m_QuestName = string.Empty;
        public string Name
        {
            get { return this.m_QuestName; }
            set { this.m_QuestName = value; }
        }
        [InspectorLabel(LanguagesMacro.QUEST_TITLE)]
        [SerializeField]
        protected string m_Title;
        public string Title
        {
            get { return this.m_Title; }
            set { this.m_Title = value; }
        }

        [TextArea(4, 4)]
        [SerializeField]
        protected string m_Description;
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }

        [InspectorLabel(LanguagesMacro.IS_AUTO_COMPLETE)]
        [SerializeField]
        protected bool m_AutoComplete = false;
        public bool AutoComplete
        {
            get { return this.m_AutoComplete; }
        }
        [InspectorLabel(LanguagesMacro.IS_RESTART_FAILED)]
        [SerializeField]
        protected bool m_RestartFailed = true;
        public bool RestartFailed
        {
            get { return this.m_RestartFailed; }
        }
        [InspectorLabel(LanguagesMacro.IS_RESTART_CANCELED)]
        [SerializeField]
        protected bool m_RestartCanceled = true;
        public bool RestartCanceled
        {
            get { return this.m_RestartCanceled; }
        }
        [InspectorLabel(LanguagesMacro.IS_RESTART_COMPLETED)]
        [SerializeField]
        protected bool m_RestartCompleted = false;
        public bool RestartCompleted
        {
            get { return this.m_RestartCompleted; }
        }

        [SerializeReference]
        public List<QuestReward> rewards = new List<QuestReward>();

        [HeaderLine(LanguagesMacro.TASKS)]
        [SerializeField]
        [EnumLabel(LanguagesMacro.TASK_ORDER_RELATIONSHIP)]
        protected TaskExecution m_TaskExecution;
        public TaskExecution TaskExecution {
            get { return this.m_TaskExecution; }
        }

        [HeaderLine(LanguagesMacro.CONDITIONS)]
        [SerializeReference]
        public List<ICondition> conditions = new List<ICondition>();

        [SerializeReference]
        public List<QuestTask> tasks = new List<QuestTask>();

        protected Status m_Status= Status.Inactive;
        public Status Status {
            get { return this.m_Status; }
            protected set {
                if (this.m_Status != value)
                {
                    this.m_Status = value;
                    NotifyStatusChange(this);
                }
            }
        }

        public virtual void DisplayReward(RectTransform parent)
        {
            for (int i = 0; i < rewards.Count; i++)
                 rewards[i].DisplayReward(parent,i);
        }

        public void Activate() {
            if (!CanActivate()) 
                return;
            QuestManager.current.AddQuest(this);
            Status = Status.Active;
            if (AutoComplete && CanComplete())
                Complete();
        }

        public void Cancel()
        {
            if (!CanCancel()) 
                return;
            Status = Status.Canceled;
        }

        public void Decline()
        {
            if (!CanDecline()) 
                return;
            Status = Status.Inactive;
        }

        public void Complete()
        {
            if (!CanComplete()) 
                return;
            Status = Status.Completed;
        }

        public bool CanActivate() {
            for (int i = 0; i < conditions.Count; i++) {
                ICondition condition = conditions[i];
                condition.Initialize(QuestManager.current.PlayerInfo.gameObject, QuestManager.current.PlayerInfo, QuestManager.current.PlayerInfo.gameObject.GetComponent<ComponentBlackboard>());
                condition.OnStart();
                if (condition.OnUpdate() == ActionStatus.Failure)
                {
                    condition.OnEnd();
                    return false;
                }
            }
            return Status == Status.Inactive || (Status == Status.Canceled && this.m_RestartCanceled);
        }

        public bool CanDecline()
        {
            return Status == Status.Inactive;
        }

        public bool CanCancel() {
            return Status == Status.Active;
        }

        public bool CanComplete() {
            return Status == Status.Active && tasks.All(x => x.Status == Status.Completed || x.Optional);
        }

        public void NotifyStatusChange(Quest quest)
        {
            OnStatusChanged?.Invoke(quest);
            if (quest.Status == Status.Active)
            {
                if (this.TaskExecution == TaskExecution.Parallel){
                    for (int i = 0; i < tasks.Count; i++) {
                        tasks[i].Activate();
                    }
                }else {
                    var next = tasks.FirstOrDefault(x => x.Status == Status.Inactive);

                    if (next != null)
                    {
                        next.Activate();
                    }
                }  
            }
            if (quest.Status == Status.Completed)
            {
                QuestManager.Notifications.questCompleted.Show(quest.Title);
                for (int i = 0; i < quest.tasks.Count; i++)
                    quest.tasks[i].OnQuestCompleted();
                for (int i = 0; i < quest.rewards.Count; i++)
                    quest.rewards[i].GiveReward();
                if (quest.RestartCompleted)
                {
                    quest.Reset();
                }
            }
            if (quest.Status == Status.Failed)
                QuestManager.Notifications.questFailed.Show(quest.Title);
        }

        public void NotifyTaskStatusChange(QuestTask task) {
            OnTaskStatusChanged?.Invoke(this, task);
            if (task.Status == Status.Completed) {
                QuestManager.Notifications.taskCompleted.Show(task.Description);
                var next = tasks.FirstOrDefault(x => x.Status == Status.Inactive);
                if (next != null){
                    next.Activate();
                }
            }
            if (task.Status == Status.Failed) {
                QuestManager.Notifications.taskFailed.Show(task.Description);
                if (!task.Optional)
                {
                    task.owner.Status = Status.Failed;
                }
                else {
                    var next = tasks.FirstOrDefault(x => x.Status == Status.Inactive);
                    if (next != null)
                    {
                        next.Activate();
                    }
                }
            }
            if (AutoComplete && CanComplete())
                Complete();
        }

        public void NotifyTaskProgressChange(QuestTask task) {
            OnTaskProgressChanged?.Invoke(this, task);
        }

        public void NotifyTaskTimerTick(QuestTask task) {
            OnTaskTimerTick?.Invoke(this, task);
        }

        public virtual void Reset() {
            Status = Status.Inactive;
            for (int i = 0; i < tasks.Count; i++) {
                tasks[i].Reset();
            }
        }

        public QuestTask GetTask(string name) {
            return tasks.FirstOrDefault(x => x.Name == name);
        }

        public virtual void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", this.Name);
            data.Add("Status", (int)this.Status);
            if (tasks.Count > 0)
            {
                List<object> mTasks = new List<object>();
                for (int i = 0; i < tasks.Count; i++)
                {
                    QuestTask task = tasks[i];
                    if (task != null)
                    {
                        Dictionary<string, object> taskData = new Dictionary<string, object>();
                        task.GetObjectData(taskData);
                        mTasks.Add(taskData);
                    }
                    else
                    {
                        mTasks.Add(null);
                    }
                }
                data.Add("Tasks", mTasks);
            }
        }

        public virtual void SetObjectData(Dictionary<string, object> data)
        {
            string name = (string)data["Name"];
            this.m_Status = (Status)data["Status"];

            if (data.ContainsKey("Tasks"))
            {
                List<object> mTasks = data["Tasks"] as List<object>;
                for (int i = 0; i < mTasks.Count; i++)
                {
                    Dictionary<string, object> taskData = mTasks[i] as Dictionary<string, object>;
                    if (taskData != null)
                    {
                        QuestTask task = tasks.FirstOrDefault(x => x.Name == (string)taskData["Name"]);
                        task.owner = this;
                        if(task != null)
                            task.SetObjectData(taskData);
                    }
                }
            }
        }
    }

    public enum TaskExecution {
        [Header("线性任务")]
        Single,
        [Header("并行任务")]
        Parallel
    }

    public enum Status:int
    {
        [Header("未进行")]
        Inactive = 0,
        [Header("进行中")]
        Active = 1,
        [Header("已完成")]
        Completed = 2,
        [Header("失败")]
        Failed = 3,
        [Header("已取消")]
        Canceled = 4
    }
}