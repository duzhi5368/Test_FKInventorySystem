using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    [RequireComponent(typeof(Button))]
    public class ComponentAudioButton : MonoBehaviour
    {
        public string audioName = "";
        public float volume = 1f;

        void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (ResourcesConfigManager.GetIsExitRes(audioName))
            {
                AudioManager.PlaySFX2D(audioName, volume);
            }
            else
            {
                Debug.LogError("不存在音频文件：" + audioName);
            }
        }
    }
}