using System.Text;
using System;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class EditorUtil
    {
        public static void WriteStringByFile(string path, string content)
        {
            byte[] dataByte = Encoding.GetEncoding("UTF-8").GetBytes(content);
            CreateFile(path, dataByte);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.Log("File:[" + path + "] doesn't exists");
            }
        }

        public static void CreateFile(string path, byte[] byt)
        {
            try
            {
                FileTool.CreatFilePath(path);
                File.WriteAllBytes(path, byt);
            }
            catch (Exception e)
            {
                Debug.LogError("File Create Fail! \n" + e.Message);
            }
        }
    }
}