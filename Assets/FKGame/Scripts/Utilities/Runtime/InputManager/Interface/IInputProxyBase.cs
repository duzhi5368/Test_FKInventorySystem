namespace FKGame
{
    public abstract class IInputProxyBase
    {
        // ÊÇ·ñÊÇ¼¤»î×´Ì¬
        private static bool s_isActive = true;
        public static bool IsActive
        {
            get { return IInputProxyBase.s_isActive; }
            set { IInputProxyBase.s_isActive = value; }
        }

        public static T GetEvent<T>(string eventKey) where T : IInputEventBase, new()
        {
            T tmp = HeapObjectPool<T>.GetObject();
            tmp.EventKey = eventKey;

            return tmp;
        }
    }
}