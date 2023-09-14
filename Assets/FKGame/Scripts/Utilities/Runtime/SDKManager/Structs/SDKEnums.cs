namespace FKGame
{
    // ��½����ö��
    public enum LoginErrorEnum
    {
        None,
        GameCenterNotOpen,
        NoInstallApp,       // û�а�װ��Ӧ��app
        SDKError            // SDK ����Ĵ���
    }

    // ����֧���̵�����
    public enum StoreName
    {
        None = 0,
        GooglePay = 1,
        AmazonAppStore = 2,
        AppleAppStore = 3,
        Windows = 4,
        FacebookStore = 5,
    }

    public enum GoodsType
    {
        NORMAL,             // ���Է����������Ʒ
        ONCE_ONLY,          // ֻ�ܹ���һ�ε���Ʒ
        RIGHTS,             // �������һ��ʱ�����Ʒ�������Ա
    }

    // ֧����������
    public enum PayLimitType
    {
        None,           // ������
        NoRealName,     // δ���ʵ����֤
        ChildLimit,     // δ���꣬����������
    }

    // SDK�Ĺ�������
    public enum SDKType
    {
        Log,
        Login,
        AD,
        Pay,
        RealName,
        Other,
    }

    // �鿴���Ľ��
    public enum ADResult
    {
        Load_Success,
        Load_Failure,

        Show_Start,
        Show_Click,
        Show_Failed,
        Show_Skipped,
        Show_Finished
    }
}