using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ���װ�  ���к���Ҫ��һ��unity�⣬�ٵ�������������¼����ı������ɲ鿴���
    public class MaskWordMakeTool : MonoBehaviour
    {
        public string maskDataBaseName = "MaskWordData";                        // ԭ�����ֿ��ı�
        public string savePath = "/Resources/Data/NameWorld/MaskWordData3.txt"; // �洢·��
        private string maskDataBase;                                            // ԭ�������ı�

        private void Awake()
        {
            maskDataBase = ResourceManager.Load<TextAsset>(maskDataBaseName).text;
            maskDataBase = maskDataBase.Replace(',', '��');
            string[] words = maskDataBase.Split('��');
            string newMaskData = "";
            for (int i = 0; i < words.Length; i++)
            {
                newMaskData += words[i] + "," + "\n";
            }
            ResourceIOTool.WriteStringByFile(Application.dataPath + savePath, newMaskData);
        }
    }
}