using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame 
{ 
    public enum SpecialPathType
    {
        Resources,
        Persistent,
        StreamingAssets,
    }
    public static class PathUtils
    {
        /// <summary>
        /// ����ƽ̨ת��·�����ӷ�
        /// </summary>
        /// <param name="path">����/���ָ��·��</param>
        /// <returns></returns>
        public static string CreatePlatformPath(string path)
        {
            string[] temps = path.Split(new char[] { '/' });
            string str = "";
            if (temps.Length > 0)
                str = temps[0];

            for (int i = 1; i < temps.Length; i++)
            {
                if (temps[i] != "")
                {
                    str = Path.Combine(str, temps[i]);
                }
            }

            return str;
        }
        /// <summary>
        /// �и�·��
        /// </summary>
        /// <param name="fullPath">·��</param>
        /// <param name="cutFolderName">Ҫ�и���ļ�����</param>
        /// <param name="returnLatterPart">�Ƿ񷵻غ�벿��</param>
        /// <param name="includeCutFolderName">�Ƿ�����и���ļ�����</param>
        /// <returns></returns>
        public static string CutPath(string fullPath, string cutFolderName, bool returnLatterPart = true, bool includeCutFolderName = false)
        {
            fullPath = fullPath.Replace("\\", "/");
            if (!fullPath.Contains(cutFolderName)) return fullPath;
            if (fullPath.Contains(cutFolderName + "/"))
            {
                if (returnLatterPart)
                {
                    if (includeCutFolderName)
                    {
                        return cutFolderName + fullPath.Split(new string[] { cutFolderName }, StringSplitOptions.None)[1];
                    }
                    else
                    {
                        return fullPath.Split(new string[] { cutFolderName + "/" }, StringSplitOptions.None)[1];
                    }
                }
                else
                {
                    if (includeCutFolderName)
                    {
                        return fullPath.Split(new string[] { cutFolderName }, StringSplitOptions.None)[0] + cutFolderName;
                    }
                    else
                    {
                        return fullPath.Split(new string[] { cutFolderName + "/" }, StringSplitOptions.None)[0];
                    }
                }
            }
            else
            {
                if (returnLatterPart)
                {

                    if (includeCutFolderName)
                    {
                        return cutFolderName + fullPath.Split(new string[] { cutFolderName }, StringSplitOptions.None)[1];
                    }
                    else
                    {
                        return fullPath.Split(new string[] { cutFolderName }, StringSplitOptions.None)[1];
                    }
                }
                else
                {
                    if (includeCutFolderName)
                    {
                        return fullPath.Split(new string[] { cutFolderName }, StringSplitOptions.None)[0] + cutFolderName;
                    }
                    else
                    {
                        return fullPath.Split(new string[] { cutFolderName }, StringSplitOptions.None)[0];
                    }
                }
            }
        }

        /// <summary>
        /// ���ݷ�ʽ��ȡ��Ӧ��·��
        /// </summary>
        /// <param name="assetPath">���·��</param>
        /// <param name="type">���ط�ʽ</param>
        /// <returns></returns>
        public static string GetSpecialPath(string assetPath, SpecialPathType type)
        {
            string path = assetPath;

            switch (type)
            {
                case SpecialPathType.Resources:
                    path = Application.dataPath + "/Resources/" + path;
                    break;
                case SpecialPathType.Persistent:
                    path = Application.persistentDataPath + "/" + path;
                    break;
                case SpecialPathType.StreamingAssets:
                    path = Application.streamingAssetsPath + "/" + path;
                    break;
                    //#endif
            }
            return path;
        }

        public static string RemoveExtension(string path)
        {
            string ss = path;
            if (Path.HasExtension(path))
                ss = Path.ChangeExtension(path, null);
            return ss;
        }
        /// <summary>
        /// �Ƴ��涨��׺����·��
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="endsWith"></param>
        /// <returns></returns>
        public static string[] RemovePathWithEnds(string[] paths, string[] endsWith)
        {
            if (endsWith == null && endsWith.Length == 0)
                return paths;
            List<string> resPath = new List<string>();
            List<string> temp = new List<string>(endsWith);

            for (int i = 0; i < paths.Length; i++)
            {
                string s = Path.GetExtension(paths[i]);
                if (temp.Contains(s))
                    continue;
                else
                    resPath.Add(paths[i]);
            }
            return resPath.ToArray();
        }
        /// <summary>
        /// ��ȡָ��Ŀ¼�µ������ļ�·��
        /// </summary>
        /// <param name="path">Ŀ¼</param>
        /// <param name="endsWith">�ļ���׺���硰.txt��</param>
        /// <returns>���������ļ���ȫ·��</returns>
        public static string[] GetDirectoryFilePath(string path, string[] endsWith = null, bool isIncludeChildFolder = true)
        {

            List<string> pathList = new List<string>();
            if (!Directory.Exists(path))
            {
                Debug.LogError("������Ŀ¼��" + path);
                return new string[0];
            }

            if (isIncludeChildFolder)
            {
                string[] directorys = Directory.GetDirectories(path);
                // ����Ŀ¼��������
                for (int i = 0; i < directorys.Length; i++)
                {
                    string pathTmp = directorys[i];


                    string[] tempArray = GetDirectoryFilePath(pathTmp, endsWith);
                    pathList.AddRange(tempArray);

                }
            }

            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                string pathTmp = files[i];
                pathTmp = pathTmp.Replace("\\", "/");
                string ends = Path.GetExtension(pathTmp);
                if (endsWith != null && endsWith.Length > 0)
                {
                    for (int j = 0; j < endsWith.Length; j++)
                    {
                        if (ends.Equals(endsWith[j]))
                        {
                            pathList.Add(pathTmp);
                            break;
                        }
                    }
                }
                else
                {
                    pathList.Add(pathTmp);
                }
            }
            return pathList.ToArray();
        }
        /// <summary>
        /// ��ȡָ��Ŀ¼�µ������ļ�����
        /// </summary>
        /// <param name="path"></param>
        /// <param name="endsWith"></param>
        /// <param name="isNeedExtension">�Ƿ���Ҫ�ļ���׺��</param>
        /// <returns></returns>
        public static string[] GetDirectoryFileNames(string path, string[] endsWith, bool isNeedExtension = false, bool isIncludeChildFolder = true)
        {
            string[] temps = GetDirectoryFilePath(path, endsWith, isIncludeChildFolder);
            List<string> names = new List<string>();
            for (int i = 0; i < temps.Length; i++)
            {
                if (isNeedExtension)
                {
                    names.Add(Path.GetFileName(temps[i]));
                }
                else
                    names.Add(Path.GetFileNameWithoutExtension(temps[i]));
            }
            return names.ToArray();
        }

        public static string[] GetDirectorys(string path, bool isIncludeChildFolder = true)
        {
            List<string> pathList = new List<string>();
            if (Directory.Exists(path))
            {
                string[] directorys = Directory.GetDirectories(path);
                pathList.AddRange(directorys);

                if (isIncludeChildFolder)
                {
                    //����Ŀ¼��������
                    for (int i = 0; i < directorys.Length; i++)
                    {
                        string pathTmp = directorys[i];

                        string[] tempArray = GetDirectorys(pathTmp);
                        pathList.AddRange(tempArray);
                    }
                }
            }
            return pathList.ToArray();
        }
        public static string GetFileName(string path)
        {
            string name = "";
            try
            {
                name = Path.GetFileName(path);
            }
            catch
            {
                path = path.Replace("\\", "/");
                if (!path.Contains("/"))
                {
                    return path;
                }
                string[] ss = path.Split(new string[] { "/" }, StringSplitOptions.None);
                if (ss.Length > 0)
                    return ss[ss.Length - 1];
            }
            return name;
        }
    }
}