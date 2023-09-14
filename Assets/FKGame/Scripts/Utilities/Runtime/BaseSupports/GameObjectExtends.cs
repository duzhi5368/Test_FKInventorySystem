using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class GameObjectExtends
    {
        // �ݹ�ı��ӽڵ��Scale
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

        // �ݹ��޸��ӽڵ�Ĳ㼶
        public static void SetLayer(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform item in obj.transform)
            {
                item.gameObject.layer = layer;
                SetLayer(item.gameObject, layer);
            }
        }
        // �Ż�������SetActive���������Խ�Լ�ظ�����Active�Ŀ���
        public static void SetActiveOptimize(this GameObject go, bool isActive)
        {
            if (go.activeSelf != isActive)
            {
                go.SetActive(isActive);
            }
        }
    }
}