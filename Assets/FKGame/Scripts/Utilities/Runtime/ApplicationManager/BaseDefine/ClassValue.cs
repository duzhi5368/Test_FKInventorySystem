using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // �洢ʵ������
    [System.Serializable]
    public class ClassValue
    {

        public string ScriptName = "";
        public List<BaseValue> fieldValues = new List<BaseValue>();
        public List<BaseValue> propertyValues = new List<BaseValue>();
        private bool isDeclaredOnly = true;
        public ClassValue() { }
        // �洢ʵ�����ݡ����� isDeclaredOnly �Ƿ�ֻ�е�ǰ��ı������ԣ�True������������ģ�
        public ClassValue(object value, bool isDeclaredOnly = true)
        {
            this.isDeclaredOnly = isDeclaredOnly;
            SetValue(value);
        }

        public void SetValue(object value)
        {
            if (value == null)
                return;
            fieldValues.Clear();
            Type type = value.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            FieldInfo[] fields = type.GetFields(flags);
            ScriptName = type.FullName;
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];
                object v = f.GetValue(value);
                if (v == null)
                    continue;
                BaseValue scriptValue = new BaseValue(f.Name, v);
                fieldValues.Add(scriptValue);
            }
            BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance;
            if (isDeclaredOnly)
            {
                bindingAttr = bindingAttr | BindingFlags.DeclaredOnly;
            }
            PropertyInfo[] propertyInfos = type.GetProperties(bindingAttr);
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo property = propertyInfos[i];
                if (property.CanRead && property.CanWrite)
                {
                    try
                    {
                        BaseValue scriptValue = new BaseValue(property.Name, property.GetValue(value, null));
                        propertyValues.Add(scriptValue);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        // ��ȡ�����ʵ��(��ֵ)
        public object GetValue(object classObj = null)
        {
            if (string.IsNullOrEmpty(ScriptName))
                return null;
            Type type = null;
            if (classObj == null)
            {
                type = ReflectionUtils.GetTypeByTypeFullName(ScriptName);
                classObj = ReflectionUtils.CreateDefultInstance(type);
            }
            else
            {
                type = classObj.GetType();
                if (type.FullName != ScriptName)
                {
                    Debug.LogError("���Ͳ�ͬ ->ScriptName��" + ScriptName + "  classObj:" + classObj.GetType());
                }
            }

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            for (int i = 0; i < fieldValues.Count; i++)
            {
                BaseValue fInfo = fieldValues[i];
                FieldInfo f = type.GetField(fInfo.name, flags);
                if (f != null && f.Name == fInfo.name)
                {
                    try
                    {
                        f.SetValue(classObj, fInfo.GetValue());
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pinfo in propertyValues)
            {
                if (string.IsNullOrEmpty(pinfo.name))
                    continue;
                PropertyInfo property = type.GetProperty(pinfo.name, BindingFlags.Public | BindingFlags.Instance);
                if (property.CanWrite)
                {
                    try
                    {
                        property.SetValue(classObj, pinfo.GetValue(), null);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return classObj;
        }
    }
}