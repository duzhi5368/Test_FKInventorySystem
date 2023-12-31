using UnityEngine.Profiling;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // �ڴ����������Profiler��Ϣ
    public class MemoryDetector
    {
        private readonly static string TotalAllocMemroyFormation = "Alloc Memory : {0}M";
        private readonly static string TotalReservedMemoryFormation = "Reserved Memory : {0}M";
        private readonly static string TotalUnusedReservedMemoryFormation = "Unused Reserved: {0}M";
        private readonly static string MonoHeapFormation = "Mono Heap : {0}M";
        private readonly static string MonoUsedFormation = "Mono Used : {0}M";
        private float ByteToM = 0.000001f;
        private Rect allocMemoryRect;
        private Rect reservedMemoryRect;
        private Rect unusedReservedMemoryRect;
        private Rect monoHeapRect;
        private Rect monoUsedRect;
        private int x = 0;
        private int y = 0;
        private int w = 0;
        private int h = 0;

        public void Init()
        {
            GUIConsole.onGUICallback += OnGUI;
        }

        void ResetGUISize()
        {
            this.x = 5;
            this.y = GUIUtil.FontSize;
            this.w = 1000;
            this.h = GUIUtil.FontSize;

            this.allocMemoryRect = new Rect(x, y, w, h);
            this.reservedMemoryRect = new Rect(x, y + h, w, h);
            this.unusedReservedMemoryRect = new Rect(x, y + 2 * h, w, h);
            this.monoHeapRect = new Rect(x, y + 3 * h, w, h);
            this.monoUsedRect = new Rect(x, y + 4 * h, w, h);
        }

        void OnGUI()
        {
            ResetGUISize();

            GUI.Label(this.allocMemoryRect,
                string.Format(TotalAllocMemroyFormation, Profiler.GetTotalAllocatedMemoryLong() * ByteToM));
            GUI.Label(this.reservedMemoryRect,
                string.Format(TotalReservedMemoryFormation, Profiler.GetTotalReservedMemoryLong() * ByteToM));
            GUI.Label(this.unusedReservedMemoryRect,
                string.Format(TotalUnusedReservedMemoryFormation, Profiler.GetTotalUnusedReservedMemoryLong() * ByteToM));
            GUI.Label(this.monoHeapRect,
                string.Format(MonoHeapFormation, Profiler.GetMonoHeapSizeLong() * ByteToM));
            GUI.Label(this.monoUsedRect,
                string.Format(MonoUsedFormation, Profiler.GetMonoUsedSizeLong() * ByteToM));
        }
    }
}