using UnityEngine.EventSystems;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ModelRotate : MonoBehaviour
    {
        public float rotateSpeed = 1;
        private Vector2 mousePos = Vector2.zero;

        private void Rotate(Vector2 delta)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, -delta.x * rotateSpeed, 0));
        }

        public void OnDrag(BaseEventData arg0)
        {
            if (mousePos == Vector2.zero)
            {
                mousePos = arg0.currentInputModule.input.mousePosition;
                return;
            }
            Vector2 delta = arg0.currentInputModule.input.mousePosition - mousePos;
            if (delta != Vector2.zero)
                mousePos = arg0.currentInputModule.input.mousePosition;
            Rotate(delta);
        }
    }
}