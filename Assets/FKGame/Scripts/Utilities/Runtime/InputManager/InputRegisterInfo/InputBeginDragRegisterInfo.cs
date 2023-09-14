namespace FKGame
{
    public class InputBeginDragRegisterInfo : InputEventRegisterInfo<InputUIOnBeginDragEvent>
    {
        public DragAcceptor m_acceptor;
        public InputUIEventDragCallBack m_OnBeginDrag;

        public override void AddListener()
        {
            base.AddListener();
            m_acceptor.m_OnBeginDrag += m_OnBeginDrag;
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            m_acceptor.m_OnBeginDrag -= m_OnBeginDrag;
        }
    }
}