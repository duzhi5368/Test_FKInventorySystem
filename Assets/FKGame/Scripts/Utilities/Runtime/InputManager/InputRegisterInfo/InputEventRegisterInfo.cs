namespace FKGame
{
    public class InputEventRegisterInfo<T> : InputEventRegisterInfo where T : IInputEventBase
    {
        public InputEventHandle<T> callBack;
        public InputEventRegisterInfo(){}

        // ��Ӽ������ɷ�
        public override void AddListener()
        {
            InputManager.AddListener<T>(eventKey, callBack);
        }

        // �Ƴ��������ɷ�
        public override void RemoveListener()
        {
            InputManager.RemoveListener<T>(eventKey, callBack);
            HeapObjectPool<InputEventRegisterInfo<T>>.PutObject(this);
        }
    }
}