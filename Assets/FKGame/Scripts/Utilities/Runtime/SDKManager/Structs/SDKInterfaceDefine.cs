namespace FKGame
{
    public class SDKInterfaceDefine
    {
        public const string ModuleName = "ModuleName";
        public const string FunctionName = "FunctionName";
        public const string ListenerName = "ListenerName";

        public const string SDKName = "SDKName";
        public const string SDKIndex = "SDKIndex";
        public const string Tag = "Tag";

        public const string ParameterName_IsSuccess = "IsSuccess";
        public const string ParameterName_Error = "Error";
        public const string ParameterName_Content = "Content";
        public const string ParameterName_UserID = "UserID";

        public const string ModuleName_Init = "Init";
        public const string ModuleName_Dispose = "Dispose";
        public const string ModuleName_Debug = "Debug";

        public const string ModuleName_Login = "Login";
        public const string ModuleName_Pay = "Pay";
        public const string ModuleName_AD = "AD";
        public const string ModuleName_Log = "Log";
        public const string ModuleName_Other = "Other";
        public const string ModuleName_LifeCycle = "LifeCycle";
        public const string ModuleName_RealName = "RealName";

        //�ص�����
        public const string FunctionName_OnError = "OnError";
        public const string FunctionName_OnLog = "OnLog";

        public const string FunctionName_OnInit = "OnInit";
        public const string FunctionName_OnLogin = "OnLogin";
        public const string FunctionName_OnLogout = "OnLogout";
        public const string FunctionName_OnPay = "OnPay";
        public const string FunctionName_OnOther = "OnOther";

        //LifeCycle ��ز���
        public const string LifeCycle_FunctionName_OnApplicationQuit = "OnApplicationQuit";

        //Login��ز���
        public const string Login_FunctionName_Login = "Login";
        public const string Login_FunctionName_LoginOut = "LoginOut";

        public const string Login_ParameterName_Device = "Device";
        public const string Login_ParameterName_AccountId = "AccountId";
        public const string Login_ParameterName_loginPlatform = "loginPlatform";
        public const string Login_ParameterName_NickName = "NickName";
        public const string Login_ParameterName_HeadPortrait = "HeadPortrait";

        //Pay��ز���
        public const string Pay_FunctionName_OnPay = "OnPay";//֧���ص�
        public const string Pay_FunctionName_GetGoodsInfo = "GetGoodsInfo";//��ȡ��Ʒ��Ϣ�ص�
        public const string Pay_FunctionName_ClearPurchase = "ClearPurchase";//���������¼��������������
        public const string Pay_ParameterName_GoodsID = "GoodsID";
        public const string Pay_ParameterName_GoodsType = "GoodsType";
        public const string Pay_ParameterName_Count = "Count";
        public const string Pay_ParameterName_GoodsName = "GoodsName";
        public const string Pay_ParameterName_GoodsDescription = "GoodsDescription";
        public const string Pay_ParameterName_CallBackUrl = "CallBackUrl";
        public const string Pay_ParameterName_CpOrderID = "CpOrderID";//������֧��ID
        public const string Pay_ParameterName_OrderID = "OrderID";  //�����Լ���֧��ID
        public const string Pay_ParameterName_PrepayID = "PrepayID "; //Ԥ֧������id
        public const string Pay_ParameterName_Price = "Price";  //�۸�
        public const string Pay_ParameterName_Currency = "Currency";  //����
        public const string Pay_ParameterName_Payment = "Payment";   //֧��;��
        public const string Pay_ParameterName_Receipt = "Receipt";   //֧����ִ
        public const string Pay_ParameterName_LocalizedPriceString = "localizedPriceString";//���ػ�����������Ŀ

        public const string Pay_GoodsTypeEnum_ONCE_ONLY = "ONCE_ONLY";
        public const string Pay_GoodsTypeEnum_NORMAL = "NORMAL";
        public const string Pay_GoodsTypeEnum_RIGHTS = "RIGHTS";

        //AD��ز���
        public const string AD_ParameterName_ADType = "ADType";
        public const string AD_ParameterName_ADResult = "ADResult";   //��沥�Ž��
        public const string AD_ParameterName_VideoADKey = "VideoADKey";//��Ƶ���Key

        public const string AD_FunctionName_LoadAD = "LoadAD";
        public const string AD_FunctionName_PlayAD = "PlayAD";
        public const string AD_FunctionName_CloseAD = "CloseAD";
        public const string AD_FunctionName_ADIsLoaded = "ADIsLoaded";
        public const string AD_FunctionName_OnAD = "OnAD";


        //Log��ز���
        public const string Log_FunctionName_Login = "LogLogin";
        public const string Log_FunctionName_LoginOut = "LogLoginOut";
        public const string Log_FunctionName_Event = "LogEvent";
        public const string Log_FunctionName_LogPay = "LogPay";
        public const string Log_FunctionName_LogPaySuccess = "LogPaySuccess";
        public const string Log_FunctionName_RewardVirtualCurrency = "LogRewardVirtualCurrency"; //���������
        public const string Log_FunctionName_PurchaseVirtualCurrency = "LogPurchaseVirtualCurrency";//���������
        public const string Log_FunctionName_UseItem = "LogUseItem";//ʹ��������Ʒ��ͨ������ҹ���ģ�

        //Log Login���
        public const string Log_ParameterName_AccountId = "AccountId";

        //Log VirtualCurrency���
        public const string Log_ParameterName_RewardReason = "RewardReason";

        //Log Event���
        public const string Log_ParameterName_EventID = "EventID";
        public const string Log_ParameterName_EventLabel = "EventLabel";
        public const string Log_ParameterName_EventMap = "EventMap";

        //realName���
        public const string RealName_FunctionName_OnLogin = "OnLogin";                                  //��¼
        public const string RealName_FunctionName_OnLogout = "OnLogout";                                //�ǳ�
        public const string RealName_FunctionName_GetRealNameType = "GetRealNameType";                  //���ʵ����֤״̬
        public const string RealName_FunctionName_IsAdult = "IsAdult";                                   //�Ƿ����
        public const string RealName_FunctionName_LogPayAmount = "LogPayAmount";                         //�ϱ�֧�����
        public const string RealName_FunctionName_CheckPayLimit = "CheckPayLimit";                       //��ѯ�Ƿ�֧��������
        public const string RealName_FunctionName_GetTodayOnlineTime = "GetTodayOnlineTime";             //��ȡ��������ʱ��
        public const string RealName_FunctionName_StartRealNameAttestation = "StartRealNameAttestation";//��ʼʵ����֤
        public const string RealName_FunctionName_RealNameCallBack = "RealNameCallBack";                  //ʵ����֤��ɺ�Ļص�
        public const string RealName_FunctionName_PayLimitCallBack = "PayLimitCallBack";                //ѯ��֧�����ƵĻص�
        public const string RealName_FunctionName_RealNameLogout = "RealNameLogout";                     //ʵ��ϵͳ ֪ͨ�߼����˳���¼
        public const string RealName_ParameterName_RealNameStatus = "RealNameStatus";                    //ʵ����֤��״̬
        public const string RealName_ParameterName_IsAdult = "IsAdult";                                 //�Ƿ��ǳ�����
        public const string RealName_ParameterName_IsPayLimit = "IsPayLimit";                           //�Ƿ�����֧��
        public const string RealName_ParameterName_PayAmount = "PayAmount";                             //֧�����

        //Other��ز���
        public const string Other_FunctionName_Exit = "Exit";

        //Other -> ������
        public const string Other_FunctionName_CopyToClipboard = "CopyToClipboard";
        public const string Other_FunctionName_CopyFromClipboard = "CopyFromClipboard";
        public const string Other_ParameterName_Content = "Content";

        //Other -> �ȸ��°�װ��
        public const string Other_FunctionName_DownloadAPK = "DownloadAPK";
        public const string Other_FunctionName_GetAPKSize = "GetAPKSize";
        public const string Other_ParameterName_DownloadURL = "DownloadURL";
        public const string Other_ParameterName_Progress = "Progress";
        public const string Other_ParameterName_TotalProgress = "TotalProgress";
        public const string Other_ParameterName_Size = "Size";

        //Other -> ��ȡ�ֻ�ͨѶ¼
        public const string Other_FunctionName_GetPhoneNumberList = "GetPhoneNumberList";

        //Properties
        public const string FileName_ChannelProperties = "Channel";                  //�ļ���
        public const string PropertiesKey_IsLog = "IsLog";                           //�Ƿ������־
        public const string PropertiesKey_SelectNetworkPath = "SelectNetworkPath";   //ѡ���������ص�ַ
        public const string PropertiesKey_UpdateDownLoadPath = "UpdateDownLoadPath"; //�ȸ������ص�ַ(���û��б��)
        public const string PropertiesKey_TestUpdateDownLoadPath = "TestUpdateDownLoadPath"; //�ȸ������ز��Ե�ַ(���û��б��)
        public const string PropertiesKey_ChannelName = "ChannelName";               //��������
        public const string PropertiesKey_StoreName = "StoreName";                 //�̵����ƣ������֧����ʽ��@���зָ�
        public const string PropertiesKey_LoginPlatform = "LoginPlatform";           //��¼ƽ̨
        public const string PropertiesKey_ADPlatform = "ADPlatform";                 //���ƽ̨
        public const string PropertiesKey_NetworkID = "NetworkID";                   //������ѡ��
        public const string PropertiesKey_QQGroup = "QQGroup";                     //���QQȺ
        public const string PropertiesKey_AllContact = "AllContact";                 //������ϵ��ʽ�����߷ָ�
        public const string PropertiesKey_DirectlyLogin = "DirectlyLogin";           //�Ƿ�ֱ�ӵ�¼����ѡ���¼ģʽ
        public const string PropertiesKey_CloseHealthGamePact = "CloseHealthGamePact";//�رս�����Ϸ��Լ
        public const string PropertiesKey_OpenRealName = "OpenRealName";              // ����ʵ����֤
        public const string PropertiesKey_IsExamine = "IsExamine";                    //��˰汾
        public const string PropertiesKey_RedeemCode = "RedeemCode";                  //�һ���
        public const string PropertiesKey_HideAllAD = "HideAllAD";                    //�������й�水ť
        public const string PropertiesKey_CloseGuide = "CloseGuide";                  //�ر���������
        public const string PropertiesKey_SDKLogo = "SDKLogo";                        //SDK logo 
        public const string PropertiesKey_LogoShowTime = "LogoShowTime";              //��ʾ��һ��logo n �� �� 0 ��ʾ����ԭ��logo
        public const string PropertiesKey_CanPay = "CanPay";                          //����֧��

        public const string PropertiesKey_SelectServerURL = "SelectServerURL";       //ѡ����������ַ

        public const string PropertiesKey_SelectServerOSSURL = "SelectServerOSSURL";       //ѡ�������� OSS��ַ
    }
}