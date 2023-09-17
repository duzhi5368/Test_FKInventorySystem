using FKGame;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
public class TestReaderAndWriter : MonoBehaviour
{
    private const string testConfigFile = "testConfigManager";
    private const string testConfigDataKey1 = "testConfigInfo1";
    private const string testConfigDataKey2 = "testConfigInfo2";
    private class TestConfigSchemeDatas
    {
        public string name = string.Empty;
        public bool really = false;
        public List<TestConfigSchemeData> data1 = null;
        public List<TestConfigSchemeData> data2 = null;
    }
    private class TestConfigSchemeData
    {
        public string name = string.Empty;
        public string content = string.Empty;
    }

    void Start()
    {
        TestConfigManager();
        TestDataManager();
        TestRecordManager();
    }

    void TestConfigManager()
    {
        Debug.Log("¡¾FK¡¿Test config manager begin.");
        if (!ConfigManager.IsConfigExist(testConfigFile))
        {
            Debug.LogError("¡¾FK¡¿Can't find config file " + testConfigFile );
            return;
        }
        Dictionary<string, SingleField> configData = ConfigManager.GetData(testConfigFile);
        if(configData.ContainsKey(testConfigDataKey1))
        {
            TestConfigSchemeDatas data = JsonUtility.FromJson<TestConfigSchemeDatas>(configData[testConfigDataKey1].GetString());
            Debug.Log(data.data1[0].content);
        }
        if (configData.ContainsKey(testConfigDataKey2))
        {
            string[] valueList = configData[testConfigDataKey2].GetStringArray();
            for (int i = 0; i < valueList.Length; i++)
            {
                Debug.Log(valueList[i]);
            }
        }
    }

    void TestDataManager()
    {
        Debug.Log("¡¾FK¡¿Test data manager begin.");
    }

    void TestRecordManager()
    {
        Debug.Log("¡¾FK¡¿Test record manager begin.");
    }
}
