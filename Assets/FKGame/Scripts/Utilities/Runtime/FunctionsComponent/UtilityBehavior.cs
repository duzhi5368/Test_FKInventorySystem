using UnityEngine;
//------------------------------------------------------------------------
// 一些便捷的管理，可做为组件提供给对象使用
//------------------------------------------------------------------------
namespace FKGame
{
    public class UtilityBehavior : MonoBehaviour
    {
        public void QuitApplication() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void LoadScene(string scene) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }

        public void Instantiate(GameObject gameObject) {
            GameObject.Instantiate(gameObject, transform.position, Quaternion.identity);
        }
    }
}