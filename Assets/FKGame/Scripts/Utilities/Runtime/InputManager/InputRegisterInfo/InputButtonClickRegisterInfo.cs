using UnityEngine.Events;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class InputButtonClickRegisterInfo : InputEventRegisterInfo<InputUIOnClickEvent>
    {
        public Button m_button;
        public UnityAction m_OnClick;

        public override void RemoveListener()
        {
            base.RemoveListener();
            m_button.onClick.RemoveListener(m_OnClick);
        }

        public override void AddListener()
        {
            base.AddListener();
            m_button.onClick.AddListener(m_OnClick);
        }
    }
}