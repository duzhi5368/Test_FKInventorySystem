using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public enum RenderEventEnum
    {
        UpdateLOD
    }

    // 粒子系统LOD管理器
    public class ParticleLODService : PoolObject
    {
        public Transform m_LOD_1;
        public Transform m_LOD_2;
        public Transform m_LOD_3;

        static int s_LOD = 3;   // 细节等级，值目前在0-3之间
        static int MaxLOD = 3;
        static int MinLOD = 0;

        public static int LOD
        {
            get { return s_LOD; }
            set
            {
                s_LOD = value;
                if (s_LOD > MaxLOD)
                {
                    s_LOD = MaxLOD;
                }
                if (s_LOD < MinLOD)
                {
                    s_LOD = MinLOD;
                }
                Shader.globalMaximumLOD = s_LOD * 100;
                GlobalEvent.DispatchEvent(RenderEventEnum.UpdateLOD);
            }
        }


#if UNITY_EDITOR
        public void Reset()
        {
            m_LOD_1 = transform.Find("LOD_1");
            m_LOD_2 = transform.Find("LOD_2");
            m_LOD_3 = transform.Find("LOD_3");
        }
#endif

        public override void OnCreate()
        {
            ResetLOD(LOD);
            GlobalEvent.AddEvent(RenderEventEnum.UpdateLOD, ReceviceUpdateLOD);
        }

        public override void OnObjectDestroy()
        {
            GlobalEvent.RemoveEvent(RenderEventEnum.UpdateLOD, ReceviceUpdateLOD);
        }

        public void ResetLOD(int LOD)
        {
            if (m_LOD_3 != null)
            {
                m_LOD_3.gameObject.SetActive(LOD >= 3);
            }
            if (m_LOD_2 != null)
            {
                m_LOD_2.gameObject.SetActive(LOD >= 2);
            }
        }

        public void ReceviceUpdateLOD(params object[] objs)
        {
            ResetLOD(LOD);
        }
    }
}