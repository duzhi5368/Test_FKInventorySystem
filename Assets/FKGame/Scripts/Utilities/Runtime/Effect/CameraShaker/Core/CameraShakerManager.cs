using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class CameraShakerManager
    {
        private static Dictionary<string, CameraShaker> dicData = new Dictionary<string, CameraShaker>();

        public static void AddCameraShaker(string tag, CameraShaker shake)
        {
            if (dicData.ContainsKey(tag))
                Debug.LogError("���ظ���CameraShake tag ��" + tag + " gameObject :" + shake.gameObject.name);
            else
                dicData.Add(tag, shake);
        }
        public static void RemoveCameraShaker(string tag)
        {
            if (dicData.ContainsKey(tag))
                dicData.Remove(tag);
        }

        public static CameraShaker GetCameraShaker(string tag)
        {
            if (dicData.ContainsKey(tag))
                return dicData[tag];
            Debug.LogError("δ�ҵ�GetCameraShake tag��" + tag);
            return null;
        }
    }
}