using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using System.Collections;
//------------------------------------------------------------------------
namespace FKGame
{
    public class FileUtils
    {
        // ����һ��Ŀ¼�µ������ļ�����һ��Ŀ¼
        public static void CopyDirectory(string oldDirectory, string newDirectory, bool overwrite = true)
        {
            string[] pathArray = PathUtils.GetDirectoryFilePath(oldDirectory);
            for (int i = 0; i < pathArray.Length; i++)
            {
                string newPath = pathArray[i].Replace(oldDirectory, newDirectory);
                string s = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(s))
                {
                    Directory.CreateDirectory(s);
                }
                File.Copy(pathArray[i], newPath, overwrite);
            }
        }

        public static void MoveFile(string oldFilePath, string newFilePath, bool overwrite = true)
        {
            if (!File.Exists(oldFilePath) || oldFilePath == newFilePath)
                return;
            string s = Path.GetDirectoryName(newFilePath);
            if (!Directory.Exists(s))
            {
                Directory.CreateDirectory(s);
            }
            File.Copy(oldFilePath, newFilePath, overwrite);
            DeleteFile(oldFilePath);
        }
        public static string LoadTextFileByPath(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("path dont exists ! : " + path);
                return "";
            }
            return File.ReadAllText(path, Encoding.UTF8);

        }
        public static byte[] LoadByteFileByPath(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("path dont exists ! : " + path);
                return null;
            }
            FileStream fs = new FileStream(path, FileMode.Open);

            byte[] array = new byte[fs.Length];

            fs.Read(array, 0, array.Length);
            fs.Close();

            return array;
        }
        /// <summary>
        /// ����txt ����ÿһ������
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] LoadTextFileLineByPath(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("path dont exists ! : " + path);
                return null;
            }

            StreamReader sr = File.OpenText(path);
            List<string> line = new List<string>();
            string tmp = "";
            while ((tmp = sr.ReadLine()) != null)
            {
                line.Add(tmp);
            }

            sr.Close();
            sr.Dispose();

            return line.ToArray();

        }
        public static bool DeleteFile(string path)
        {
            FileInfo t = new FileInfo(path);
            try
            {

                if (t.Exists)
                {
                    t.Delete();
                }
                Debug.Log("File Delete: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("File delete fail: " + path + "  ---:" + e);
                return false;
            }

            return true;
        }
        /// <summary>
        /// �����ļ�����
        /// </summary>
        /// <param name="path">ȫ·��</param>
        /// <param name="_data">����</param>
        /// <returns></returns>
        public static bool CreateTextFile(string path, string _data)
        {

            byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(_data);

            return CreateFile(path, dataByte);
        }
        public static bool CreateFile(string path, byte[] _data)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            string temp = Path.GetDirectoryName(path);
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (FileStream stream = new FileStream(path.ToString(), FileMode.OpenOrCreate))
                {
                    stream.Write(_data, 0, _data.Length);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("File written fail: " + path + "  ---:" + e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ��ȡ�ļ�MD5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string filePath)
        {
            try
            {
                FileInfo fileTmp = new FileInfo(filePath);
                if (fileTmp.Exists)
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    int len = (int)fs.Length;
                    byte[] data = new byte[len];
                    fs.Read(data, 0, len);
                    fs.Close();

                    return MD5Utils.GetMD5(data);

                }
                return "";
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

#pragma warning disable CS0618
        public static IEnumerator LoadTxtFileIEnumerator(string path, CallBack<string> callback)
        {

            WWW www = new WWW(path);
            yield return www;

            string data = "";
            if (string.IsNullOrEmpty(www.error))
            {
                data = www.text;
            }
            if (callback != null)
                callback(data);
            yield return new WaitForEndOfFrame();
        }
#pragma warning restore CS0618
    }
}