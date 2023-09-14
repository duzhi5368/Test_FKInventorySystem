using UnityEngine.UI;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GuideHeightLightComponent : MonoBehaviour
    {
        public int order = 1;
        private GuideChangeData status = new GuideChangeData();
        private bool RunGuide = false;

        private void OnEnable()
        {
            RunGuide = true;
        }

        public void ClearGuide()
        {
            Canvas canvas = gameObject.GetComponent<Canvas>();
            GraphicRaycaster graphic = gameObject.GetComponent<GraphicRaycaster>();
            if (graphic != null && status.isCreateGraphic)
            {
                Destroy(graphic);
            }
            if (canvas != null && status.isCreateCanvas)
            {
                Destroy(canvas);
            }
            else
            {
                if (canvas != null)
                {
                    canvas.overrideSorting = status.OldOverrideSorting;
                    canvas.sortingOrder = status.OldSortingOrder;
                    canvas.sortingLayerName = status.oldSortingLayerName;
                }
            }
        }

        public void SetHeightLight()
        {
            Canvas canvas = GetComponent<Canvas>();
            GraphicRaycaster graphic = GetComponent<GraphicRaycaster>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
                status.isCreateCanvas = true;
            }
            if (graphic == null)
            {
                graphic = gameObject.AddComponent<GraphicRaycaster>();
                status.isCreateGraphic = true;
            }
            status.OldOverrideSorting = canvas.overrideSorting;
            status.OldSortingOrder = canvas.sortingOrder;
            status.oldSortingLayerName = canvas.sortingLayerName;
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;
            canvas.sortingLayerName = "Guide";
        }
        
        private void Update()
        {
            if (RunGuide)
            {
                RunGuide = false;
                SetHeightLight();
            }
        }

        protected struct GuideChangeData
        {
            public bool isCreateCanvas;
            public bool isCreateGraphic;
            public string oldSortingLayerName;
            public int OldSortingOrder;
            public bool OldOverrideSorting;
        }
    }
}