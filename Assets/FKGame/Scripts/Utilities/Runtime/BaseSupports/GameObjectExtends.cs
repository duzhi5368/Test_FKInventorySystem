using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class GameObjectExtends
    {
        // 递归改变子节点的Scale
        public static void SetScale(this Transform tr, Vector3 scale, bool recursion = true)
        {
            tr.localScale = scale;
            if (recursion)
            {
                foreach (Transform item in tr)
                {
                    SetScale(item, scale);
                }
            }
        }

        // 递归修改子节点的层级
        public static void SetLayer(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform item in obj.transform)
            {
                item.gameObject.layer = layer;
                SetLayer(item.gameObject, layer);
            }
        }
        // 优化的设置SetActive方法，可以节约重复设置Active的开销
        public static void SetActiveOptimize(this GameObject go, bool isActive)
        {
            if (go.activeSelf != isActive)
            {
                go.SetActive(isActive);
            }
        }
    }
}