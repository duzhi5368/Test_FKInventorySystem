namespace FKGame
{
    public abstract class ServiceBase
    {
        private bool enable = false;
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                if (enable == value)
                    return;
                enable = value;
                if (enable)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }

        protected ServiceManager serviceManager;
        protected IMessageHandler msgManager;
        protected NetworkServerManager netManager;

        public void SetNetworkServerManager(NetworkServerManager netManager)
        {
            this.netManager = netManager;
        }
        public void SetMessageManager(IMessageHandler msgManager)
        {
            this.msgManager = msgManager;
        }
        public void SetServiceManager(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        public virtual void OnInit() { }
        public virtual void OnStart() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnStop() { }
        internal virtual void OnUpdate(float deltaTime) { }
    }
}