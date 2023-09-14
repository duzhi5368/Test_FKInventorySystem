using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class KeyboardBootConsole : IBootFunctionBase
    {
        private System.Action OnTriggerBoot;
        private RemoteConsoleSettingData config;
        private bool isBoot = false;

        public void OnInit(RemoteConsoleSettingData config, System.Action OnTriggerBoot)
        {
            this.config = config;
            this.OnTriggerBoot = OnTriggerBoot;
            Debug.Log("KeyboardBootConsole.init");
        }

        public void OnGUI(){}

        public void OnUpdate()
        {
            if (isBoot)
                return;
            if (Input.GetKey(KeyCode.F12)
                &&
                Input.GetKey(KeyCode.A) &&
                Input.GetKey(KeyCode.LeftShift)
                )
            {
                Debug.Log("KeyboardBootConsole.OnTriggerBoot");
                if (OnTriggerBoot != null)
                {
                    OnTriggerBoot();
                }
                isBoot = true;
            }
        }
    }
}