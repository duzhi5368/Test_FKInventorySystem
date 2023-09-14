using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��������MonoBehaviour��Update ��Unity�����ڵ���MonoBehaviour������ʱ��ͷǳ��˷���Դ
    public static class MonoBehaviourManager
    {
        static List<MonoBehaviourExtend> mlist = new List<MonoBehaviourExtend>();

        private static bool isInit = false;
        private static void Init()
        {
            if (isInit) return;
            isInit = true;

            MonoBehaviourRuntime.Instance.OnUpdate += OnUpdate;
            MonoBehaviourRuntime.Instance.OnFixedUpdate += OnFixedUpdate;
            MonoBehaviourRuntime.Instance.OnLateUpdate += OnLateUpdate;
            MonoBehaviourRuntime.Instance.OnGUIUpdate += OnGUIUpdate;
        }
        public static void Add(MonoBehaviourExtend ex)
        {
            Init();
            if (!mlist.Contains(ex))
            {
                mlist.Add(ex);
                mlist.Sort(Compare);
            }
        }
        public static void Remove(MonoBehaviourExtend ex)
        {
            Init();
            if (mlist.Contains(ex))
                mlist.Remove(ex);
        }
        static int Compare(MonoBehaviourExtend m1, MonoBehaviourExtend m2)
        {
            if (m1.callLevel > m2.callLevel)
                return 1;
            else if (m1.callLevel == m2.callLevel)
                return 0;
            else
                return -1;
        }

        static void OnUpdate()
        {
            for (int i = 0; i < mlist.Count; i++)
            {
                mlist[i].UpdateEx();
            }
        }

        static void OnFixedUpdate()
        {
            for (int i = 0; i < mlist.Count; i++)
            {
                mlist[i].FixedUpdateEx();
            }
        }

        static void OnLateUpdate()
        {
            for (int i = 0; i < mlist.Count; i++)
            {
                mlist[i].LateUpdateEx();
            }
        }

        static private void OnGUIUpdate()
        {
            for (int i = 0; i < mlist.Count; i++)
            {
                mlist[i].OnGUIEx();
            }
        }
    }
}