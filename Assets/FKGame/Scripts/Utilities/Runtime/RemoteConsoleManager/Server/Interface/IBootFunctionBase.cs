namespace FKGame
{
    public interface IBootFunctionBase
    {
        void OnInit(RemoteConsoleSettingData config, System.Action OnTriggerBoot);
        void OnUpdate();
        void OnGUI();
    }
}