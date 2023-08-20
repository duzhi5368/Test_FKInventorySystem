using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FKGame.UIWidgets
{
    public class WidgetTrigger : MonoBehaviour
    {
        public void Show(string name) {
           UIWidget widget = WidgetUtility.Find<UIWidget>(name);
            if (widget != null)
                widget.Show();
        }

        public void Close(string name)
        {
            UIWidget widget = WidgetUtility.Find<UIWidget>(name);
            if (widget != null)
                widget.Close();
        }
    }
}