using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 被对象池管理的对象
    public class PoolObject : MonoBehaviour
    {
        public bool SetActive = true;

        /// <summary>
        /// 对象初始化时调用
        /// </summary>
        public virtual void OnCreate()
        {

        }

        /// <summary>
        /// 从对象池中取出时调用
        /// </summary>
        public virtual void OnFetch()
        {

        }

        /// <summary>
        /// 被回收时调用
        /// </summary>
        public virtual void OnRecycle()
        {

        }

        /// <summary>
        /// 被销毁时调用
        /// </summary>
        public virtual void OnObjectDestroy()
        {

        }
    }
}