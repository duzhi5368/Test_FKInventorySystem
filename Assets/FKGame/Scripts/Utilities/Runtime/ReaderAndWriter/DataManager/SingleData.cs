using System;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SingleData : Dictionary<string, string>
    {
        public DataTable data;          // 实际数据存储位
        public string keyComment;       // 该记录的key键名

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

                if (data.defaultValueDict.ContainsKey(key))
                {
                    content = data.defaultValueDict[key];
                    return ParseTool.GetInt(content);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetInt Error TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + keyComment + "<- content: ->" + content + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + keyComment + "<-");// throw  
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
                if (data.defaultValueDict.ContainsKey(key))
                {
                    content = data.defaultValueDict[key];
                    return ParseTool.String2IntArray(content);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetIntArray Error TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + keyComment + "<- content: ->" + content + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + keyComment + "<-");// throw  
        }

        public float GetFloat(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.GetFloat(this[key]);
                }

                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.GetFloat(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetFloat Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public float[] GetFloatArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2FloatArray(this[key]);
                }

                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2FloatArray(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetFloatArray Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
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

                if (data.defaultValueDict.ContainsKey(key))
                {
                    content = data.defaultValueDict[key];
                    return ParseTool.GetBool(content);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetBool Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- content: ->" + content + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public bool[] GetBoolArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2BoolArray(this[key]);
                }

                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2BoolArray(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetBoolArray Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public string GetString(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.GetString(this[key]);
                }

                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.GetString(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetString Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-");// throw  
        }

        public Vector2 GetVector2(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector2(this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2Vector2(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public Vector2[] GetVector2Array(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector2Array(this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2Vector2Array(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public Vector3[] GetVector3Array(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector3Array(this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2Vector3Array(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector3Array Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public Vector3 GetVector3(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Vector3(this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2Vector3(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetVector3 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public Color GetColor(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2Color(this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2Color(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetColor Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public T GetEnum<T>(string key) where T : struct
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return (T)Enum.Parse(typeof(T), this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return (T)Enum.Parse(typeof(T), data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetEnum Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-"); // throw  
        }

        public string[] GetStringArray(string key)
        {
            try
            {
                if (this.ContainsKey(key))
                {
                    return ParseTool.String2StringArray(this[key]);
                }
                if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2StringArray(data.defaultValueDict[key]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetStringArray Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-");// throw  
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
                else if (data.defaultValueDict.ContainsKey(key))
                {
                    return ParseTool.String2Array(data.GetFieldType(key), data.defaultValueDict[key], data.GetArraySplitFormat(key));
                }
            }
            catch (Exception e)
            {
                throw new Exception("SingleData GetStringArray2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + keyComment + "<- \n" + e.ToString());
            }
            throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + keyComment + "<-");
        }
    }
}