using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ReusingScrollItemBase : UIBase
    {
        public int m_index = 0;

        public virtual void OnShow(){}
        public virtual void OnHide(){}
        public virtual void SetContent(int index, Dictionary<string, object> data){}
        public virtual void OnDrag(){}
    }
}