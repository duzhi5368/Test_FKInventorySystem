using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        // 更换场景不要删除
        public bool dontDestroyOnLoad = true;

        public event Quest.StatusChanged OnQuestStatusChanged;
        public event Quest.TaskStatusChanged OnTaskStatusChanged;
        public event Quest.TaskProgressChanged OnTaskProgressChanged;
        public event Quest.TaskTimerTick OnTaskTimerTick;

        private static QuestManager m_Current;
        public static QuestManager current
        {
            get
            {
                Assert.IsNotNull(m_Current, "需要一个任务管理器，请通过 Tools > FKGame > 任务系统 > 创建任务管理器 创建一个。");
                return m_Current;
            }
        }

        // 任务数据库
        [SerializeField]
        private QuestDatabase m_Database = null;
        public static QuestDatabase Database
        {
            get
            {
                if (QuestManager.current != null)
                {
                    Assert.IsNotNull(QuestManager.current.m_Database, "请先将 任务数据库 分配到 任务管理器中");
                    return QuestManager.current.m_Database;
                }
                return null;
            }
        }

        private static Default m_DefaultSettings;
        public static Default DefaultSettings
        {
            get
            {
                if (m_DefaultSettings == null)
                {
                    m_DefaultSettings = GetSetting<Default>();
                }
                return m_DefaultSettings;
            }
        }

        private static UI m_UI;
        public static UI UI
        {
            get
            {
                if (m_UI == null)
                {
                    m_UI = GetSetting<UI>();
                }
                return m_UI;
            }
        }

        private static QuestNotifications m_Notifications;
        public static QuestNotifications Notifications
        {
            get
            {
                if (m_Notifications == null)
                {
                    m_Notifications = GetSetting<QuestNotifications>();
                }
                return m_Notifications;
            }
        }

        private static SavingLoading m_SavingLoading;
        public static SavingLoading SavingLoading
        {
            get
            {
                if (m_SavingLoading == null)
                {
                    m_SavingLoading = GetSetting<SavingLoading>();
                }
                return m_SavingLoading;
            }
        }

        private static T GetSetting<T>() where T : Settings
        {
            if (QuestManager.Database != null)
            {
                return (T)QuestManager.Database.settings.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
            }
            return default(T);
        }

        private PlayerInfo m_PlayerInfo;
        public PlayerInfo PlayerInfo
        {
            get
            {
                if (this.m_PlayerInfo == null) { this.m_PlayerInfo = new PlayerInfo(QuestManager.DefaultSettings.playerTag); }
                return this.m_PlayerInfo;
            }
        }

        public List<Quest> AllQuests {
            get {
                List<Quest> quests = new List<Quest>();
                quests.AddRange(this.ActiveQuests);
                quests.AddRange(this.CompletedQuests);
                quests.AddRange(this.FailedQuests);
                return quests.Distinct().ToList();
            }
        }

        [HideInInspector]
        public List<Quest> ActiveQuests = new List<Quest>();
        private List<Quest> CompletedQuests = new List<Quest>();
        private List<Quest> FailedQuests = new List<Quest>();

        private void Awake()
        {
            if (m_Current != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                m_Current = this;
                if (dontDestroyOnLoad){
                    if (transform.parent != null)
                    {
                        if (DefaultSettings.debugMessages)
                            Debug.Log("Quest Manager with DontDestroyOnLoad can't be a child transform. Unparent!");
                        transform.parent = null;
                    }
                    DontDestroyOnLoad(gameObject);
                }
                if (QuestManager.SavingLoading.autoSave)
                    Load();

                Debug.Log("Quest Manager initialized.");
            }
        }

        public static void Save()
        {
            string key = PlayerPrefs.GetString(QuestManager.SavingLoading.savingKey, QuestManager.SavingLoading.savingKey);
            Save(key);
        }

        public static void Save(string key)
        {
            string activeQuestData = JsonSerializer.Serialize(QuestManager.current.ActiveQuests.ToArray());
            string completedQuestData = JsonSerializer.Serialize(QuestManager.current.CompletedQuests.ToArray());
            string failedQuestData =JsonSerializer.Serialize(QuestManager.current.FailedQuests.ToArray());

            PlayerPrefs.SetString(key + ".ActiveQuests", activeQuestData);
            PlayerPrefs.SetString(key + ".CompletedQuests", completedQuestData);
            PlayerPrefs.SetString(key + ".FailedQuests", failedQuestData);

            List<string> keys = PlayerPrefs.GetString("QuestSystemSavedKeys").Split(';').ToList();
            keys.RemoveAll(x => string.IsNullOrEmpty(x));
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }
            PlayerPrefs.SetString("QuestSystemSavedKeys", string.Join(";", keys));
            if (DefaultSettings.debugMessages)
            {
                Debug.Log("[任务系统] 任务已保存");
            }
        }

        public static void Load()
        {
            string key = PlayerPrefs.GetString(QuestManager.SavingLoading.savingKey, QuestManager.SavingLoading.savingKey);
            Load(key);
        }

        public static void Load(string key) { 

            string activeQuestData = PlayerPrefs.GetString(key + ".ActiveQuests");
            string completedQuestData = PlayerPrefs.GetString(key + ".CompletedQuests");
            string failedQuestData = PlayerPrefs.GetString(key + ".FailedQuests");

            LoadQuests(activeQuestData, ref QuestManager.current.ActiveQuests, true);
            LoadQuests(completedQuestData, ref QuestManager.current.CompletedQuests);
            LoadQuests(failedQuestData, ref QuestManager.current.FailedQuests);

            if (DefaultSettings.debugMessages)
            {
                Debug.Log("[任务系统] 任务已加载");
            }
        }

        private static void LoadQuests(string json, ref List<Quest> quests, bool registerCallbacks = false)
        {
            if (string.IsNullOrEmpty(json)) 
                return;
            List<object> list = MiniJSON.Deserialize(json) as List<object>;
            for (int i = 0; i < list.Count; i++)
            {
                Dictionary<string, object> mData = list[i] as Dictionary<string, object>;
                string name = (string)mData["Name"];
                Quest quest = QuestManager.Database.items.FirstOrDefault(x => x.Name == name);
                if (quest != null)
                {
                    Quest instance = Instantiate(quest);
                    for (int j = 0; j < instance.tasks.Count; j++)
                    {
                        instance.tasks[j].owner = instance;
                    }
                    instance.SetObjectData(mData);
                    if (registerCallbacks) {
                        instance.OnStatusChanged += QuestManager.current.NotifyQuestStatusChanged;
                        instance.OnTaskStatusChanged += QuestManager.current.NotifyTaskStatusChanged;
                        instance.OnTaskProgressChanged += QuestManager.current.NotifyTaskProgressChanged;
                        instance.OnTaskTimerTick += QuestManager.current.NotifyTaskTimerTick;
                    }
                    quests.Add(instance);
                }
                else {
                    Debug.LogWarning("Failed to laod quest "+name+". Quest is not present in Database.");
                }
            }
            if (QuestManager.DefaultSettings.debugMessages)
            {
                Debug.Log("[任务系统] 任务已加载");
            }
        }

        public void AddQuest(Quest quest) {
            if (this.ActiveQuests.Contains(quest)) return;
            quest.OnStatusChanged += NotifyQuestStatusChanged;
            quest.OnTaskStatusChanged += NotifyTaskStatusChanged;
            quest.OnTaskProgressChanged += NotifyTaskProgressChanged;
            quest.OnTaskTimerTick += NotifyTaskTimerTick;
            this.ActiveQuests.Add(quest);
            
        }

        public void RemoveQuest(Quest quest)
        {
            quest.OnStatusChanged -= NotifyQuestStatusChanged;
            quest.OnTaskStatusChanged -= NotifyTaskStatusChanged;
            quest.OnTaskProgressChanged -= NotifyTaskProgressChanged;
            quest.OnTaskTimerTick -= NotifyTaskTimerTick;
            this.ActiveQuests.Remove(quest);
        }

        private void NotifyQuestStatusChanged(Quest quest) {
            OnQuestStatusChanged?.Invoke(quest);
            if (quest.Status == Status.Completed && !this.CompletedQuests.Contains(quest)) {
                this.CompletedQuests.Add(quest);
                RemoveQuest(quest);
            }
            if (quest.Status == Status.Failed && !this.FailedQuests.Contains(quest)) {
                if (quest.RestartFailed){
                    quest.Reset();
                }else{
                    this.FailedQuests.Add(quest);
                    RemoveQuest(quest);
                }
            }
            if (quest.Status == Status.Canceled)
            {
                if (quest.RestartCanceled)
                {
                    RemoveQuest(quest);
                    quest.Reset();
                }
            }
            if (QuestManager.SavingLoading.autoSave)
                QuestManager.Save();
        }

        private void NotifyTaskStatusChanged(Quest quest, QuestTask task) {
            OnTaskStatusChanged?.Invoke(quest, task);
            if (QuestManager.SavingLoading.autoSave)
                QuestManager.Save();
        }

        private void NotifyTaskProgressChanged(Quest quest, QuestTask task)
        {
            OnTaskProgressChanged?.Invoke(quest, task);
            if (QuestManager.SavingLoading.autoSave)
                QuestManager.Save();
        }

        private void NotifyTaskTimerTick(Quest quest, QuestTask task) {
            OnTaskTimerTick?.Invoke(quest, task);
            if (QuestManager.SavingLoading.autoSave)
                QuestManager.Save();
        }

        public Quest GetQuest(string name) {
            return this.ActiveQuests.FirstOrDefault(x => x.Name == name);
        }

        public bool HasQuest(Quest quest, Status status) {
            return AllQuests.FirstOrDefault(x => x.Status == status && x.Name == quest.Name);
        }

        public bool HasQuest(Quest quest, out Quest instance)
        {
            instance = AllQuests.FirstOrDefault(x => x.Name == quest.Name);
            return instance != null;
        }
    }
}