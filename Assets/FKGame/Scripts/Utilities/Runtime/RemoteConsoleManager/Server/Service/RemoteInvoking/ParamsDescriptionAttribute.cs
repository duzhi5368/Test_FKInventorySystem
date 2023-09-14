using System.Collections.Generic;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ParamsDescriptionAttribute : Attribute
    {
        public string paramName { get; set; }
        public string paramsDescriptionName { get; set; }           // 自定义显示名称
        public string getDefaultValueMethodName { get; set; }       // 自定义参数默认值（只支持基础类型参数(int，float,bool,等等)）
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