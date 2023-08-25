using UnityEngine;
using System.Linq;
using FKGame.UIWidgets;
using FKGame.Macro;
//------------------------------------------------------------------------
namespace FKGame.QuestSystem
{
    public class QuestTrigger : BaseTrigger
    {
        public static QuestWindow currentUsedWindow;

        public override PlayerInfo PlayerInfo => QuestManager.current.PlayerInfo;

        [Header("Selection")]
        [SerializeField]
        protected string m_Title = LanguagesMacro.AVAILIBLE_QUESTS;
        [SerializeField]
        protected string m_Text = LanguagesMacro.SELECT_A_QUEST_NOTICE;

        protected QuestCollection m_QuestCollection;

        protected override void Start()
        {
            base.Start();
            this.m_QuestCollection = GetComponent<QuestCollection>();
        }

        public override bool CanUse()
        {
            Quest quest = GetNextQuest();
            return base.CanUse() && quest != null;
        }

        public override bool Use()
        {
            if (!CanUse())
            {
                return false;
            }
            this.InUse = true;
            currentUsedWindow = QuestManager.UI.questWindow;
            //currentUsedWindow.Show(GetNextQuest());
            
            for (int i = 0; i < this.m_QuestCollection.Count; i++) {
                Quest quest = this.m_QuestCollection[i];
                if (quest.CanComplete())
                {
                    currentUsedWindow.Show(quest);
                    return true;
                }
            }

            string[] quests = this.m_QuestCollection.Where(x => x.CanActivate()).Select(y => y.Name).ToArray();
            if (quests.Length > 1)
            {
                DialogBox questSelection = QuestManager.UI.questSelectionWindow;
                Debug.Log(questSelection);
                questSelection.RegisterListener("OnClose", (CallbackEventData eventData) => {
                    InUse = false;
                });

                questSelection.Show(this.m_Title, this.m_Text, (int result) => {
                    currentUsedWindow.Show(this.m_QuestCollection.FirstOrDefault(x => x.Name == quests[result]));
                }, quests);
            }else if(quests.Length == 1) {
                currentUsedWindow.Show(this.m_QuestCollection.FirstOrDefault(x=>x.Name == quests[0]));
            }
            return true;
        }

        private Quest GetNextQuest()
        {
            for (int i = 0; i < this.m_QuestCollection.Count; i++)
            {
                Quest quest = this.m_QuestCollection[i];
                if (quest.CanComplete())
                {
                    return quest;
                }
            }
            for (int i = 0; i < this.m_QuestCollection.Count; i++)
            {
                Quest quest = this.m_QuestCollection[i];
                if (quest.CanActivate())
                {
                    return quest;
                }
            }
            return null;
        }

        protected override void DisplayInUse()
        {
            QuestManager.Notifications.inUse.Show();
        }

        protected override void DisplayOutOfRange()
        {
            QuestManager.Notifications.toFarAway.Show();
        }

        protected override void OnWentOutOfRange()
        {
            if (currentUsedWindow != null)
            {
                currentUsedWindow.Close();
                QuestTrigger.currentUsedWindow = null;
            }
        }
    }
}