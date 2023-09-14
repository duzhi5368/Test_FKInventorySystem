namespace FKGame
{
    public class RemoteConsoleModuleAdapter : AppModuleBase
    {
        public override void OnCreate()
        {
            RemoteConsoleServerStarter.ConsoleStart();
        }
    }
}