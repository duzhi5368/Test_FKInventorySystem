using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    // �ֶ�������Ϣ
    public class TableConfigFieldInfo
    {
        [ShowGUIName("�ֶ���")]
        public string fieldName = "";
        [ShowGUIName("����")]
        public string description = "";
        [ShowGUIName("��������")]
        public FieldType fieldValueType;
        [ShowGUIName("������;")]
        public DataFieldAssetType fieldAssetType;
        [ShowGUIName("Ĭ��ֵ")]
        public object defultValue = null;
        public string enumType = "";

        // ����ָ����
        [NoShowInEditor]
        public List<char> m_ArraySplitFormat = new List<char>();
    }
}