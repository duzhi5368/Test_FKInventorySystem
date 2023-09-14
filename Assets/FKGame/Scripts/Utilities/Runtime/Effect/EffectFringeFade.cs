using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 边缘淡化效果
    public class EffectFringeFade : MonoBehaviour
    {
        void Start()
        {
            bool isPortrait = Screen.height > Screen.width;     // 是否是竖屏
            Rect safeArea = Screen.safeArea;
#if UNITY_EDITOR
            if (Screen.width == 1125 && Screen.height == 2436)
            {
                safeArea.y = 102;
                safeArea.height = 2202;
            }
            if (Screen.width == 2436 && Screen.height == 1125)
            {
                safeArea.x = 132;
                safeArea.y = 63;
                safeArea.height = 1062;
                safeArea.width = 2172;
            }
#endif
            if (Application.platform == RuntimePlatform.IPhonePlayer || ApplicationManager.AppMode == AppMode.Developing)
            {
                float x = Screen.width - safeArea.width;
                if (x > 0)
                {
                    Vector2 offsetMin = GetComponent<RectTransform>().offsetMin;
                    if (isPortrait)
                    {
                        offsetMin.y = x * 0.5f;
                    }
                    else
                    {
                        offsetMin.x = x * 0.5f;
                    }
                    GetComponent<RectTransform>().offsetMin = offsetMin;
                    Vector2 offsetMax = GetComponent<RectTransform>().offsetMax;
                    if (isPortrait)
                    {
                        offsetMax.y = -x * 0.5f;
                    }
                    else
                    {
                        offsetMax.x = -x * 0.5f;
                    }
                    GetComponent<RectTransform>().offsetMax = offsetMax;
                }
                Debug.LogWarning(GetComponent<RectTransform>().offsetMax);
            }
        }
    }
}