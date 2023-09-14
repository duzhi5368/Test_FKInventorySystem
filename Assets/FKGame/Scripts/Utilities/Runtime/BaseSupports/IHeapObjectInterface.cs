namespace FKGame
{
    public interface IHeapObjectInterface
    {
        void OnInit();

        /// <summary>
        /// 取出时调用
        /// </summary>
        void OnPop();

        /// <summary>
        /// 放回时调用
        /// </summary>
        void OnPush();
    }
}