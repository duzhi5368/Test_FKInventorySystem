namespace FKGame
{
    public class RemoteConsoleModule : AppModuleBase
    {
        public override void OnCreate()
        {
            RemoteConsoleManager.ConsoleStart();
        }
    }
}