namespace FKGame
{
    public interface UILifeCycleInterface
    {
        void Init(string UIEventKey, int id = 0);
        void Dispose();
    }
}