using System.Collections.Generic;
using System.Text;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DataTable : Dictionary<string, SingleData>
    {
        const char c_split = '\t';
        const string c_newline = "\r\n";
        const string c_defaultValueTableTitle = "default";
        const string c_noteTableTitle = "note";
        const string c_fieldTypeTableTitle = "type";
        const char c_EnumSplit = '|';
        const char c_DataFieldAssetTypeSplit = '&';

        public string m_tableName;

        // Ĭ��ֵ
        public Dictionary<string, string> m_defaultValue = new Dictionary<string, string>();
        // ע��
        public Dictionary<string, string> m_noteValue = new Dictionary<string, string>();
        // ����ÿ���ֶ���ʲô����
        public Dictionary<string, FieldType> m_tableTypes = new Dictionary<string, FieldType>();
        // ����ָ���ţ��ֶ���,�ָ��ַ���
        public Dictionary<string, char[]> m_ArraySplitFormat = new Dictionary<string, char[]>();
        // �����ö�����ͣ����ﴢ���������
        public Dictionary<string, string> m_tableEnumTypes = new Dictionary<string, string>();
        // ������¼��ӵ�е��ֶ���
        public List<string> TableKeys = new List<string>();
        // �������е�Key
        public List<string> TableIDs = new List<string>();
        // �ֶε���;����
        public Dictionary<string, DataFieldAssetType> m_fieldAssetTypes = new Dictionary<string, DataFieldAssetType>();


        // ���ı�����Ϊ������
        public static DataTable Analysis(string stringData)
        {
            string debugContent = "";
            int debugLineCount = 0;
            int debugRowCount = 0;
            string debugKey = "";
            string debugProperty = "";

            try
            {
                int lineIndex = 0;
                DataTable data = new DataTable();
                string[] line = stringData.Split(c_newline.ToCharArray());

                //��һ����ΪKey
                debugContent = "����Key";
                data.TableKeys = new List<string>();
                string[] rowKeys = ConvertStringArray(line[0]);
                for (int i = 0; i < rowKeys.Length; i++)
                {
                    debugRowCount = i;
                    if (!rowKeys[i].Equals(""))
                    {
                        data.TableKeys.Add(rowKeys[i]);
                    }
                }

                string[] LineData;
                for (lineIndex = 1; lineIndex < line.Length; lineIndex++)
                {
                    if (line[lineIndex] != "" && line[lineIndex] != null)
                    {
                        debugLineCount = lineIndex;
                        LineData = ConvertStringArray(line[lineIndex]);

                        //ע��
                        if (LineData[0].Equals(c_noteTableTitle))
                        {
                            debugContent = "����ע��";
                            AnalysisNoteValue(data, LineData);
                        }
                        //Ĭ��ֵ
                        else if (LineData[0].Equals(c_defaultValueTableTitle))
                        {
                            debugContent = "����Ĭ��ֵ";
                            AnalysisDefaultValue(data, LineData);
                        }
                        //��������
                        else if (LineData[0].Equals(c_fieldTypeTableTitle))
                        {
                            debugContent = "��������";
                            AnalysisFieldType(data, LineData);
                        }
                        //��������
                        else
                        {
                            debugContent = "��������";
                            break;
                        }
                    }
                }

                data.TableIDs = new List<string>();
                //��ʼ��������
                for (int i = lineIndex; i < line.Length; i++)
                {
                    debugLineCount = i;
                    SingleData dataTmp = new SingleData();
                    dataTmp.data = data;

                    if (line[i] != "" && line[i] != null)
                    {
                        string[] row = ConvertStringArray(line[i]);
                        for (int j = 0; j < data.TableKeys.Count; j++)
                        {
                            debugRowCount = j;
                            debugKey = row[0];
                            if (!row[j].Equals(""))
                            {
                                debugProperty = data.TableKeys[j];
                                dataTmp.Add(data.TableKeys[j], row[j]);
                            }
                        }
                        //��һ��������Ϊ��һ����¼��Key
                        data.AddData(dataTmp);
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("DataTable Analysis Error: ����λ�ã�" + debugContent + " ��:" + debugLineCount / 2 + " �У�" + debugRowCount + " key:->" + debugKey + "<- PropertyName��->" + debugProperty + "<-\n" + e.ToString()); // throw  
            }
        }

        // ����ע��
        public static void AnalysisNoteValue(DataTable l_data, string[] l_lineData)
        {
            l_data.m_noteValue = new Dictionary<string, string>();

            for (int i = 0; i < l_lineData.Length && i < l_data.TableKeys.Count; i++)
            {
                if (!l_lineData[i].Equals(""))
                {
                    l_data.m_noteValue.Add(l_data.TableKeys[i], l_lineData[i]);
                }
            }
        }

        public static void AnalysisDefaultValue(DataTable l_data, string[] l_lineData)
        {
            l_data.m_defaultValue = new Dictionary<string, string>();

            for (int i = 0; i < l_lineData.Length && i < l_data.TableKeys.Count; i++)
            {
                if (!l_lineData[i].Equals(""))
                {
                    l_data.m_defaultValue.Add(l_data.TableKeys[i], l_lineData[i]);
                }
            }
        }

        public static void AnalysisFieldType(DataTable l_data, string[] l_lineData)
        {
            l_data.m_tableTypes = new Dictionary<string, FieldType>();

            for (int i = 1; i < l_lineData.Length && i < l_data.TableKeys.Count; i++)
            {
                if (!l_lineData[i].Equals(""))
                {
                    string field = l_data.TableKeys[i];
                    string[] tempType = l_lineData[i].Split(c_DataFieldAssetTypeSplit);
                    string[] content = tempType[0].Split(c_EnumSplit);
                    try
                    {
                        string fieldType = content[0];
                        if (fieldType.Contains("["))
                        {
                            string[] tempSS = fieldType.Split('[');
                            fieldType = tempSS[0];
                            string splitStr = tempSS[1].Replace("]", "");

                            l_data.m_ArraySplitFormat.Add(field, splitStr.ToCharArray());
                        }
                        l_data.m_tableTypes.Add(field, (FieldType)Enum.Parse(typeof(FieldType), fieldType));

                        if (content.Length > 1)
                        {
                            l_data.m_tableEnumTypes.Add(field, content[1]);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("AnalysisFieldType Exception: " + content + "\n" + e.ToString());
                    }

                    if (tempType.Length > 1)
                    {
                        l_data.m_fieldAssetTypes.Add(field, (DataFieldAssetType)Enum.Parse(typeof(DataFieldAssetType), tempType[1]));
                    }
                    else
                    {
                        l_data.m_fieldAssetTypes.Add(field, DataFieldAssetType.Data);
                    }
                }
            }
        }

        public static string Serialize(DataTable data)
        {
            StringBuilder build = new StringBuilder();
            for (int i = 0; i < data.TableKeys.Count; i++)
            {
                build.Append(data.TableKeys[i]);
                if (i != data.TableKeys.Count - 1)
                {
                    build.Append(c_split);
                }
                else
                {
                    build.Append(c_newline);
                }
            }

            List<string> type = new List<string>(data.m_tableTypes.Keys);
            build.Append(c_fieldTypeTableTitle);

            if (type.Count > 0)
            {
                build.Append(c_split);
                for (int i = 1; i < data.TableKeys.Count; i++)
                {
                    string key = data.TableKeys[i];
                    string typeString = "";
                    if (data.m_tableTypes.ContainsKey(key))
                    {
                        typeString = data.m_tableTypes[key].ToString();
                        if (data.m_ArraySplitFormat.ContainsKey(key))
                        {
                            typeString += "[";
                            foreach (var item in data.m_ArraySplitFormat[key])
                            {
                                typeString += item;
                            }
                            typeString += "]";
                        }

                        if (data.m_tableEnumTypes.ContainsKey(key))
                        {
                            typeString += c_EnumSplit + data.m_tableEnumTypes[key];
                        }
                    }
                    else
                    {
                        typeString = FieldType.String.ToString();
                    }

                    if (data.m_fieldAssetTypes.ContainsKey(key))
                    {
                        if (data.m_fieldAssetTypes[key] != DataFieldAssetType.Data)
                            typeString += "&" + data.m_fieldAssetTypes[key];
                    }

                    build.Append(typeString);
                    if (i != data.TableKeys.Count - 1)
                    {
                        build.Append(c_split);
                    }
                    else
                    {
                        build.Append(c_newline);
                    }
                }
            }
            else
            {
                build.Append(c_newline);
            }

            List<string> noteValue = new List<string>(data.m_noteValue.Keys);
            build.Append(c_noteTableTitle);
            if (noteValue.Count > 0)
            {
                build.Append(c_split);
                for (int i = 1; i < data.TableKeys.Count; i++)
                {
                    string key = data.TableKeys[i];
                    string defauleNoteTmp = "";

                    if (data.m_noteValue.ContainsKey(key))
                    {
                        defauleNoteTmp = data.m_noteValue[key];
                    }
                    else
                    {
                        defauleNoteTmp = "";
                    }

                    build.Append(defauleNoteTmp);

                    if (i != data.TableKeys.Count - 1)
                    {
                        build.Append(c_split);
                    }
                    else
                    {
                        build.Append(c_newline);
                    }
                }
            }
            else
            {
                build.Append(c_newline);
            }

            List<string> defaultValue = new List<string>(data.m_defaultValue.Keys);
            build.Append(c_defaultValueTableTitle);
            if (defaultValue.Count > 0)
            {
                build.Append(c_split);
                for (int i = 1; i < data.TableKeys.Count; i++)
                {
                    string key = data.TableKeys[i];
                    string defauleValueTmp = "";

                    if (data.m_defaultValue.ContainsKey(key))
                    {
                        defauleValueTmp = data.m_defaultValue[key];
                    }
                    else
                    {
                        defauleValueTmp = "";
                    }

                    build.Append(defauleValueTmp);

                    if (i != data.TableKeys.Count - 1)
                    {
                        build.Append(c_split);
                    }
                    else
                    {
                        build.Append(c_newline);
                    }
                }
            }
            else
            {
                build.Append(c_newline);
            }

            foreach (string k in data.TableIDs)
            {
                SingleData dataTmp = data[k];
                for (int i = 0; i < data.TableKeys.Count; i++)
                {
                    string valueTmp = "";
                    string field = data.TableKeys[i];
                    string defaultV = "";
                    if (data.m_defaultValue.ContainsKey(field))
                        defaultV = data.m_defaultValue[field];
                    if (dataTmp.ContainsKey(field) && dataTmp[field] != defaultV)
                    {
                        valueTmp = dataTmp[field];
                    }

                    build.Append(valueTmp);
                    if (i != data.TableKeys.Count - 1)
                    {
                        build.Append(c_split);
                    }
                    else
                    {
                        build.Append(c_newline);
                    }
                }
            }
            return build.ToString();
        }

        public static string[] ConvertStringArray(string lineContent)
        {
            List<string> result = new List<string>();
            int startIndex = 0;
            bool state = true; // ����״̬������״̬

            for (int i = 0; i < lineContent.Length; i++)
            {
                if (state)
                {
                    if (lineContent[i] == c_split)
                    {
                        result.Add(lineContent.Substring(startIndex, i - startIndex));
                        startIndex = i + 1;
                    }
                    else if (lineContent[i] == '\"')
                    {
                        state = false; //תΪ����״̬
                    }
                }
                else
                {
                    if (lineContent[i] == '\"')
                    {
                        state = true;// תΪ����״̬
                    }
                }
            }
            result.Add(lineContent.Substring(startIndex, lineContent.Length - startIndex));
            return result.ToArray();
        }

        public FieldType GetFieldType(string key)
        {
            // ����ֻ����String����
            if (key == TableKeys[0])
            {
                return FieldType.String;
            }
            if (m_tableTypes.ContainsKey(key))
            {
                return m_tableTypes[key];
            }
            else
            {
                return FieldType.String;
            }
        }
        public char[] GetArraySplitFormat(string key)
        {
            if (m_ArraySplitFormat.ContainsKey(key))
            {
                return m_ArraySplitFormat[key];
            }
            return new char[0];
        }

        public void SetFieldType(string key, FieldType type, string enumType)
        {
            // ����ֻ����String����
            if (key == TableKeys[0])
            {
                return;
            }
            if (m_tableTypes.ContainsKey(key))
            {
                m_tableTypes[key] = type;
            }
            else
            {
                m_tableTypes.Add(key, type);
            }
            // �洢��������
            if (enumType != null)
            {
                if (m_tableEnumTypes.ContainsKey(key))
                {
                    m_tableEnumTypes[key] = enumType;
                }
                else
                {
                    m_tableEnumTypes.Add(key, enumType);
                }
            }
        }

        public void SetAssetTypes(string key, DataFieldAssetType type)
        {
            // ����ֻ����String����
            if (key == TableKeys[0])
            {
                return;
            }

            if (m_fieldAssetTypes.ContainsKey(key))
            {
                m_fieldAssetTypes[key] = type;
            }
            else
            {
                m_fieldAssetTypes.Add(key, type);
            }
        }

        public SingleData GetLineFromKey(string key)
        {
            // ����ֻ����String����
            SingleData _value = null;
            if (ContainsKey(key))
                _value = this[key];
            return _value;
        }

        public string GetEnumType(string key)
        {
            if (m_tableEnumTypes.ContainsKey(key))
            {
                return m_tableEnumTypes[key];
            }
            else
            {
                return null;
            }
        }

        public string GetDefault(string key)
        {
            if (m_defaultValue.ContainsKey(key))
            {
                return m_defaultValue[key];
            }
            else
            {
                return null;
            }
        }

        public void SetDefault(string key, string value)
        {
            if (!m_defaultValue.ContainsKey(key))
            {
                m_defaultValue.Add(key, value);
            }
            else
            {
                m_defaultValue[key] = value;
            }
        }

        public void SetNote(string key, string note)
        {
            if (!m_noteValue.ContainsKey(key))
            {
                m_noteValue.Add(key, note);
            }
            else
            {
                m_noteValue[key] = note;
            }
        }

        public string GetNote(string key)
        {
            if (!m_noteValue.ContainsKey(key))
            {
                return null;
            }
            else
            {
                return m_noteValue[key];
            }
        }

        public void AddData(SingleData data)
        {
            if (data.ContainsKey(TableKeys[0]))
            {
                data.m_SingleDataKey = data[TableKeys[0]];
                Add(data[TableKeys[0]], data);
                TableIDs.Add(data[TableKeys[0]]);
            }
            else
            {
                throw new Exception("Add SingleData fail! The dataTable dont have MainKey!");
            }
        }

        public void SetData(SingleData data)
        {
            // ����
            string mainKey = TableKeys[0];

            if (data.ContainsKey(mainKey))
            {
                string key = data[mainKey];
                if (ContainsKey(key))
                {
                    this[key] = data;
                }
                else
                {
                    Add(key, data);
                    TableIDs.Add(key);
                }
            }
            else
            {
                throw new Exception("Add SingleData fail! The dataTable dont have MainKeyKey!");
            }
        }

        public void RemoveData(string key)
        {
            if (ContainsKey(key))
            {
                Remove(key);
                TableIDs.Remove(key);
            }
            else
            {
                throw new Exception("Add SingleData fail!");
            }
        }
    }
}