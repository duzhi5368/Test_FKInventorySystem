namespace FKGame
{
    public interface IRecordConverter
    {
        string GetFileExtend();             // ��ȡ�ļ���׺��
        string GetSaveDirectoryName();      // ����Ŀ¼�����֣����ü�/��
        T String2Object<T>(string content); // ��textת��Ϊ��Ӧ���ݶ���
        string Object2String(object obj);   // ���ݶ���ת��String��ͨ�÷���
    }
}