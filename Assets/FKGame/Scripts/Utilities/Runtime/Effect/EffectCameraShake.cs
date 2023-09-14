using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class EffectCameraShake : MonoBehaviour
    {
        public Transform camTransform;              // ����Ŀ���transform
        public float n_shake = 0f;                  // ����������ʱ��
        public float n_shakeAmount = 0.15f;         // ���Խ�󶶶�Խ����
        public float n_decreaseFactor = 1f;         // ���ٶ�

        private Vector3 m_v3_randomOffest = new Vector3(-0.1f, 0, 0.1f); // ����ƫ������
        private Vector3 m_v3_weight = Vector3.one;  // �𶯷���Ȩ��

        public void Init(GameObject l_go_camera)
        {
            camTransform = l_go_camera.transform;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="ʱ��"></param>
        /// <param name="���"></param>
        /// <param name="����"></param>
        /// <param name="ƫ��"></param>
        public void Shake(float l_n_shokeTime, float l_n_amount, float l_n_decreaseFactor, Vector3 l_v3_randomOffest, Vector3 l_v3_weight)
        {
            n_shake = l_n_shokeTime;
            n_shakeAmount = l_n_amount;
            n_decreaseFactor = l_n_decreaseFactor;
            m_v3_randomOffest = l_v3_randomOffest;
            m_v3_weight = l_v3_weight;
        }

        public void UpdateShake()
        {
            if (n_shake > 0)
            {
                Vector3 l_v3_randomValue = Random.insideUnitSphere * n_shakeAmount + m_v3_randomOffest;
                float weight = m_v3_weight.x + m_v3_weight.y + m_v3_weight.z;
                if (weight == 0)
                {
                    return;
                }
                l_v3_randomValue.x *= (m_v3_weight.x / weight);
                l_v3_randomValue.y *= (m_v3_weight.y / weight);
                l_v3_randomValue.z *= (m_v3_weight.z / weight);
                camTransform.localPosition = l_v3_randomValue;
                n_shake -= Time.deltaTime * n_decreaseFactor;
            }
            else
            {
                camTransform.localPosition = Vector3.zero;
                n_shake = 0;
            }
        }
    }
}