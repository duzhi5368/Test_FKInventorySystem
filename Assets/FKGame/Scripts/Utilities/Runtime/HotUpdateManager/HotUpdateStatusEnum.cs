namespace FKGame
{
    public enum HotUpdateStatusEnum
    {
        NoUpdate,                   // �������
        NeedUpdateApplication,      // ��Ҫ��������

        VersionFileDownLoadFail,    // �汾�ļ�����ʧ��
        Md5FileDownLoadFail,        // Md5�ļ�����ʧ��
        UpdateFail,                 // ����ʧ��
        UpdateSuccess,              // ���³ɹ�

        DownLoadingVersionFile,     // ���ذ汾�ļ���
        DownLoadingManifestFile,    // �����嵥�ļ���
        Updating,                   // ������
    }
}