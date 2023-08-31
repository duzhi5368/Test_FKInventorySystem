using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ComponentSelectableObjectName : MonoBehaviour
    {
        [SerializeField]
        private Text m_ObjectName = null;

        private void Start()
        {
            if (this.m_ObjectName == null)
                this.m_ObjectName = GetComponent<Text>();
        }

        private void Update()
        {
            if (ComponentSelectableObject.current == null) 
                return;

            string current = ComponentSelectableObject.current.name;
            if (this.m_ObjectName != null && !current.Equals(this.m_ObjectName.text))
                this.m_ObjectName.text = current;
        }
    }
}