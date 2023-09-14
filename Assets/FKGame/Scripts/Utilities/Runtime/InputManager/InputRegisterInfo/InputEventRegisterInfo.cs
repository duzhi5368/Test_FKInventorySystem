namespace FKGame
{
    public class InputEventRegisterInfo<T> : InputEventRegisterInfo where T : IInputEventBase
    {
        public InputEventHandle<T> callBack;
        public InputEventRegisterInfo(){}

        // 添加监听和派发
        public override void AddListener()
        {
            InputManager.AddListener<T>(eventKey, callBack);
        }

        // 移除监听和派发
        public override void RemoveListener()
        {
            InputManager.RemoveListener<T>(eventKey, callBack);
            HeapObjectPool<InputEventRegisterInfo<T>>.PutObject(this);
        }
    }
}