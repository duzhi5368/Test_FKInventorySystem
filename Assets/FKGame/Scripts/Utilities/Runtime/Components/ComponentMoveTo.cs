using UnityEngine;
//------------------------------------------------------------------------
// 简单的Player移动（不推荐）
//------------------------------------------------------------------------
namespace FKGame
{
    public class ComponentMoveTo : MonoBehaviour
    {
        [SerializeField]
        private string m_Tag = "Player";
        [SerializeField]
        private float speed = 3f;
        private Transform player;
        [SerializeField]
        private Vector3 targetPosition = Vector3.right;

        void Start()
        {
            GameObject go = GameObject.FindGameObjectWithTag(this.m_Tag);
            if (go != null)
            {
                player = go.transform;
            }
        }

        void Update()
        {
            if (player == null)
                return;

            if (Vector3.Distance(transform.position, this.targetPosition) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, this.targetPosition, speed * Time.deltaTime);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}