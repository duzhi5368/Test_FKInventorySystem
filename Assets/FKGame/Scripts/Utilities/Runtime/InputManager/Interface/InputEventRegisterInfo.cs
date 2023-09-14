namespace FKGame
{
    public class InputEventRegisterInfo : IHeapObjectInterface
    {
        public string eventKey;
        public void OnInit() { }
        public void OnPop() { }
        public void OnPush() { }
        public virtual void AddListener() { }
        public virtual void RemoveListener() { }
    }
}