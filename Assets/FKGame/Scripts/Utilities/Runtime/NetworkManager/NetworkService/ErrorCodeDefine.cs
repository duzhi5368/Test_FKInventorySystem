namespace FKGame
{
    public class ErrorCodeDefine
    {
        public const int Success = 0;

        //==============��¼================//
        /**
         * ����ĵ�¼����
         */
        public const int Login_WrongAccountOrPassword = 20000;
        /**
         * ��¼��֤ʧ��
         */
        public const int Login_VerificationFailed = 20001;
        /**
         * ����ظ���¼
         */
        public const int Login_OtherPlaceLogin = 20002;

        //=================�˺źϲ�=============//
        /**
         * ���ܰ��Լ�
         */
        public const int AccountMerge_CantBindSelf = 20100;
        /**
         * ���˺��Ѿ�����
         */
        public const int AccountMerge_AccountAlreadyBind = 20101;
        /**
         * ���˺��Ѿ��󶨵�ǰ��¼����
         */
        public const int AccountMerge_LoginTypeAlreadyBind = 20102;
        /**
         * �Է��˺��Ѿ��󶨵�ǰ��¼����
         */
        public const int AccountMerge_LoginTypeAlreadyBeBind = 20104;
        /**
         * ��ӦҪ�󶨵��˻�������
         */
        public const int AccountMerge_NoUser = 20103;

        //=================�̵깦��===========//
        /**
         * �����֧���ɹ���Ϸ�߼�δʵ��
         */
        public const int StroePay_NoGameLogic = 20200;

        /**
         * �̵���Ʒ���������Ҳ���
         */
        public const int StroePay_ConfigError = 20201;
        /**
         * �̵����δ֪����
         */
        public const int StroePay_StoreError = 20202;
        /**
         * �����ظ�
         */
        public const int StorePay_RepeatReceipt = 20203; //�����ظ�
        /**
         * �������ƷID
         */
        public const int StorePay_ErrorGoodsID = 20204; //�������ƷID
        /**
         * �̵���֤ʧ��
         */
        public const int StroePay_VerificationFailed = 20205;
        /// <summary>
        /// û�е�¼����֧��
        /// </summary>
        public const int StroePay_NoLogin = 20206;

        //===============�һ��빦��===========//
        /**
         * û�жһ���
         */
        public const int RedeemCode_DontHave = 30000;
        /**
         * �һ��������ڻ�û��ʼ
         */
        public const int RedeemCode_NotStart = 30001;
        /**
         * �һ������
         */
        public const int RedeemCode_Overdue = 30002;
        /**
         * �һ���ʧЧ
         */
        public const int RedeemCode_CantUse = 30003;
        /**
         * �һ��벻���ظ�ʹ��
         */
        public const int RedeemCode_CantRepeatUse = 30004;

        /**
         * �öһ��빦��ûʵ��
         */
        public const int RedeemCode_FunctionClassNoFound = 30005;
        /**
         * �öһ��빦�ܳ���
         */
        public const int RedeemCode_Error = 30006;
        /**
         * �һ����ǿյ�
         */
        public const int RedeemCode_CodeIsNull = 30007;
        /**
         * �öһ��벻�Ǽ����빦��
         */
        public const int RedeemCode_NotActivationCode = 30008;

        //=================ͨ���̵�===============//
        /***
         * û���̵��߼�
         */
        public const int GeneralGameShop_NoLogic = 30100;
        /***
         * �ﵽ������������
         */
        public const int GeneralGameShop_NumberLimit = 30101;
        /***
         * ��ǰʱ��β��ܹ���(�����ܹ����ʱ�����)
         */
        public const int GeneralGameShop_TimeRangeLimit = 30102;

        /***
         * ������Ŀ���㲻�ܹ���
         */
        public const int GeneralGameShop_CoinNotEnough = 30103;

        /***
         * �̵깺�����
         */
        public const int GeneralGameShop_Error = 30104;
    }
}