using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GameObjectTool
    {
        // 重置位置、旋转 、缩放
        public static void ResetTransform(GameObject go, bool isLocal = true)
        {
            if (isLocal)
            {
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                go.transform.position = Vector3.zero;
                go.transform.eulerAngles = Vector3.zero;
            }

            go.transform.localScale = Vector3.one;
        }

        // 设置父节点并重置
        public static void SetParentAndReset(GameObject go_child, GameObject go_parent)
        {
            go_child.transform.SetParent(go_parent.transform);
            ResetTransform(go_child);
        }
    }
}