namespace FKGame
{
    public abstract class CodeMessageBase : IMessageClass
    {
        public int code;
        public string e;
        public abstract void DispatchMessage();
    }
}