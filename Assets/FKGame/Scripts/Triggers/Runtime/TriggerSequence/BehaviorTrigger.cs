using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//------------------------------------------------------------------------
// 能自定义行为的触发器
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    public class BehaviorTrigger : BaseTrigger
    {
        public ActionTemplate actionTemplate;

        // 触发器被触发后要执行的动作
        [SerializeReference]
        public List<Action> actions = new List<Action>();
        [SerializeField]
        protected bool m_Interruptable=false;

        // 进行自定义操作的行为
        private Sequence m_ActionBehavior;

        protected AnimatorStateInfo[] m_LayerStateMap;

        private PlayerInfo m_PlayerInfo;

        public override PlayerInfo PlayerInfo {
            get { 
                if (this.m_PlayerInfo == null) {
                    this.m_PlayerInfo = new PlayerInfo("Player");
                }
                return this.m_PlayerInfo;
            }
        }

        protected override void Start()
        {
            base.Start();
            List<ITriggerEventHandler> list = new List<ITriggerEventHandler>(this.m_TriggerEvents);
            list.AddRange(actions.Where(x => x is ITriggerEventHandler).Cast<ITriggerEventHandler>());
            this.m_TriggerEvents = list.ToArray();
            if(actionTemplate != null)
                actionTemplate = Instantiate(actionTemplate);
            this.m_ActionBehavior = new Sequence(gameObject, PlayerInfo, GetComponent<Blackboard>(), actionTemplate != null? actionTemplate.actions.ToArray() : actions.ToArray());
        }

        protected override void Update()
        {
            if (!InRange) { 
                return; 
            }
            // 检查按键是否使用
            if (Input.GetKeyDown(key) && triggerType.HasFlag<TriggerInputType>(TriggerInputType.Key) && InRange && IsBestTrigger())
            {
                Use();
            }
            if (this.m_Interruptable && (this.InUse || this.actions.Count == 0) && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5f))
            {
                NotifyInterrupted();
                this.m_ActionBehavior.Interrupt();  
                return;
            }
            // 更新任务行为
            this.InUse = this.m_ActionBehavior.Tick() || (actions.Count == 0 && (this.actionTemplate == null || actionTemplate.actions.Count==0));
        }

        protected override void OnDisable()
        {
            if (Time.frameCount > 0)
            {
                if (this.m_Interruptable && this.InUse) {
                    NotifyInterrupted();
                    this.m_ActionBehavior.Interrupt();
                }
                if(useDistance > -1)
                    this.InRange = false;
            }
        }

        protected override void OnDestroy()
        {
            if (Time.frameCount > 0)
            {
                if (this.m_Interruptable && this.InUse)
                {
                    NotifyInterrupted();
                    this.m_ActionBehavior.Interrupt();
                }
                this.InRange = false;
            }
        }

        protected void NotifyInterrupted() {
            this.InUse = false;
            OnTriggerInterrupted();
        }

        protected virtual void OnTriggerInterrupted() { }

        protected override void OnTriggerUsed(){
            CacheAnimatorStates();
        }

        protected override void OnTriggerUnUsed()
        {
            this.m_ActionBehavior.Stop();
            LoadCachedAnimatorStates();
        }

        public override bool Use()
        {
            if (!CanUse())
            {
                return false;
            }
            // Trigger.currentUsedTrigger = this;
            this.InUse = true;
            this.m_ActionBehavior.Start();
            return true;
        }

        protected void CacheAnimatorStates()
        {
            if (PlayerInfo == null) return;

            Animator animator = PlayerInfo.animator;
            if (animator != null)
            {
                this.m_LayerStateMap = new AnimatorStateInfo[animator.layerCount];
                for (int j = 0; j < animator.layerCount; j++)
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(j);
                    this.m_LayerStateMap[j] = stateInfo;
                }
            }
        }

        protected void LoadCachedAnimatorStates()
        {
            if (PlayerInfo == null) return;

            Animator animator = PlayerInfo.animator;
            if (animator != null)
            {
                for (int j = 0; j < this.m_LayerStateMap.Length; j++)
                {
                    if (animator.GetCurrentAnimatorStateInfo(j).shortNameHash != this.m_LayerStateMap[j].shortNameHash && !animator.IsInTransition(j))
                    {
                        animator.CrossFadeInFixedTime(this.m_LayerStateMap[j].shortNameHash, 0.15f);
                    }
                }
            }
        }

        public void Execute(ActionTemplate template) {
            IEnumerator behavior = SequenceCoroutine(template.actions.ToArray());
            UnityTools.StartCoroutine(behavior);
        }
        protected IEnumerator SequenceCoroutine(Action[] actions)
        {
            Sequence sequence = new Sequence(gameObject, PlayerInfo, GetComponent<Blackboard>(), actions);
            sequence.Start();
            while (sequence.Tick())
            {
                yield return null;
            }
        }

    }
}