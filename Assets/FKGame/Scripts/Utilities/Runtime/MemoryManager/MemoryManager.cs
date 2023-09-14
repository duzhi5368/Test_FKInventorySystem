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
        FreeHeapMemory, //�ͷŶ��ڴ�
        FreeMemory,     //�ͷ�ȫ���ڴ�
    }
    public class MemoryManager
    {
        public static bool OpenAutoMemoryClean = true;          // �Ƿ����ڴ���գ�Ĭ�Ͽ�����
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
            MonitorMemorySize();        // �ڴ����

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
           showGUIStr.Append("���ڴ棺" + (int)allMemory + "M"+ "\n");
           showGUIStr.Append("ʹ���ڴ棺" + (int)usedMemory + "M" + "\n");
           showGUIStr.Append("�����ڴ棺" + (int)freeMemory + "M" + "\n");
           showGUIStr.Append("�ڴ���ֵ��" + (int)MemoryInfo.GetMemoryLimit() + "M" + "\n");
           showGUIStr.Append("�Ѽ�����Դ��" + AssetsUnloadHandler.usedAssetsDic.Count + "\n");
           showGUIStr.Append("�ɻ�����Դ��" + AssetsUnloadHandler .noUsedAssetsList.Count+ "\n");
           GUIStyle style = new GUIStyle("Box");
           style.fontSize = 20;
           style.richText = true;
           style.alignment = TextAnchor.UpperLeft;
           GUILayout.Box(showGUIStr.ToString(),style);
        }

        // �ж���ʹ�õ��ڴ��Ƿ񳬹��ڴ���ֵ
        public static bool NeedReleaseMemory()
        {
            float mLimit = MemoryInfo.GetMemoryLimit();
            if (mLimit == -1)
                return false;
            return usedMemory >= mLimit;
        }

        private static void OnLowMemoryCallBack()
        {
            Debug.LogWarning("���ڴ澯�棡����");
            if (Application.platform != RuntimePlatform.Android)
            {
                FreeMemory();
                AssetsUnloadHandler.UnloadAll();
            }
        }

        // �ͷ��ڴ�
        public static void FreeMemory()
        {
            GlobalEvent.DispatchEvent(MemoryEvent.FreeMemory);
            UIManager.DestroyAllHideUI();       // ��ջ����UI
            GameObjectManager.CleanPool();      // ��ն����
            LanguageManager.Release();
            AssetsUnloadHandler.UnloadAll();
            FreeHeapMemory();
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        // �ͷŶ��ڴ�
        public static void FreeHeapMemory()
        {
            DataManager.CleanCache();
            ConfigManager.CleanCache();
            RecordManager.CleanCache();
        }

        // ���ڼ���ڴ�
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