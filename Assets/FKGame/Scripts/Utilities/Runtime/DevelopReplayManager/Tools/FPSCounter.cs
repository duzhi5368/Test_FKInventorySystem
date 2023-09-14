using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ֡�ʼ�����
    public class FPSCounter
    {
        // ֡�ʼ���Ƶ��
        private const float calcRate = 0.5f;
        // ���μ���Ƶ����֡��
        private int frameCount = 0;
        // Ƶ��ʱ��
        private float rateDuration = 0f;
        // ��ʾ֡��
        private int fps = 0;

        public void Init()
        {
            if (ApplicationManager.AppMode != AppMode.Release)
            {
                ApplicationManager.s_OnApplicationUpdate += Update;
                DevelopReplayManager.s_ProfileGUICallBack += OnGUI;
            }
        }

        void Start()
        {
            this.frameCount = 0;
            this.rateDuration = 0f;
            this.fps = 0;
        }

        void Update()
        {
            ++this.frameCount;
            this.rateDuration += Time.unscaledDeltaTime;
            if (this.rateDuration > calcRate)
            {
                // ����֡��
                this.fps = (int)(this.frameCount / this.rateDuration);
                this.frameCount = 0;
                this.rateDuration = 0f;
            }
        }

        void OnGUI()
        {
            GUIStyle style = new GUIStyle("Box");
            style.fontSize = 20;
            style.richText = true;
            style.alignment = TextAnchor.UpperLeft;
            GUILayout.Box("FPS��" + fps.ToString(), style);
        }
    }
}