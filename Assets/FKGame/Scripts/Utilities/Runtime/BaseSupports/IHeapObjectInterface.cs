namespace FKGame
{
    public interface IHeapObjectInterface
    {
        void OnInit();

        /// <summary>
        /// ȡ��ʱ����
        /// </summary>
        void OnPop();

        /// <summary>
        /// �Ż�ʱ����
        /// </summary>
        void OnPush();
    }
}