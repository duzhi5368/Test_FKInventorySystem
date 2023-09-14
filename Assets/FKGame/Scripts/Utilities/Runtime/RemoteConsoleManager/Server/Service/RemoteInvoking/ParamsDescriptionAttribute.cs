using System.Collections.Generic;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ParamsDescriptionAttribute : Attribute
    {
        public string paramName { get; set; }
        public string paramsDescriptionName { get; set; }           // �Զ�����ʾ����
        public string getDefaultValueMethodName { get; set; }       // �Զ������Ĭ��ֵ��ֻ֧�ֻ������Ͳ���(int��float,bool,�ȵ�)��
        public string[] selectItemValues { get; set; }

        public static ParamsDescriptionAttribute GetParamsDescription(IEnumerable<ParamsDescriptionAttribute> paramsDescription, string paramName)
        {
            if (paramsDescription != null)
            {
                foreach (var item in paramsDescription)
                {
                    if (item.paramName == paramName)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}