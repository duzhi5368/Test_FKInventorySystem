using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UIModelShowData
    {
        public GameObject top;
        public GameObject root;
        public GameObject model;
        public Camera camera;
        public RenderTexture renderTexture;

        public void Dispose()
        {
            GameObjectManager.DestroyGameObject(model);
            GameObject.Destroy(top);
        }

        public void ChangeModel(string modelName)
        {
            int layer = model.layer;
            GameObjectManager.DestroyGameObject(model);
            model = GameObjectManager.CreateGameObject(modelName);
            model.transform.SetParent(root.transform);
            model.transform.localPosition = new Vector3(0, 0, 0);
            model.transform.localEulerAngles = Vector3.zero;
            model.transform.localScale = Vector3.one;
            model.SetLayer(layer);
        }
    }
}