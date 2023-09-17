using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
//------------------------------------------------------------------------
namespace FKGame
{
    public class PreloadManager : MonoBehaviour
    {
        private static PreloadManager instance = null;
        private static PreloadManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[PreloadManager]").AddComponent<PreloadManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        public static CallBack<int, int> progressCallBack;          // 进度显示 ：当前数量：最大数量
        public static CallBack completedCallBack;                   // 预加载完成
        private int count;
        private int currentNum;
        private List<PreloadResourcesDataGenerate> queueRes = new List<PreloadResourcesDataGenerate>();

        public static void StartLoad(List<PreloadResourcesDataGenerate> otherResList = null)
        {
            Instance.Prepare(otherResList);
        }

        private void Prepare(List<PreloadResourcesDataGenerate> otherResList)
        {
            List<PreloadResourcesDataGenerate> configs = DataGenerateManager<PreloadResourcesDataGenerate>.GetAllDataList();
            if (otherResList != null)
                queueRes.AddRange(otherResList);

            foreach (var item in configs)
            {
                if (item.m_UseLoad)
                {
                    queueRes.Add(item);
                }
            }

            count = queueRes.Count;
            currentNum = 0;
            instance.StartCoroutine(LoadQueue());
        }

        private IEnumerator LoadQueue()
        {
            while (true)
            {
                if (currentNum >= count)
                {
                    RunCallBack();
                    Destroy();
                    break;
                }
                PreloadResourcesDataGenerate da = queueRes[currentNum];
                currentNum++;
                try
                {
                    string typeStr = da.m_ResType.ToString().Replace("_", ".");
                    Type resType = ReflectionUtils.GetTypeByTypeFullName(typeStr);

                    if (resType == typeof(GameObject))
                    {
                        List<GameObject> resList = new List<GameObject>();
                        for (int i = 0; i < da.m_instantiateNum; i++)
                        {
                            GameObject obj = GameObjectManager.CreateGameObjectByPool(da.m_key);
                            if (obj)
                                resList.Add(obj);
                        }
                        foreach (var obj in resList)
                        {
                            GameObjectManager.DestroyGameObjectByPool(obj, !da.m_createInstanceActive);
                        }
                    }
                    else
                    {
                        object loadRes = ResourceManager.Load(da.m_key);
                        if (loadRes == null)
                        {
                            Debug.LogError("Error： 预加载失败  key：" + da.m_key);
                        }
                        else
                            ResourceManager.DestoryAssetsCounter(da.m_key);
                    }
                    RunCallBack();
                    if (currentNum >= count)
                    {
                        Destroy();
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void RunCallBack()
        {
            if (progressCallBack != null)
                progressCallBack(currentNum, count);

            if (count == 0 || currentNum >= count)
            {
                if (completedCallBack != null)
                    completedCallBack();
            }
        }

        private void Destroy()
        {
            Destroy(instance.gameObject);
        }
    }
}