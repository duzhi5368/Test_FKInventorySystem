using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // 在Editor GUI里面显示特别的名字
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ShowGUINameAttribute : Attribute
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        public ShowGUINameAttribute(string newName)
        {
            name = newName;
        }
    }
}