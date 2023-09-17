using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SaveRecordManager
    {
        private Dictionary<string, Dictionary<string, string>> allRecords = new Dictionary<string, Dictionary<string, string>>();

        public IRecordConverter converter = new JsonRecordConverter();
        // 标准非自定义目录的储存
        public readonly static SaveRecordManager standard = new SaveRecordManager();
        // 自定义储存目录
        private string customDirectory = "";
        private string persistentDataPath;

        public SaveRecordManager()
        {
            persistentDataPath = Application.persistentDataPath;
        }
        // 设定自定义储存目录，如:Name或Name/PPP
        public SaveRecordManager SetCustomDirectory(string dirName)
        {
            customDirectory = dirName;
            return this;
        }
        public SaveRecordManager SetRecordConverter(IRecordConverter converter)
        {
            this.converter = converter;
            return this;
        }
        // 读取记录
        public T GetRecord<T>(string fileName, string key, T defaultValue = default(T))
        {
            Dictionary<string, string> fileContent = null;
            try
            {
                if (!allRecords.TryGetValue(fileName, out fileContent))
                {
                    string md5 = null;
                    string text = GetFileTextData(fileName, out md5);
                    fileContent = converter.String2Object<Dictionary<string, string>>(text);
                    if (fileContent == null)
                    {
                        fileContent = new Dictionary<string, string>();
                    }
                    allRecords.Add(fileName, fileContent);
                }

                if (fileContent != null)
                {
                    string valueStr = null;
                    if (fileContent.TryGetValue(key, out valueStr))
                    {
                        return converter.String2Object<T>(valueStr);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return defaultValue;
        }

        // 保存记录
        public void SaveRecord(string fileName, string key, object value)
        {
            Dictionary<string, string> fileContent = null;
            if (allRecords.ContainsKey(fileName))
            {
                fileContent = allRecords[fileName];
            }
            else
            {
                string md51 = null;
                string text = GetFileTextData(fileName, out md51);
                fileContent = converter.String2Object<Dictionary<string, string>>(text);
                if (fileContent == null)
                {
                    fileContent = new Dictionary<string, string>();
                }
                allRecords.Add(fileName, fileContent);
            }
            string valueStr = converter.Object2String(value);
            if (fileContent.ContainsKey(key))
            {
                fileContent[key] = valueStr;
            }
            else
            {
                fileContent.Add(key, valueStr);
            }
            string ss = converter.Object2String(fileContent);
            Save2File(fileName, ss);
        }

        public void Save2File(string fileName, string ss)
        {
            byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(ss);
            string md5 = MD5Utils.GetMD5Base64(dataByte);
            string length = md5.Length.ToString().PadLeft(4, '0');
            ss = length + md5 + ss;
            FileUtils.CreateTextFile(GetFilePath(fileName), ss);
        }

        // 清除某个文件记录
        public void DeleteRecord(string fileName)
        {
            if (allRecords.ContainsKey(fileName))
            {
                allRecords.Remove(fileName);
            }
            string path = GetFilePath(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        // 清除所有记录
        public void DeleteAllRecord()
        {
            allRecords.Clear();
            string pathDir = GetFileDir();
            if (Directory.Exists(pathDir))
            {
                Directory.Delete(pathDir, true);
            }
        }

        private string GetFilePath(string fileName)
        {
            return GetFileDir() + "/" + fileName + converter.GetFileExtend();
        }
        
        private string GetFileDir()
        {
            string dir = persistentDataPath + "/" + converter.GetSaveDirectoryName();
            if (!string.IsNullOrEmpty(customDirectory))
                dir += "/" + customDirectory;
            return dir;
        }

        private string GetFileTextData(string fileName, out string md5)
        {
            string path = GetFilePath(fileName);
            string text = null;
            md5 = null;
            if (File.Exists(path))
            {
                text = FileUtils.LoadTextFileByPath(path);
                try
                {
                    int length = int.Parse(text.Substring(0, 4));
                    md5 = text.Substring(4, length);
                    text = text.Substring(4 + length);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return text;
        }

        // 检查保存文件的完整性
        public bool CheckSaveFileMD5()
        {
            Debug.Log("【FK】Begin to check file integrity by MD5.");
            if (Directory.Exists(GetFileDir()))
            {
                string[] filePaths = PathUtils.GetDirectoryFilePath(GetFileDir());
                foreach (var path in filePaths)
                {
                    string fileName = Path.GetFileNameWithoutExtension(path);
                    string md5 = null;
                    string text = GetFileTextData(fileName, out md5);
                    Dictionary<string, string> fileContent = null;
                    if (allRecords.ContainsKey(fileName))
                    {
                        fileContent = allRecords[fileName];
                    }
                    else
                    {
                        fileContent = converter.String2Object<Dictionary<string, string>>(text);
                        if (fileContent == null)
                        {
                            fileContent = new Dictionary<string, string>();
                        }
                        allRecords.Add(fileName, fileContent);
                    }
                    if (!string.IsNullOrEmpty(md5))
                    {
                        byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(text);
                        string md5New = MD5Utils.GetMD5Base64(dataByte);
                        if (md5New != md5)
                        {
                            Debug.LogError("【FK】File：" + fileName + " md5 not match: " + md5 + " md5New:" + md5New + "\n" + text);
                            return false;
                        }
                    }
                    else
                    {
                        if (text != null && text.Length < 3)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}