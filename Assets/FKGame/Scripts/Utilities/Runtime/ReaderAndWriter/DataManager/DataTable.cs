using System.Collections.Generic;
using System.Text;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DataTable : Dictionary<string, SingleData>
    {
        private const char      splitSign = '\t';
        private const string    newlineSign = "\r\n";
        private const string    defaultTableTitleValue = "default";
        private const string    defaultNoteTableTitle = "note";
        private const string    defautFieldTypeTableTitle = "type";
        private const char      enumSplitSign = '|';
        private const char      dataFieldAssetTypeSplitSign = '&';

        public string m_tableName;

        // Ĭ��ֵ
        public Dictionary<string, string> defaultValueDict                  = new Dictionary<string, string>();
        // ע��
        public Dictionary<string, string> commentValueDict                  = new Dictionary<string, string>();
        // ����ÿ���ֶ���ʲô����
        public Dictionary<string, FieldType> tableTypesDict                 = new Dictionary<string, FieldType>();
        // ����ָ���ţ��ֶ���,�ָ��ַ���
        public Dictionary<string, char[]> arraySplitFormatDict              = new Dictionary<string, char[]>();
        // �����ö�����ͣ����ﴢ���������
        public Dictionary<string, string> tableEnumTypeDict                 = new Dictionary<string, string>();
        // ������¼��ӵ�е��ֶ���
        public List<string> tableKeyDict                                    = new List<string>();
        // �������е�Key
        public List<string> tableIDDict                                     = new List<string>();
        // �ֶε���;����
        public Dictionary<string, DataFieldAssetType> fieldAssetTypeDict    = new Dictionary<string, DataFieldAssetType>();


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
                string[] line = stringData.Split(newlineSign.ToCharArray());

                // ��һ����ΪKey
                debugContent = "Parse key";
                data.tableKeyDict = new List<string>();
                string[] rowKeys = ConvertStringArray(line[0]);
                for (int i = 0; i < rowKeys.Length; i++)
                {
                    debugRowCount = i;
                    if (!rowKeys[i].Equals(""))
                    {
                        data.tableKeyDict.Add(rowKeys[i]);
                    }
                }

                string[] LineData;
                for (lineIndex = 1; lineIndex < line.Length; lineIndex++)
                {
                    if (line[lineIndex] != "" && line[lineIndex] != null)
                    {
                        debugLineCount = lineIndex;
                        LineData = ConvertStringArray(line[lineIndex]);

                        // ע��
                        if (LineData[0].Equals(defaultNoteTableTitle))
                        {
                            debugContent = "Parse comment";
                            AnalysisNoteValue(data, LineData);
                        }
                        // Ĭ��ֵ
                        else if (LineData[0].Equals(defaultTableTitleValue))
                        {
                            debugContent = "Parse default value";
                            AnalysisDefaultValue(data, LineData);
                        }
                        // ��������
                        else if (LineData[0].Equals(defautFieldTypeTableTitle))
                        {
                            debugContent = "Parse type";
                            AnalysisFieldType(data, LineData);
                        }
                        // ��������
                        else
                        {
                            debugContent = "Parse content";
                            break;
                        }
                    }
                }

                data.tableIDDict = new List<string>();
                // ��ʼ��������
                for (int i = lineIndex; i < line.Length; i++)
                {
                    debugLineCount = i;
                    SingleData dataTmp = new SingleData();
                    dataTmp.data = data;

                    if (line[i] != "" && line[i] != null)
                    {
                        string[] row = ConvertStringArray(line[i]);
                        for (int j = 0; j < data.tableKeyDict.Count; j++)
                        {
                            debugRowCount = j;
                            debugKey = row[0];
                            if (!row[j].Equals(""))
                            {
                                debugProperty = data.tableKeyDict[j];
                                dataTmp.Add(data.tableKeyDict[j], row[j]);
                            }
                        }
                        // ��һ��������Ϊ��һ����¼��Key
                        data.AddData(dataTmp);
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("��FK��DataTable Analysis Error: " + debugContent + " -> Col:" + debugLineCount / 2 
                    + " Row��" + debugRowCount + " Key:->" + debugKey + " <- PropertyName��-> " + debugProperty + " <-\n" + e.ToString()); // throw  
            }
        }

        // ����ע��
        public static void AnalysisNoteValue(DataTable l_data, string[] l_lineData)
        {
            l_data.commentValueDict = new Dictionary<string, string>();

            for (int i = 0; i < l_lineData.Length && i < l_data.tableKeyDict.Count; i++)
            {
                if (!l_lineData[i].Equals(""))
                {
                    l_data.commentValueDict.Add(l_data.tableKeyDict[i], l_lineData[i]);
                }
            }
        }

        public static void AnalysisDefaultValue(DataTable l_data, string[] l_lineData)
        {
            l_data.defaultValueDict = new Dictionary<string, string>();

            for (int i = 0; i < l_lineData.Length && i < l_data.tableKeyDict.Count; i++)
            {
                if (!l_lineData[i].Equals(""))
                {
                    l_data.defaultValueDict.Add(l_data.tableKeyDict[i], l_lineData[i]);
                }
            }
        }

        public static void AnalysisFieldType(DataTable l_data, string[] l_lineData)
        {
            l_data.tableTypesDict = new Dictionary<string, FieldType>();

            for (int i = 1; i < l_lineData.Length && i < l_data.tableKeyDict.Count; i++)
            {
                if (!l_lineData[i].Equals(""))
                {
                    string field = l_data.tableKeyDict[i];
                    string[] tempType = l_lineData[i].Split(dataFieldAssetTypeSplitSign);
                    string[] content = tempType[0].Split(enumSplitSign);
                    try
                    {
                        string fieldType = content[0];
                        if (fieldType.Contains("["))
                        {
                            string[] tempSS = fieldType.Split('[');
                            fieldType = tempSS[0];
                            string splitStr = tempSS[1].Replace("]", "");

                            l_data.arraySplitFormatDict.Add(field, splitStr.ToCharArray());
                        }
                        l_data.tableTypesDict.Add(field, (FieldType)Enum.Parse(typeof(FieldType), fieldType));

                        if (content.Length > 1)
                        {
                            l_data.tableEnumTypeDict.Add(field, content[1]);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("��FK��AnalysisFieldType Exception: " + content + "\n" + e.ToString());
                    }

                    if (tempType.Length > 1)
                    {
                        l_data.fieldAssetTypeDict.Add(field, (DataFieldAssetType)Enum.Parse(typeof(DataFieldAssetType), tempType[1]));
                    }
                    else
                    {
                        l_data.fieldAssetTypeDict.Add(field, DataFieldAssetType.Data);
                    }
                }
            }
        }

        public static string Serialize(DataTable data)
        {
            StringBuilder build = new StringBuilder();
            for (int i = 0; i < data.tableKeyDict.Count; i++)
            {
                build.Append(data.tableKeyDict[i]);
                if (i != data.tableKeyDict.Count - 1)
                {
                    build.Append(splitSign);
                }
                else
                {
                    build.Append(newlineSign);
                }
            }

            List<string> type = new List<string>(data.tableTypesDict.Keys);
            build.Append(defautFieldTypeTableTitle);

            if (type.Count > 0)
            {
                build.Append(splitSign);
                for (int i = 1; i < data.tableKeyDict.Count; i++)
                {
                    string key = data.tableKeyDict[i];
                    string typeString = "";
                    if (data.tableTypesDict.ContainsKey(key))
                    {
                        typeString = data.tableTypesDict[key].ToString();
                        if (data.arraySplitFormatDict.ContainsKey(key))
                        {
                            typeString += "[";
                            foreach (var item in data.arraySplitFormatDict[key])
                            {
                                typeString += item;
                            }
                            typeString += "]";
                        }

                        if (data.tableEnumTypeDict.ContainsKey(key))
                        {
                            typeString += enumSplitSign + data.tableEnumTypeDict[key];
                        }
                    }
                    else
                    {
                        typeString = FieldType.String.ToString();
                    }

                    if (data.fieldAssetTypeDict.ContainsKey(key))
                    {
                        if (data.fieldAssetTypeDict[key] != DataFieldAssetType.Data)
                            typeString += "&" + data.fieldAssetTypeDict[key];
                    }

                    build.Append(typeString);
                    if (i != data.tableKeyDict.Count - 1)
                    {
                        build.Append(splitSign);
                    }
                    else
                    {
                        build.Append(newlineSign);
                    }
                }
            }
            else
            {
                build.Append(newlineSign);
            }

            List<string> noteValue = new List<string>(data.commentValueDict.Keys);
            build.Append(defaultNoteTableTitle);
            if (noteValue.Count > 0)
            {
                build.Append(splitSign);
                for (int i = 1; i < data.tableKeyDict.Count; i++)
                {
                    string key = data.tableKeyDict[i];
                    string defauleNoteTmp = "";

                    if (data.commentValueDict.ContainsKey(key))
                    {
                        defauleNoteTmp = data.commentValueDict[key];
                    }
                    else
                    {
                        defauleNoteTmp = "";
                    }

                    build.Append(defauleNoteTmp);

                    if (i != data.tableKeyDict.Count - 1)
                    {
                        build.Append(splitSign);
                    }
                    else
                    {
                        build.Append(newlineSign);
                    }
                }
            }
            else
            {
                build.Append(newlineSign);
            }

            List<string> defaultValue = new List<string>(data.defaultValueDict.Keys);
            build.Append(defaultTableTitleValue);
            if (defaultValue.Count > 0)
            {
                build.Append(splitSign);
                for (int i = 1; i < data.tableKeyDict.Count; i++)
                {
                    string key = data.tableKeyDict[i];
                    string defauleValueTmp = "";

                    if (data.defaultValueDict.ContainsKey(key))
                    {
                        defauleValueTmp = data.defaultValueDict[key];
                    }
                    else
                    {
                        defauleValueTmp = "";
                    }

                    build.Append(defauleValueTmp);

                    if (i != data.tableKeyDict.Count - 1)
                    {
                        build.Append(splitSign);
                    }
                    else
                    {
                        build.Append(newlineSign);
                    }
                }
            }
            else
            {
                build.Append(newlineSign);
            }

            foreach (string k in data.tableIDDict)
            {
                SingleData dataTmp = data[k];
                for (int i = 0; i < data.tableKeyDict.Count; i++)
                {
                    string valueTmp = "";
                    string field = data.tableKeyDict[i];
                    string defaultV = "";
                    if (data.defaultValueDict.ContainsKey(field))
                        defaultV = data.defaultValueDict[field];
                    if (dataTmp.ContainsKey(field) && dataTmp[field] != defaultV)
                    {
                        valueTmp = dataTmp[field];
                    }

                    build.Append(valueTmp);
                    if (i != data.tableKeyDict.Count - 1)
                    {
                        build.Append(splitSign);
                    }
                    else
                    {
                        build.Append(newlineSign);
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
                    if (lineContent[i] == splitSign)
                    {
                        result.Add(lineContent.Substring(startIndex, i - startIndex));
                        startIndex = i + 1;
                    }
                    else if (lineContent[i] == '\"')
                    {
                        state = false; // תΪ����״̬
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
            if (key == tableKeyDict[0])
            {
                return FieldType.String;
            }
            if (tableTypesDict.ContainsKey(key))
            {
                return tableTypesDict[key];
            }
            else
            {
                return FieldType.String;
            }
        }
        public char[] GetArraySplitFormat(string key)
        {
            if (arraySplitFormatDict.ContainsKey(key))
            {
                return arraySplitFormatDict[key];
            }
            return new char[0];
        }

        public void SetFieldType(string key, FieldType type, string enumType)
        {
            // ����ֻ����String����
            if (key == tableKeyDict[0])
            {
                return;
            }
            if (tableTypesDict.ContainsKey(key))
            {
                tableTypesDict[key] = type;
            }
            else
            {
                tableTypesDict.Add(key, type);
            }
            // �洢��������
            if (enumType != null)
            {
                if (tableEnumTypeDict.ContainsKey(key))
                {
                    tableEnumTypeDict[key] = enumType;
                }
                else
                {
                    tableEnumTypeDict.Add(key, enumType);
                }
            }
        }

        public void SetAssetTypes(string key, DataFieldAssetType type)
        {
            // ����ֻ����String����
            if (key == tableKeyDict[0])
            {
                return;
            }

            if (fieldAssetTypeDict.ContainsKey(key))
            {
                fieldAssetTypeDict[key] = type;
            }
            else
            {
                fieldAssetTypeDict.Add(key, type);
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
            if (tableEnumTypeDict.ContainsKey(key))
            {
                return tableEnumTypeDict[key];
            }
            else
            {
                return null;
            }
        }

        public string GetDefault(string key)
        {
            if (defaultValueDict.ContainsKey(key))
            {
                return defaultValueDict[key];
            }
            else
            {
                return null;
            }
        }

        public void SetDefault(string key, string value)
        {
            if (!defaultValueDict.ContainsKey(key))
            {
                defaultValueDict.Add(key, value);
            }
            else
            {
                defaultValueDict[key] = value;
            }
        }

        public void SetNote(string key, string note)
        {
            if (!commentValueDict.ContainsKey(key))
            {
                commentValueDict.Add(key, note);
            }
            else
            {
                commentValueDict[key] = note;
            }
        }

        public string GetNote(string key)
        {
            if (!commentValueDict.ContainsKey(key))
            {
                return null;
            }
            else
            {
                return commentValueDict[key];
            }
        }

        public void AddData(SingleData data)
        {
            if (data.ContainsKey(tableKeyDict[0]))
            {
                data.keyComment = data[tableKeyDict[0]];
                Add(data[tableKeyDict[0]], data);
                tableIDDict.Add(data[tableKeyDict[0]]);
            }
            else
            {
                throw new Exception("��FK��Add SingleData fail! The dataTable don't have MainKey!");
            }
        }

        public void SetData(SingleData data)
        {
            // ����
            string mainKey = tableKeyDict[0];
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
                    tableIDDict.Add(key);
                }
            }
            else
            {
                throw new Exception("��FK��Set SingleData fail! The dataTable don't have MainKeyKey!");
            }
        }

        public void RemoveData(string key)
        {
            if (ContainsKey(key))
            {
                Remove(key);
                tableIDDict.Remove(key);
            }
            else
            {
                throw new Exception("��FK��Remove SingleData fail! The dataTable don't have MainKeyKey!");
            }
        }
    }
}