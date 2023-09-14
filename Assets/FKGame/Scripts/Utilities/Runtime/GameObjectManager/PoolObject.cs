using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ������ع���Ķ���
    public class PoolObject : MonoBehaviour
    {
        public bool SetActive = true;

        /// <summary>
        /// �����ʼ��ʱ����
        /// </summary>
        public virtual void OnCreate()
        {

        }

        /// <summary>
        /// �Ӷ������ȡ��ʱ����
        /// </summary>
        public virtual void OnFetch()
        {

        }

        /// <summary>
        /// ������ʱ����
        /// </summary>
        public virtual void OnRecycle()
        {

        }

        /// <summary>
        /// ������ʱ����
        /// </summary>
        public virtual void OnObjectDestroy()
        {

        }
    }
}