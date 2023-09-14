using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RandomString : MonoBehaviour
    {
        public List<string> subStringList;  // 所需的文件名（有先后顺序）
        public char splitChar = ',';        // 分割字符串
        private Dictionary<string, string[]> subStringsData;

        void Start(){}

        private void InitData()
        {
            if (subStringList == null)
            {
                return;
            }
            subStringsData = new Dictionary<string, string[]>();
            for (int i = 0; i < subStringList.Count; i++)
            {
                if (subStringList[i] == null)
                {
                    continue;
                }
                string[] subString = ResourceManager.LoadText(subStringList[i]).Split(splitChar);
                ResourceManager.DestoryAssetsCounter(subStringList[i]);
                subStringsData.Add(subStringList[i], subString);
            }
        }

        public void Init(List<string> l_subStringList, char l_splitChar)
        {
            subStringList = l_subStringList;
            splitChar = l_splitChar;
            InitData();
        }

        // 获取随机结果
        public string GetRandomResult(string[] l_subStringList, string[] l_addChar)
        {
            if (subStringsData == null || subStringsData.Count < 1)
            {
                return null;
            }
            string result = null;
            for (int i = 0; i < l_subStringList.Length; i++)
            {
                Debug.Log("l_subStringList==" + l_subStringList[i]);
                if (subStringsData.ContainsKey(l_subStringList[i]))
                {
                    result += GetSubString(subStringsData[l_subStringList[i]]);
                }
                if (l_addChar != null && l_addChar.Length > i)
                {
                    if (l_addChar[i] != "NoValue")
                    {
                        result += l_addChar[i];
                    }
                }
            }
            Debug.LogWarning(result);
            return result;
        }

        private string GetSubString(string[] data)
        {
            int index = Random.Range(0, data.Length);
            return data[index];
        }
    }
}