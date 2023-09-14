using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [System.Serializable]
    public struct UICameraData
    {
        public string m_key;
        public GameObject m_root;
        public Camera m_camera;
        public Transform m_GameUILayerParent;
        public Transform m_FixedLayerParent;
        public Transform m_NormalLayerParent;
        public Transform m_TopbarLayerParent;
        public Transform m_UpperParent;
        public Transform m_PopUpLayerParent;
    }
}