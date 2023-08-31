using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
// 触发器（地面物品，或者临接NPC）
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    public abstract class BaseTrigger : ComponentCallbackHandler
    {
        public abstract PlayerInfo PlayerInfo { get; }
        public override string[] Callbacks
        {
            get
            {
                return new[] {
                    "OnTriggerUsed",
                    "OnTriggerUnUsed",
                    "OnCameInRange",
                    "OnWentOutOfRange",
                };
            }
        }

        // Trigger最大使用距离
        public float useDistance = 1.2f;
        [EnumFlags]
        public TriggerInputType triggerType = TriggerInputType.LeftClick | TriggerInputType.Key;
        // 如果已在触发范围内，可使用该键进行Trigger触发
        public KeyCode key = KeyCode.F;

        // 自定义Trigger的触发回调函数
        protected ITriggerEventHandler[] m_TriggerEvents;
        // 当前被触发的Trigger
        public static BaseTrigger currentUsedTrigger;


        // 区域范围内的Trigger
        private static List<BaseTrigger> m_TriggerInRange = new List<BaseTrigger>();
        // 回调函数列表
        protected Dictionary<Type, string> m_CallbackHandlers;

        protected delegate void EventFunction<T>(T handler, GameObject player);
        protected delegate void PointerEventFunction<T>(T handler, PointerEventData eventData);

        protected bool m_CheckBlocking = true;
        protected bool m_Started = false;

        // 玩家是否在Trigger范围内（或者触发器初始就已挂接到玩家）
        private bool m_InRange;
        public bool InRange
        {
            get
            {
                return this.m_InRange;
            }
            protected set
            {
                if (this.m_InRange != value)
                {
                    this.m_InRange = value;
                    if (this.m_InRange){
                        NotifyCameInRange();
                    }else{
                        NotifyWentOutOfRange();
                    }
                }
            }
        }

        // Trigger是否正在被使用
        private bool m_InUse;
        public bool InUse
        {
            get { return this.m_InUse; }
            set
            {
                if (this.m_InUse != value)
                {
                    this.m_InUse = value;
                    if (!this.m_InUse){
                        NotifyUnUsed();
                    }else{
                        NotifyUsed();
                    }
                }
            }
        }

        protected virtual void Start()
        {
            this.RegisterCallbacks();
            this.m_TriggerEvents = GetComponentsInChildren<ITriggerEventHandler>();
            if (PlayerInfo.gameObject == null && useDistance != -1) {
                useDistance = -1;
                Debug.LogWarning("There is no Player in scene! Please set Use Distance to -1 to ignore range check in "+gameObject+".");
            }
            if (PlayerInfo.gameObject == null && triggerType.HasFlag<TriggerInputType>(TriggerInputType.OnTriggerEnter))
            {
                Debug.LogWarning("OnTriggerEnter is only valid with a Player in scene. Please remove OnTriggerEnter in "+gameObject+".");
                triggerType = TriggerInputType.LeftClick;
            }

            ComponentEventHandler.Register<int>(gameObject, "OnPoinerClickTrigger", OnPointerTriggerClick);

            if (gameObject == PlayerInfo.gameObject || this.useDistance == -1) {
                InRange = true;
            }
            else{
                // 创建触发碰撞器
                CreateTriggerCollider();
            }
            this.m_Started = true;
        }

        protected virtual void OnDisable() {
            if (Time.frameCount > 0){
                this.InRange = false;
            }
        }

        protected virtual void OnEnable()
        {
            if (Time.frameCount > 0 && this.m_Started && PlayerInfo.transform != null && useDistance > -1)
                InRange = Vector3.Distance(transform.position, PlayerInfo.transform.position) <= this.useDistance;
        }


        protected virtual void Update() {
            if (!InRange) { 
                return; 
            }
            // 检查按键是否按下，是否支持按键
            if (Input.GetKeyDown(key) && triggerType.HasFlag<TriggerInputType>(TriggerInputType.Key) && InRange && IsBestTrigger()){
                Use();
            }
        }

        protected virtual void OnDestroy()
        {
            // 检查用户是否已退出游戏
            if (Time.frameCount > 0)
            {
                InRange = false;    // 关闭Trigger的触发
            }
        }

        // 进入了触发器区域的回调函数
        protected virtual void OnTriggerEnter(Collider other)
        {
            // 检查进入触发器区域的是否是主角
            if (isActiveAndEnabled && PlayerInfo.gameObject != null && other.tag == PlayerInfo.gameObject.tag)
            {
                InRange = true;     // 认为角色进入了触发器
            }
        }

        // 离开了触发器区域的回调函数
        protected virtual void OnTriggerExit(Collider other)
        {
            // 检查进入触发器区域的是否是主角
            if (isActiveAndEnabled && PlayerInfo.gameObject != null && other.tag == PlayerInfo.gameObject.tag)
            {
                InRange = false;    // 认为角色离开了触发器
            }
        }

        private void OnPointerTriggerClick(int button)
        {
            if (!UnityTools.IsPointerOverUI() &&
                   triggerType.HasFlag<TriggerInputType>(TriggerInputType.LeftClick) && button == 0 ||
                   triggerType.HasFlag<TriggerInputType>(TriggerInputType.RightClick) && button == 1 ||
                   triggerType.HasFlag<TriggerInputType>(TriggerInputType.MiddleClick) && button == 2)
            {
                Use();
            }
        }

        // 使用触发器
        public virtual bool Use()
        {
            // 触发器是否可用
            if (!CanUse())
            {
                return false;
            }
            // BaseTrigger.currentUsedTrigger = this;
            this.InUse = true;
            return true;
        }

        // 触发器是否可用
        public virtual bool CanUse()
        {
            // 触发器正在被使用，则返回false
            if (InUse || (BaseTrigger.currentUsedTrigger != null && BaseTrigger.currentUsedTrigger.InUse)){
                DisplayInUse();
                return false;
            }
            if (this.useDistance == -1) { 
                return true; 
            }
            // 玩家不在区域内，则返回false
            if (!InRange){
                DisplayOutOfRange();
                return false;
            }
            Animator animator = PlayerInfo.animator;
            if (PlayerInfo != null && animator != null){
                for (int j = 0; j < animator.layerCount; j++){
                    if (animator.IsInTransition(j))
                        return false;
                }
            }

            return true;
        }

        protected virtual void OnWentOutOfRange() { }

        protected void NotifyWentOutOfRange(){
            ExecuteEvent<ITriggerWentOutOfRange>(Execute, true);
            BaseTrigger.m_TriggerInRange.Remove(this);
            this.InUse = false;
            OnWentOutOfRange();
        }

        protected virtual void OnCameInRange() { }

        protected void NotifyCameInRange() {
            ExecuteEvent<ITriggerCameInRange>(Execute, true);
            BaseTrigger.m_TriggerInRange.Add(this);
            if (triggerType.HasFlag<TriggerInputType>(TriggerInputType.OnTriggerEnter) && IsBestTrigger()){
                this.m_CheckBlocking = false;
                Use();
                this.m_CheckBlocking = true;
            }
            OnCameInRange();
        }

        protected virtual void OnTriggerUsed() { }

        private void NotifyUsed() {
            BaseTrigger.currentUsedTrigger = this;
            ExecuteEvent<ITriggerUsedHandler>(Execute);
            OnTriggerUsed();
        }

        protected virtual void OnTriggerUnUsed() { }

        protected void NotifyUnUsed() {
            ExecuteEvent<ITriggerUnUsedHandler>(Execute, true);
            BaseTrigger.currentUsedTrigger = null;
            OnTriggerUnUsed();
        }

        // 通知玩家他已触发一个Trigger
        protected virtual void DisplayInUse() { }

        // 通知玩家他超处了范围
        protected virtual void DisplayOutOfRange() { }

        // 创建一个球体碰撞器，以便进行Trigger区域检测
        protected virtual void CreateTriggerCollider()
        {
            Vector3 position = Vector3.zero;
            GameObject handlerGameObject = new GameObject("TriggerRangeHandler");
            handlerGameObject.transform.SetParent(transform,false);
            handlerGameObject.layer = 2;

            Collider collider = GetComponent<Collider>();
            if (collider != null){
                position = collider.bounds.center;
                position.y = (collider.bounds.center - collider.bounds.extents).y;
                position = transform.InverseTransformPoint(position);
            }

            SphereCollider sphereCollider = handlerGameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.center = position;
            Vector3 scale = transform.lossyScale;
            sphereCollider.radius = useDistance / Mathf.Max(scale.x, scale.y, scale.z);

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null) {
                rigidbody =gameObject.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
            }
        }

        // 检查是否是最佳触发器。（根据用户 输入热键 以及 用户和触发器 范围进行判断 ）
        public virtual bool IsBestTrigger()
        {
            if (gameObject == PlayerInfo.gameObject){
                return true;
            }

            BaseTrigger tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = PlayerInfo.transform.position;
            foreach (BaseTrigger t in BaseTrigger.m_TriggerInRange)
            {
                if (t.key != key) continue;
                Vector3 dir = t.transform.position - currentPos;
                float angle = 0f;
                if (dir != Vector3.zero)
                   angle = Quaternion.Angle(PlayerInfo.transform.rotation, Quaternion.LookRotation(dir));
                float dist = Vector3.Distance(t.transform.position, currentPos) * angle;
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin == this;
        }

        protected static void Execute(ITriggerUsedHandler handler, GameObject player)
        {
            handler.OnTriggerUsed(player);
        }

        protected static void Execute(ITriggerUnUsedHandler handler, GameObject player)
        {
            handler.OnTriggerUnUsed(player);
        }

        protected static void Execute(ITriggerCameInRange handler, GameObject player)
        {
            handler.OnCameInRange(player);
        }

        protected static void Execute(ITriggerWentOutOfRange handler, GameObject player)
        {
            handler.OnWentOutOfRange(player);
        }

        // 执行事件
        protected void ExecuteEvent<T>(EventFunction<T> func, bool includeDisabled = false) where T : ITriggerEventHandler
        {
            for (int i = 0; i < this.m_TriggerEvents.Length; i++)
            {
                ITriggerEventHandler handler = this.m_TriggerEvents[i];
                if (ShouldSendEvent<T>(handler, includeDisabled))
                {
                    func.Invoke((T)handler, PlayerInfo.gameObject);
                }
            }
            string eventID = string.Empty;
            if (this.m_CallbackHandlers.TryGetValue(typeof(T), out eventID))
            {
                CallbackEventData triggerEventData = new CallbackEventData();
                triggerEventData.AddData("Trigger", this);
                triggerEventData.AddData("Player", PlayerInfo.gameObject);
                triggerEventData.AddData("EventData", new PointerEventData(EventSystem.current));
                base.Execute(eventID, triggerEventData);
            }
        }

        protected void ExecuteEvent<T>(PointerEventFunction<T> func, PointerEventData eventData, bool includeDisabled = false) where T : ITriggerEventHandler
        {
            for (int i = 0; i < this.m_TriggerEvents.Length; i++)
            {
                ITriggerEventHandler handler = this.m_TriggerEvents[i];
                if (ShouldSendEvent<T>(handler, includeDisabled))
                {
                    func.Invoke((T)handler, eventData);
                }
            }
            string eventID = string.Empty;
            if (this.m_CallbackHandlers.TryGetValue(typeof(T), out eventID))
            {
                CallbackEventData triggerEventData = new CallbackEventData();
                triggerEventData.AddData("Trigger", this);
                triggerEventData.AddData("Player", PlayerInfo.gameObject);
                triggerEventData.AddData("EventData", new PointerEventData(EventSystem.current));
                base.Execute(eventID, triggerEventData);
            }
        }

        // 检查是否应该执行该事件
        protected bool ShouldSendEvent<T>(ITriggerEventHandler handler, bool includeDisabled)
        {
            var valid = handler is T;
            if (!valid)
                return false;
            var behaviour = handler as Behaviour;
            if (behaviour != null && !includeDisabled)
                return behaviour.isActiveAndEnabled;

            return true;
        }

        // 注册回调函数
        protected virtual void RegisterCallbacks()
        {
            this.m_CallbackHandlers = new Dictionary<Type, string>();
            this.m_CallbackHandlers.Add(typeof(ITriggerUsedHandler), "OnTriggerUsed");
            this.m_CallbackHandlers.Add(typeof(ITriggerUnUsedHandler), "OnTriggerUnUsed");
            this.m_CallbackHandlers.Add(typeof(ITriggerCameInRange), "OnCameInRange");
            this.m_CallbackHandlers.Add(typeof(ITriggerWentOutOfRange), "OnWentOutOfRange");
        }

        [System.Flags]
        public enum TriggerInputType
        {
            LeftClick = 1,
            RightClick = 2,
            MiddleClick = 4,
            Key = 8,
            OnTriggerEnter = 16,
        }
    }
}