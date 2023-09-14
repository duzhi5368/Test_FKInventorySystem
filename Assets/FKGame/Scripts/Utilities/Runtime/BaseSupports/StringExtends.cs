using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class StringExtends
    {
        // �ָ��ض���ʼ�ַ����ͽ����ַ����������ַ���
        public static string[] SplitExtend(this string value, string startSign, string endSign)
        {
            List<string> results = new List<string>();

            string content = value;
            int startIndex = content.IndexOf(startSign);
            while (startIndex != -1)
            {
                int tempInt = startIndex + startSign.Length;

                int endIndex = content.IndexOf(endSign, tempInt);

                if (endIndex == -1)
                    break;
                else
                {
                    results.Add(content.Substring(tempInt, endIndex - tempInt));
                    content = content.Remove(0, endIndex + endSign.Length);
                    startIndex = content.IndexOf(startSign);
                }
            }

            return results.ToArray();
        }
    }
}