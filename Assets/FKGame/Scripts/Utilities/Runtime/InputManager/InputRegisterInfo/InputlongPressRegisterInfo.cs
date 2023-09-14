namespace FKGame
{
    public class InputlongPressRegisterInfo : InputEventRegisterInfo<InputUILongPressEvent>
    {
        public LongPressAcceptor m_acceptor;
        public InputUIEventLongPressCallBack m_OnLongPress;

        public override void RemoveListener()
        {
            base.RemoveListener();
            m_acceptor.OnLongPress -= m_OnLongPress;
        }

        public override void AddListener()
        {
            base.AddListener();
            m_acceptor.OnLongPress += m_OnLongPress;
        }
    }
}