namespace FKGame
{
    public class InputEndDragRegisterInfo : InputEventRegisterInfo<InputUIOnEndDragEvent>
    {
        public DragAcceptor m_acceptor;
        public InputUIEventDragCallBack m_OnEndDrag;

        public override void AddListener()
        {
            base.AddListener();
            m_acceptor.m_OnEndDrag += m_OnEndDrag;
        }
        public override void RemoveListener()
        {
            base.RemoveListener();
            m_acceptor.m_OnEndDrag -= m_OnEndDrag;
        }
    }
}