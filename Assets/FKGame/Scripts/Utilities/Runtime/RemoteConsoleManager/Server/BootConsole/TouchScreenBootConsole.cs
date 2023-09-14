using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    class TouchScreenBootConsole : IBootFunctionBase
    {

        private System.Action OnTriggerBoot;
        private RemoteConsoleSettingData config;
        private bool isBoot = false;
        public int tapCount = 0;

        public void OnInit(RemoteConsoleSettingData config, System.Action OnTriggerBoot)
        {
            this.config = config;
            this.OnTriggerBoot = OnTriggerBoot;
        }

        public void OnGUI(){}
        
        public void OnUpdate()
        {
            if (isBoot)
                return;
            if (Input.touchCount == 1)
            {
                Touch myTouch = Input.touches[0];
                if (myTouch.tapCount >= tapCount &&
                    (myTouch.position.x > 0 && myTouch.position.x < Screen.width / 3) &&
                    (myTouch.position.y > (Screen.height - Screen.height / 3) && myTouch.position.y < Screen.height)
                    )
                {
                    Debug.Log("TouchScreenBootConsole.OnTriggerBoot");
                    if (OnTriggerBoot != null)
                    {
                        OnTriggerBoot();
                    }
                    isBoot = true;
                }
            }
        }
    }
}