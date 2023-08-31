using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
// 全屏窗口/非全屏设置
//------------------------------------------------------------------------
namespace FKGame.UIWidgets
{
    public class UIFullscreen : MonoBehaviour
    {
		private const string FULL_SCREEN_KEY = "FullScreen";

        private void Start()
        {
            bool fullScreen = PlayerPrefs.GetInt(FULL_SCREEN_KEY, Screen.fullScreen?1:0)==1? true:false;
            SetFullscreen(fullScreen);

            Toggle toggle = GetComponent<Toggle>();
            if (toggle != null) {
                toggle.isOn = fullScreen;   
                toggle.onValueChanged.AddListener(SetFullscreen);
            }
        }

        public void SetFullscreen(bool fullScreen)
		{
			Screen.fullScreen = fullScreen;
			PlayerPrefs.SetInt(FULL_SCREEN_KEY,fullScreen?1:0);
		}
	}
}