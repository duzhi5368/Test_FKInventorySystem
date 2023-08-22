using UnityEngine;
//------------------------------------------------------------------------
// 添加到对象上，根据快捷键可以隐藏鼠标和显示鼠标
// 也可注册回调事件
//------------------------------------------------------------------------
namespace FKGame
{
    public class SetCursorLockState : CallbackHandler
    {
        public KeyCode key = KeyCode.LeftControl;

        public override string[] Callbacks {
            get { return new string[] {"OnCursorLocked","OnCursorUnlocked" }; }
        }

        private void Update()
        {
            CursorLockMode currentMode = Cursor.lockState;

            if (Input.GetKey(key))
            {
                if (currentMode != CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Execute("OnCursorUnlocked", new CallbackEventData());
                }
            }
            else
            {
                if (currentMode != CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Execute("OnCursorLocked", new CallbackEventData());
                }
            }
        }
    }
}