using FKGame.UIWidgets;
using UnityEngine;
//------------------------------------------------------------------------
public class TestUIWidgetScene : MonoBehaviour
{
    void Start()
    {
        FloatingTextManager.Add(this.gameObject, this.gameObject.name.Replace("(Clone)", ""), Color.green, Vector3.zero);
        FloatingTextManager.Remove(gameObject, 2.0f);
    }
    void Update()
    {
        
    }
}
