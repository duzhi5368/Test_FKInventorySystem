﻿using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [System.Serializable]
    public abstract class Action : IAction
    {
        [HideInInspector]
        [SerializeField]
        private string m_Type;
        [HideInInspector]
        [SerializeField]
        private bool m_Enabled = true;
        public bool enabled {
            get { return this.m_Enabled; }
            set { this.m_Enabled = value; }
        }

        public bool isActiveAndEnabled { get { return enabled && (gameObject == null || gameObject.activeSelf); } }

        protected PlayerInfo playerInfo;
        protected GameObject gameObject;
        protected ComponentBlackboard blackboard;

        public Action() {
            this.m_Type = GetType().FullName;
        }

        public void Initialize(GameObject gameObject, PlayerInfo playerInfo, ComponentBlackboard blackboard) {
            this.gameObject = gameObject;
            this.playerInfo = playerInfo;
            this.blackboard = blackboard; 
        }

        public abstract ActionStatus OnUpdate();

        public virtual void Update() { }

        public virtual void OnStart(){}

        public virtual void OnEnd(){}

        public virtual void OnSequenceStart(){}

        public virtual void OnSequenceEnd(){}

        public virtual void OnInterrupt() { }

        protected GameObject GetTarget(TargetType type)
        {
            switch (type)
            {
                case TargetType.Player:
                    return playerInfo.gameObject;
                case TargetType.Camera:
                    return Camera.main.gameObject;
            }
            return gameObject;
        }
    }

    public enum TargetType {
        [Header("自己")]
        Self,
        [Header("玩家")]
        Player,
        [Header("镜头")]
        Camera
    }
}
