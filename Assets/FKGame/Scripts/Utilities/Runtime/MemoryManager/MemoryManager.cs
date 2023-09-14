using System;
using UnityEngine.Profiling;
using UnityEngine;
using System.Text;
//------------------------------------------------------------------------
namespace FKGame
{
    public delegate void LoadProgressCallBack(LoadState loadState);

    public enum MemoryEvent
    {
        FreeHeapMemory, //释放堆内存
        FreeMemory,     //释放全部内存
    }
    public class MemoryManager
    {
        public static bool OpenAutoMemoryClean = true;          // 是否开启内存回收（默认开启）
        public static double totalReservedMemory;
        public static double totalAllocatedMemory;
        public static float freeMemory = 0;
        public static float usedMemory = 0;
        public static float allMemory = 0;
        private static float UpdateMemoryTime = 0.5f;
        private static float tempTime = 0;

        public static void Init()
        {
            ApplicationManager.s_OnApplicationLateUpdate += Update;
            Application.lowMemory += OnLowMemoryCallBack;
            if (ApplicationManager.AppMode != AppMode.Release)
                DevelopReplayManager.s_ProfileGUICallBack += GUI;
        }

        static void Update()
        {
            MonitorMemorySize();        // 内存管理

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F12))
            {
                FreeMemory();
                AssetsUnloadHandler.UnloadAll();
            }
#endif
        }


        static void GUI()
        {
           StringBuilder showGUIStr = new StringBuilder();
           showGUIStr.Append("总内存：" + (int)allMemory + "M"+ "\n");
           showGUIStr.Append("使用内存：" + (int)usedMemory + "M" + "\n");
           showGUIStr.Append("空闲内存：" + (int)freeMemory + "M" + "\n");
           showGUIStr.Append("内存阈值：" + (int)MemoryInfo.GetMemoryLimit() + "M" + "\n");
           showGUIStr.Append("已加载资源：" + AssetsUnloadHandler.usedAssetsDic.Count + "\n");
           showGUIStr.Append("可回收资源：" + AssetsUnloadHandler .noUsedAssetsList.Count+ "\n");
           GUIStyle style = new GUIStyle("Box");
           style.fontSize = 20;
           style.richText = true;
           style.alignment = TextAnchor.UpperLeft;
           GUILayout.Box(showGUIStr.ToString(),style);
        }

        // 判断已使用的内存是否超过内存阈值
        public static bool NeedReleaseMemory()
        {
            float mLimit = MemoryInfo.GetMemoryLimit();
            if (mLimit == -1)
                return false;
            return usedMemory >= mLimit;
        }

        private static void OnLowMemoryCallBack()
        {
            Debug.LogWarning("低内存警告！！！");
            if (Application.platform != RuntimePlatform.Android)
            {
                FreeMemory();
                AssetsUnloadHandler.UnloadAll();
            }
        }

        // 释放内存
        public static void FreeMemory()
        {
            GlobalEvent.DispatchEvent(MemoryEvent.FreeMemory);
            UIManager.DestroyAllHideUI();       // 清空缓存的UI
            GameObjectManager.CleanPool();      // 清空对象池
            LanguageManager.Release();
            AssetsUnloadHandler.UnloadAll();
            FreeHeapMemory();
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        // 释放堆内存
        public static void FreeHeapMemory()
        {
            DataManager.CleanCache();
            ConfigManager.CleanCache();
            RecordManager.CleanCache();
        }

        // 用于监控内存
        static void MonitorMemorySize()
        {
            if (ApplicationManager.AppMode != AppMode.Release)
            {
                totalReservedMemory = ByteToM(Profiler.GetTotalReservedMemoryLong());
                totalAllocatedMemory = ByteToM(Profiler.GetTotalAllocatedMemoryLong());
            }
            if (tempTime <= 0)
            {
                tempTime = UpdateMemoryTime;
                if (MemoryInfo.GetMemoryInfo())
                {
                    freeMemory = MemoryInfo.minf.memfree / 1024f / 1024f;
                    allMemory = MemoryInfo.minf.memtotal / 1024f / 1024f;
                    usedMemory = MemoryInfo.minf.memused / 1024f / 1024f;
                }
            }
            else
            {
                tempTime -= Time.deltaTime;
            }

            AssetsUnloadHandler.LateUpdate();
            if (NeedReleaseMemory() && OpenAutoMemoryClean)
            {
                AssetsUnloadHandler.UnloadOne();
            }
        }

        static double ByteToM(long byteCount)
        {
            return (double)(byteCount / (1024.0f * 1024.0f));
        }
    }
}