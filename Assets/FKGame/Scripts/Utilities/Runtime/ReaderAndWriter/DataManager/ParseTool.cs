using System;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class ParseTool
    {
        static string[] nullStringArray = new string[0];

        public static float[] String2FloatArray(string value)
        {
            string[] strArray = String2StringArray(value);
            float[] array = new float[strArray.Length];

            for (int i = 0; i < strArray.Length; i++)
            {
                float tmp = float.Parse(strArray[i]);

                array[i] = tmp;
            }
            return array;
        }

        public static bool[] String2BoolArray(string value)
        {
            string[] strArray = String2StringArray(value);
            bool[] array = new bool[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                bool tmp = bool.Parse(strArray[i]);
                array[i] = tmp;
            }
            return array;
        }


        public static Vector2 String2Vector2(string value)
        {
            try
            {
                string[] values = value.Split(',');
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                return new Vector2(x, y);
            }
            catch (Exception e)
            {
                throw new Exception("��FK��ParseVector2: Don't convert value to Vector2 value:" + value + "\n" + e.ToString()); // throw  
            }
        }

        public static Vector2[] String2Vector2Array(string value)
        {
            string[] strArray = String2StringArray(value);
            Vector2[] array = new Vector2[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] values = strArray[i].Split(',');
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                array[i] = new Vector2(x, y);
            }
            return array;
        }

        public static Vector3 String2Vector3(string value)
        {
            try
            {
                string[] values = value.Split(',');
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                float z = float.Parse(values[2]);
                return new Vector3(x, y, z);
            }
            catch (Exception e)
            {
                throw new Exception("��FK��ParseVector3: Can't convert value to Vector3 value:" + value + "\n" + e.ToString()); // throw  
            }
        }

        public static Vector3[] String2Vector3Array(string value)
        {
            string[] strArray = String2StringArray(value);
            Vector3[] array = new Vector3[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] values = strArray[i].Split(',');
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                float z = float.Parse(values[2]);
                array[i] = new Vector3(x, y, z);
            }
            return array;
        }

        public static Color String2Color(string value)
        {
            try
            {
                string[] values = value.Split(',');
                float r = float.Parse(values[0]);
                float g = float.Parse(values[1]);
                float b = float.Parse(values[2]);
                float a = 1;
                if (values.Length > 3)
                {
                    a = float.Parse(values[3]);
                }
                return new Color(r, g, b, a);
            }
            catch (Exception e)
            {
                throw new Exception("��FK��ParseColor: Can't convert value to Color value:" + value + "\n" + e.ToString()); // throw  
            }
        }
        
        public static string[] String2StringArray(string value)
        {
            if (value != null
                    && value != ""
                    && value != "null"
                    && value != "Null"
                    && value != "NULL"
                    && value != "None")
            {
                return value.Split('|');
            }
            else
            {
                return nullStringArray;
            }
        }

        public static int[] String2IntArray(string value)
        {
            int[] intArray = null;
            if (!string.IsNullOrEmpty(value))
            {
                string[] strs = value.Split('|');
                intArray = Array.ConvertAll(strs, s => int.Parse(s));
                return intArray;
            }
            else
            {
                return new int[0];
            }
        }

        // ����ת�����ַ�������
        public static string ArrayObject2String(object value, char[] arraySplitFormat)
        {
            if (value == null)
                return "null";
            List<char> charList = new List<char>();
            charList.Add('|');
            charList.AddRange(arraySplitFormat);
            return Array2String(value, charList.ToArray(), 0);
        }

        private static string Array2String(object value, char[] arraySplitFormat, int charTagIndex)
        {
            Array array = (Array)value;
            string result = "";
            char useTag = ' ';
            try
            {
                useTag = arraySplitFormat[charTagIndex];
            }
            catch (Exception e)
            {
                Debug.LogError("��FK��charTagIndex:" + charTagIndex + " arraySplitFormat:" + arraySplitFormat.Length + " value:" + value.GetType() + "\n" + e);
                return result;
            }

            charTagIndex++;
            for (int i = 0; i < array.Length; i++)
            {
                object childValue = array.GetValue(i);
                if (childValue == null)
                    continue;
                if (childValue.GetType().IsArray)
                {
                    result += Array2String(childValue, arraySplitFormat, charTagIndex);
                }
                else
                {
                    result += SingleData2String(childValue);
                }
                if (i < array.Length - 1)
                    result += useTag;
            }
            return result;
        }
        private static string SingleData2String(object value)
        {
            Type t = value.GetType();
            if (t == typeof(Vector2))
            {
                Vector2 v2 = (Vector2)value;
                return v2.x + "," + v2.y;
            }
            if (t == typeof(Vector3))
            {
                Vector3 v3 = (Vector3)value;
                return v3.x + "," + v3.y + "," + v3.z;
            }
            if (t == typeof(Color))
            {
                Color c = (Color)value;
                return c.r + "," + c.g + "," + c.b + "," + c.a;
            }
            else
                return value.ToString();
        }

        // Stringת��ά����
        public static Array String2Array(FieldType fieldValueType, string value, char[] arraySplitFormat)
        {

            if (string.IsNullOrEmpty(value))
                value = "";
            if (value.ToLower().Equals("null"))
                value = "";

            Type elementType = ConfigFieldValueType2ArrayElementType(fieldValueType);
            List<char> charList = new List<char>();
            charList.Add('|');
            charList.AddRange(arraySplitFormat);
            return (Array)CreateInterleavedArray(value, elementType, charList.Count, charList.ToArray(), 0);
        }

        /// <summary>
        /// ʹ�õݹ�ͷ��������ά����
        /// </summary>
        /// <param name="value">��Ҫ�������ַ�������</param>
        /// <param name="elementType">����Ļ�������</param>
        /// <param name="num">������������ά��</param>
        /// <param name="arraySplitFormat">ÿ��ά�ȷָ���</param>
        /// <param name="charTagIndex">��ǰʹ�õڼ����ָ���</param>
        /// <returns></returns>
        private static object CreateInterleavedArray(string value, Type elementType, int num, char[] arraySplitFormat, int charTagIndex)
        {
            string[] array0 = value.Split(arraySplitFormat[charTagIndex]);
            Type arrayType = CreateInterleavedType(elementType, num);
            charTagIndex++;
            int rank = arrayType.GetArrayRank();
            object[] par = new object[rank];
            int arrayLenth = string.IsNullOrEmpty(value) ? 0 : array0.Length;
            if (rank > 0)
            {
                par[0] = arrayLenth;
            }
            Array instance = (Array)Activator.CreateInstance(arrayType, par);
            for (int i = 0; i < arrayLenth; i++)
            {
                string tempStr = array0[i];
                object childValue = null;
                if (charTagIndex < arraySplitFormat.Length)
                {
                    childValue = CreateInterleavedArray(tempStr, elementType, arraySplitFormat.Length - charTagIndex, arraySplitFormat, charTagIndex);
                }
                else
                {
                    childValue = String2SingleObject(elementType, tempStr);

                }
                try
                {
                    instance.SetValue(childValue, i);
                }
                catch (Exception e)
                {
                    Debug.LogError("��FK��" + instance.GetType() + " instance.Length:" + instance.Length + " childValue.GetType():" + childValue.GetType() + "\n" + e);
                }
            }
            return instance;
        }

        public static object String2SingleObject(Type type, string value)
        {
            if (type == typeof(int))
            {
                return GetInt(value);
            }
            else if (type == typeof(float))
            {
                return GetFloat(value);
            }
            else if (type == typeof(bool))
            {
                return GetBool(value);
            }
            else if (type == typeof(string))
            {
                return GetString(value);
            }
            else if (type == typeof(Vector2))
            {
                return String2Vector2(value);
            }
            else if (type == typeof(Vector3))
            {
                return String2Vector3(value);
            }
            else
            {
                Debug.LogError("��FK��Unspport data type: " + type.FullName + " value:" + value);
            }
            return null;
        }

        public static int GetInt(string content)
        {
            return int.Parse(content);
        }

        public static float GetFloat(string content)
        {
            return float.Parse(content);
        }

        public static bool GetBool(string content)
        {
            return bool.Parse(content);
        }
        public static string GetString(string content)
        {
            if (content == null || content.ToLower().Equals("null"))
                return null;
            else
            {
                return content;
            }
        }

        // ���ɽ�������
        public static Type CreateInterleavedType(Type elementType, int num)
        {
            Type arrayType = elementType;
            for (int i = 0; i < num; i++)
            {
                arrayType = arrayType.MakeArrayType();
            }
            return arrayType;
        }

        public static Type ConfigFieldValueType2ArrayElementType(FieldType fieldValueType)
        {
            Type t = null;
            switch (fieldValueType)
            {
                case FieldType.StringArray:
                    t = typeof(string);
                    break;
                case FieldType.BoolArray:
                    t = typeof(bool);
                    break;
                case FieldType.IntArray:
                    t = typeof(int);
                    break;
                case FieldType.FloatArray:
                    t = typeof(float);
                    break;
                case FieldType.Vector2Array:
                    t = typeof(Vector2);
                    break;
                case FieldType.Vector3Array:
                    t = typeof(Vector3);
                    break;

                default:
                    break;
            }
            return t;
        }
    }
}