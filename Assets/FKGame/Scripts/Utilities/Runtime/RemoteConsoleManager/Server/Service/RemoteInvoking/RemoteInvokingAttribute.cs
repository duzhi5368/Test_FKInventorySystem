using System;
//------------------------------------------------------------------------
namespace FKGame 
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class RemoteInvokingAttribute : Attribute
    {
        public const string MethodType_Custom = "Custom";
        public const string MethodType_System = "System";
        private string m_methodType;
        public string name { get; set; }
        public string description { get; set; }

        // 调用方法分类
        public string methodType
        {
            get
            {
                if (string.IsNullOrEmpty(m_methodType))
                    return MethodType_Custom;
                else
                    return m_methodType;
            }
            set
            {
                m_methodType = value;
            }
        }
    }
}