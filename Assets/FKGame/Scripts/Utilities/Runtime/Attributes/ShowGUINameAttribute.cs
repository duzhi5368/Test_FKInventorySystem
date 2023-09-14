using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��Editor GUI������ʾ�ر������
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