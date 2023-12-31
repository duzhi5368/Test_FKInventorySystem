using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    [RequireComponent(typeof(AspectRatioFitter))]
    [RequireComponent(typeof(Image))]
    public class AspectRatioFitterHelper : MonoBehaviour
    {
        [ContextMenu("Set Aspect Ratio")]
        void Start()
        {
            Rect rect = GetComponent<Image>().sprite.rect;
            AspectRatioFitter fitter = GetComponent<AspectRatioFitter>();
            fitter.aspectRatio = rect.width / rect.height;
        }
    }
}