using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    // 字段描述信息
    public class TableConfigFieldInfo
    {
        [ShowGUIName("字段名")]
        public string fieldName = "";
        [ShowGUIName("描述")]
        public string description = "";
        [ShowGUIName("数据类型")]
        public FieldType fieldValueType;
        [ShowGUIName("数据用途")]
        public DataFieldAssetType fieldAssetType;
        [ShowGUIName("默认值")]
        public object defultValue = null;
        public string enumType = "";

        // 数组分割符号
        [NoShowInEditor]
        public List<char> m_ArraySplitFormat = new List<char>();
    }
}