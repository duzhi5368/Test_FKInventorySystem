using System.Collections;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class TimedEnable : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("定时触发时间，单位：秒")]
        private float m_Delay = 1f;
        [SerializeField]
        [Tooltip("到时后，需要开启/关闭的组件对象")]
        private Behaviour m_Combonent=null;
        [SerializeField]
        [Tooltip("到时后，如需要关闭组件对象，则这里不要点选；如到时后激活，这里需点选")]
        private bool m_Enable = true;

        private void OnEnable()
        {
            StartCoroutine(WaitAndSetEnabled());
        }

        private IEnumerator WaitAndSetEnabled() {
            yield return new WaitForSeconds(this.m_Delay);
            if (this.m_Combonent != null)
            {
                this.m_Combonent.enabled = this.m_Enable;
            }
            // 自身无效化
            enabled = false;
        }
    }
}