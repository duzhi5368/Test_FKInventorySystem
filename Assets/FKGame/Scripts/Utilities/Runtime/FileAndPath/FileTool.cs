using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class FileTool
    {
        // �ж���û������ļ�·�������û���򴴽���(·����ȥ���ļ���)
        public static void CreatFilePath(string filepath)
        {
            string newPathDir = Path.GetDirectoryName(filepath);

            CreatPath(newPathDir);
        }

        // �ж���û�����·�������û���򴴽���
        public static void CreatPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        // ɾ��ĳ��Ŀ¼�µ�������Ŀ¼�����ļ������Ǳ������Ŀ¼
        public static void DeleteDirectory(string path)
        {
            string[] directorys = Directory.GetDirectories(path);
            //ɾ��������Ŀ¼
            for (int i = 0; i < directorys.Length; i++)
            {
                string pathTmp = directorys[i];

                if (Directory.Exists(pathTmp))
                {
                    Directory.Delete(pathTmp, true);
                }
            }
            //ɾ���������ļ�
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                string pathTmp = files[i];

                if (File.Exists(pathTmp))
                {
                    File.Delete(pathTmp);
                }
            }
        }

        // �����ļ��У����ļ������������ļ��к��ļ���
        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);

            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = Path.Combine(destinationPath, fsi.Name);
                if (fsi is FileInfo)          //������ļ��������ļ�
                    File.Copy(fsi.FullName, destName);
                else                                    //������ļ��У��½��ļ��У��ݹ�
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        // ɾ�����п���ɾ�����ļ�
        public static void SafeDeleteDirectory(string path)
        {
            string[] directorys = Directory.GetDirectories(path);
            // ɾ��������Ŀ¼
            for (int i = 0; i < directorys.Length; i++)
            {
                string pathTmp = directorys[i];
                if (Directory.Exists(pathTmp))
                {
                    SafeDeleteDirectory(pathTmp);
                    try
                    {
                        Directory.Delete(pathTmp, false);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }

            //ɾ���������ļ�
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                string pathTmp = files[i];
                if (File.Exists(pathTmp))
                {
                    try
                    {
                        File.Delete(pathTmp);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }
        }

        // �������п��Ը��Ƶ��ļ��У����ļ������������ļ��к��ļ���
        public static void SafeCopyDirectory(string sourcePath, string destinationPath)
        {
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);

            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = Path.Combine(destinationPath, fsi.Name);
                if (fsi is FileInfo)          //������ļ��������ļ�
                {
                    try
                    {
                        File.Copy(fsi.FullName, destName);
                    }
                    catch { }
                }
                else                            //������ļ��У��½��ļ��У��ݹ�
                {
                    Directory.CreateDirectory(destName);
                    SafeCopyDirectory(fsi.FullName, destName);
                }
            }
        }


        // �Ƴ���չ��
        public static string RemoveExpandName(string name)
        {
            if (Path.HasExtension(name))
                name = Path.ChangeExtension(name, null);
            return name;
        }

        public static string GetExpandName(string name)
        {
            return Path.GetExtension(name);
        }

        // ȡ��һ��·���µ��ļ���
        public static string GetFileNameByPath(string path)
        {
            FileInfo fi = new FileInfo(path);
            return fi.Name;
        }

        // ȡ��һ�����·���µ��ļ���
        public static string GetFileNameBySring(string path)
        {
            string[] paths = path.Split('/');
            return paths[paths.Length - 1];
        }

        public static string GetUpperPath(string path)
        {
            int index = path.LastIndexOf('/');

            if (index != -1)
            {
                return path.Substring(0, index);
            }
            else
            {
                return "";
            }
        }

        // �޸��ļ���
        public static void ChangeFileName(string path, string newName)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Move(path, newName);
            }
        }

        // �ļ�����ת��
        public static void ConvertFileEncoding(string sourceFile, string destFile, System.Text.Encoding targetEncoding)
        {
            destFile = string.IsNullOrEmpty(destFile) ? sourceFile : destFile;
            Encoding sourEncoding = GetEncodingType(sourceFile);

            System.IO.File.WriteAllText(destFile, System.IO.File.ReadAllText(sourceFile, sourEncoding), targetEncoding);
        }

        // �����ļ���·������ȡ�ļ��Ķ��������ݣ��ж��ļ��ı������� 
        public static Encoding GetEncodingType(string FILE_NAME)
        {
            FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
            Encoding r = GetEncodingType(fs);
            fs.Close();
            return r;
        }


        // ͨ���������ļ������ж��ļ��ı������� 
        public static Encoding GetEncodingType(FileStream fs)
        {
            //byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            //byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            //byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //��BOM 
            Encoding reVal = Encoding.Default;
            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        //�ж��Ƿ��ǲ��� BOM �� UTF8 ��ʽ 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;
            // ���㵱ǰ���������ַ�Ӧ���е��ֽ��� 
            byte curByte; //��ǰ�������ֽ�. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        // �жϵ�ǰ 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        // ���λ��λ��Ϊ��0 ��������2��1��ʼ ��:110XXXXX......1111110X�� 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // ����UTF-8 ��ʱ��һλ����Ϊ1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("��Ԥ�ڵ�byte��ʽ");
            }
            return true;
        }

        /// <summary>
        /// �ݹ鴦��ĳ·������������Ŀ¼
        /// </summary>
        /// <param name="path">Ŀ��·��</param>
        /// <param name="expandName">Ҫ������ض���չ��</param>
        /// <param name="handle">������</param>
        public static void RecursionFileExecute(string path, string expandName, FileExecuteHandle handle)
        {
            string[] allUIPrefabName = Directory.GetFiles(path);
            foreach (var item in allUIPrefabName)
            {
                try
                {
                    if (expandName != null)
                    {
                        if (item.EndsWith("." + expandName))
                        {
                            handle(item);
                        }
                    }
                    else
                    {
                        handle(item);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("RecursionFileExecute Error :" + item + " Exception:" + e.ToString());
                }
            }

            string[] dires = Directory.GetDirectories(path);
            for (int i = 0; i < dires.Length; i++)
            {
                RecursionFileExecute(dires[i], expandName, handle);
            }
        }

        // ��ȡһ��·���µ������ļ�
        public static List<string> GetAllFileNamesByPath(string path, string[] expandNames = null)
        {
            List<string> list = new List<string>();

            RecursionFind(list, path, expandNames);

            return list;
        }

        static void RecursionFind(List<string> list, string path, string[] expandNames)
        {
            string[] allUIPrefabName = Directory.GetFiles(path);
            foreach (var item in allUIPrefabName)
            {
                if (ExpandFilter(item, expandNames))
                {
                    list.Add(item);
                }
            }
            string[] dires = Directory.GetDirectories(path);
            for (int i = 0; i < dires.Length; i++)
            {
                RecursionFind(list, dires[i], expandNames);
            }
        }

        static bool ExpandFilter(string name, string[] expandNames)
        {
            if (expandNames == null)
            {
                return true;
            }
            else if (expandNames.Length == 0)
            {
                return !name.Contains(".");
            }
            else
            {
                for (int i = 0; i < expandNames.Length; i++)
                {
                    if (name.EndsWith("." + expandNames[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    public delegate void FileExecuteHandle(string filePath);
}
