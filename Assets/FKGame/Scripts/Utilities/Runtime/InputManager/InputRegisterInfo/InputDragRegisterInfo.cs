namespace FKGame
{
    public class InputDragRegisterInfo : InputEventRegisterInfo<InputUIOnDragEvent>
    {
        public DragAcceptor m_acceptor;
        public InputUIEventDragCallBack m_OnDrag;

        public override void AddListener()
        {
            base.AddListener();
            m_acceptor.m_OnDrag += m_OnDrag;
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            m_acceptor.m_OnDrag -= m_OnDrag;
        }
    }
}