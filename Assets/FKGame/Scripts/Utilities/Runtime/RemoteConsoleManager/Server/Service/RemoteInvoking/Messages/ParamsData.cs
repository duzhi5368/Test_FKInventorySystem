using System;
//------------------------------------------------------------------------
namespace FKGame
{
    [Serializable]
    public class ParamsData : INetSerializable
    {
        public string descriptionName;
        public string paraName;
        public string paraTypeFullName;
        public string defaultValueStr;          // Ĭ��ֵ��json�ַ���ֵ
        public string[] selectItemValues;       // ��ǰ������ֵ�ɹ�ѡ�����(���ܶ�String���Ͳ���ʹ��)

        public void Deserialize(NetDataReader reader)
        {
            descriptionName = reader.GetString();
            paraName = reader.GetString();
            paraTypeFullName = reader.GetString();
            defaultValueStr = reader.GetString();
            selectItemValues = reader.GetStringArray();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(descriptionName);
            writer.Put(paraName);
            writer.Put(paraTypeFullName);
            writer.Put(defaultValueStr);
            writer.Put(selectItemValues);
        }

        public Type GetParamValueType()
        {
            return ReflectionTool.GetTypeByTypeFullName(paraTypeFullName);
        }

        public object GetDefaultValue()
        {
            Type type = GetParamValueType();
            if (type == null)
                return null;
            if (string.IsNullOrEmpty(defaultValueStr))
            {
                return ReflectionTool.CreateDefultInstance(type);
            }
            return JsonSerializer.FromJson(type, defaultValueStr);
        }
    }
}