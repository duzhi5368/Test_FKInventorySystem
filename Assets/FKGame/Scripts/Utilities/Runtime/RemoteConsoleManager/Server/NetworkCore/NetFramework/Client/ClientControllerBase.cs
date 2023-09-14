namespace FKGame
{
    public class ClientControllerBase
    {
        private bool enable = false;
        protected IMessageHandler msgManager;
        protected NetworkClientManager netManager;
        protected ClientControllerManager controllerManager;

        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                if (enable && value)
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

        public void SetMessageManager(IMessageHandler msgManager)
        {
            this.msgManager = msgManager;
        }
        public void SetNetworkClientManager(NetworkClientManager netManager)
        {
            this.netManager = netManager;
        }
        public void SetNetControllerManager(ClientControllerManager controllerManager)
        {
            this.controllerManager = controllerManager;
        }

        public virtual void OnInit() { }
        public virtual void OnStart() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnStop() { }
        public virtual void OnUpdate(float deltaTime) { }
    }
}