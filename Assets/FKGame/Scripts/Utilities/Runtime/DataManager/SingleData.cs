using System;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SingleData : Dictionary<string, string>
    {
        public DataTable data;
        public string m_SingleDataKey;      // ¸Ã¼ÇÂ¼µÄkey
        public int GetInt(string key)
        {
            string content = null;
            try
            {
                if (this.ContainsKey(key))
                {
                    content = this[key];
                    return ParseTool.GetInt(content);
                }

                if (data.m_defaultValue.ContainsKey(key))
                {
                    content = data.m_defaultValue[key];
                    return ParseTool.GetInt(content);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetInt Error TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- content: ->" + content + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<-");// throw  
        }

        public int[] GetIntArray(string key)
        {
            string content = null;
            try
            {
                if (this.ContainsKey(key))
                {
                    content = this[key];
                    return ParseTool.String2IntArray(content);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    content = data.m_defaultValue[key];
                    return ParseTool.String2IntArray(content);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetIntArray Error TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- content: ->" + content + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<-");// throw  
        }

        public float GetFloat(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.GetFloat(this[key]);
                }

                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.GetFloat(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetFloat Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public float[] GetFloatArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2FloatArray(this[key]);
                }

                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2FloatArray(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetFloatArray Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public bool GetBool(string key)
        {
            string content = null;
            try
            {
                if (this.ContainsKey(key))
                {
                    content = this[key];
                    return ParseTool.GetBool(content);
                }

                if (data.m_defaultValue.ContainsKey(key))
                {
                    content = data.m_defaultValue[key];
                    return ParseTool.GetBool(content);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetBool Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- content: ->" + content + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public bool[] GetBoolArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2BoolArray(this[key]);
                }

                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2BoolArray(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetBoolArray Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public string GetString(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.GetString(this[key]);
                }

                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.GetString(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetString Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");// throw  
        }

        string StringFilter(string content)
        {
            if (content == "Null"
                || content == "null"
                || content == "NULL"
                || content == "nu11"
                || content == "none"
                || content == "nil"
                || content == "")
            {
                return null;
            }
            else
            {
                return content;
            }
        }

        public Vector2 GetVector2(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector2(this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2Vector2(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public Vector2[] GetVector2Array(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector2Array(this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2Vector2Array(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }


        public Vector3[] GetVector3Array(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector3Array(this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2Vector3Array(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector3Array Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public Vector3 GetVector3(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector3(this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2Vector3(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector3 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public Color GetColor(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Color(this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2Color(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetColor Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public T GetEnum<T>(string key) where T : struct
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return (T)Enum.Parse(typeof(T), this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return (T)Enum.Parse(typeof(T), data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetEnum Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-"); // throw  
        }

        public string[] GetStringArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2StringArray(this[key]);
                }
                if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2StringArray(data.m_defaultValue[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetStringArray Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");// throw  
        }

        public T[] GetArray<T>(string key)
        {
            return (T[])GetArray(key);
        }

        public Array GetArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Array(data.GetFieldType(key), this[key], data.GetArraySplitFormat(key));
                }
                else if (data.m_defaultValue.ContainsKey(key))
                {
                    return ParseTool.String2Array(data.GetFieldType(key), data.m_defaultValue[key], data.GetArraySplitFormat(key));
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetStringArray2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
        }
    }
}