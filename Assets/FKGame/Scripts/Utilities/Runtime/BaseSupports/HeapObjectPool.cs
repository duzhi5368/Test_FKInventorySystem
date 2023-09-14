using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class HeapObjectPool
    {
        public static Dictionary<string, object> GetSODict()
        {
            return HeapObjectPool<Dictionary<string, object>>.GetObject();
        }

        public static void PutSODict(Dictionary<string, object> dict)
        {
            dict.Clear();
            HeapObjectPool<Dictionary<string, object>>.PutObject(dict);
        }
    }

    public static class HeapObjectPool<T> where T : new()
    {
        static Stack<T> s_pool = new Stack<T>();

        public static int GetCount()
        {
            return s_pool.Count;
        }

        public static T GetObject()
        {
            T obj;
            IHeapObjectInterface heapObj;
            if (s_pool.Count > 0)
            {
                obj = s_pool.Pop();
                heapObj = obj as IHeapObjectInterface;
            }
            else
            {
                obj = new T();
                heapObj = obj as IHeapObjectInterface;
                if (heapObj != null)
                {
                    heapObj.OnInit();
                }
            }
            if (heapObj != null)
            {
                heapObj.OnPop();
            }
            return obj;
        }

        public static void PutObject(T obj)
        {
            IHeapObjectInterface heapObj = obj as IHeapObjectInterface;
            if (heapObj != null)
            {
                heapObj.OnPush();
            }
            s_pool.Push(obj);
        }
    }
}