using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ֧���̵������
    public static class StorePayController
    {
        public const int c_PayCode_repeate = 20203;                                                         // �ظ�����
        private static User user;
        public static CallBack<PayResult> OnPayCallBack;                                                    // ֧����ɻص���code�룬��ƷID,error��
        public static CallBack<LocalizedGoodsInfo, List<PayPlatformInfo>> NeedSelectPayPlatformCallBack;    // ����Ҫѡ��֧����ʽʱ����ܻ�ص���CallBack����Ҫ�߼��������
        public static CallBack<LocalizedGoodsInfo, PayPlatformInfo> OnSelectPayPlatformCallBack;            // ѡ����֧����ʽʱ�ص�(�߼��㲻��Ҫ����)
        public static List<LocalizedGoodsInfo> productDefinitions;
        private static LocalizedGoodsInfo m_goodsInfo;
        private static PayPlatformInfo m_payPlatform;

        [RuntimeInitializeOnLoadMethod]
        private static void EventAdd()
        {
            NetworkVerificationImplement implement = new NetworkVerificationImplement();
            PaymentVerificationManager.Init(implement);
            PaymentVerificationManager.onVerificationResultCallBack += OnVerificationResultCallBack;
            LoginGameController.OnUserLogin += OnUserLogin;
            GlobalEvent.AddTypeEvent<CheckPayLimitResultEvent>(OnCheckPayLimitResult);
        }


        public static void Init(List<LocalizedGoodsInfo> productDefinitions)
        {
            Debug.Log("�̵��ʼ����" + JsonSerializer.ToJson(productDefinitions));
            // ��ʼ��֧��ƾ����֤������
            StorePayController.productDefinitions = productDefinitions;
            Debug.Log("֧���̵��ʼ��");
            SDKManager.ExtraInit(SDKType.Pay, null, JsonSerializer.ToJson(productDefinitions));
            //PayReSend.Instance.ReSendPay();
            OnSelectPayPlatformCallBack += OnOnSelectPayPlatform;
            SDKManager.GoodsInfoCallBack += OnGoodsInfoCallBack;
        }

        private static void OnVerificationResultCallBack(PayResult result)
        {
            Debug.Log("֧�����أ�" + result.code);
            if (OnPayCallBack != null)
            {
                OnPayCallBack(result);
            }
        }

        private static void OnUserLogin(UserLogin2Client t)
        {
            if (t.code != 0)
                return;

            user = t.user;
        }

        public static LocalizedGoodsInfo GetGoodsInfo(string goodID)
        {
            LocalizedGoodsInfo info = null;
            if (info == null)
            {
                foreach (var item in productDefinitions)
                {
                    if (item.goodsID == goodID)
                    {
                        info = item;
                        break;
                    }
                }
            }
            return info;
        }
        public static void Pay(string goodID)
        {
            if (user == null)
            {
                Debug.LogError("δ��¼������֧����");
                if (OnPayCallBack != null)
                {
                    OnPayCallBack(new PayResult(ErrorCodeDefine.StroePay_NoLogin, goodID, "No login!"));
                }
                return;
            }
            LocalizedGoodsInfo info = SDKManager.GetGoodsInfo(goodID);

            SelectPayPlatform(info);
        }

        // ѡ��֧����ʽ
        private static void SelectPayPlatform(LocalizedGoodsInfo goodsInfo)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
            {
                List<PayPlatformInfo> allPayPlatformInfos = SDKManager.GetAllPayPlatformInfos();
                // ��֧����ʽ- ���� 
                if (allPayPlatformInfos.Count == 0)
                {
                    OnVerificationResultCallBack(new PayResult(-9, goodsInfo.goodsID, "No Pay Platform"));
                    Debug.LogError("SelectPayPlatform error: no Pay Platform ->" + SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_StoreName, "--"));
                }
                else if (allPayPlatformInfos.Count == 1)
                {
                    //��һ֧����ʽ��ֱ�ӵ���
                    OnOnSelectPayPlatform(goodsInfo, allPayPlatformInfos[0]);
                }
                else
                {
                    //����֧����ʽ���ɷ��¼�
                    if (NeedSelectPayPlatformCallBack != null)
                    {
                        NeedSelectPayPlatformCallBack(goodsInfo, allPayPlatformInfos);
                    }
                    else
                    {
                        Debug.LogError("����� StorePayController.NeedSelectPayPlatformCallBack , ���ڻص�ʱ��ѡ��֧����ʽ�Ľ��档 ���ѡ��֧����ʽ�� �ٵ���StorePayController.OnSelectPayPlatformCallBack ֪ͨ���");
                        Debug.LogError("Ϊ�˲���ס���̣� ��ʱĬ�ϵ��õ�һ��֧����ʽ");
                        if (OnSelectPayPlatformCallBack != null)
                        {
                            OnSelectPayPlatformCallBack(goodsInfo, allPayPlatformInfos[0]);
                        }
                        else
                        {
                            OnVerificationResultCallBack(new PayResult(-11, goodsInfo.goodsID, "Pay Platform CallBack Null"));
                            Debug.LogError("OnSelectPayPlatformCallBack error: null");
                        }
                    }
                }
            }
            else
            {
                // ios����ʱû��ѡ��֧����ʽ ��һ����
                OnOnSelectPayPlatform(goodsInfo, new PayPlatformInfo());
            }
        }

        // ѡ��֧��ƽ̨��ϣ��ж�ʵ��������
        private static void OnOnSelectPayPlatform(LocalizedGoodsInfo goodsInfo, PayPlatformInfo payPlatform)
        {
            if (payPlatform == null) // ����֧��
            {
                OnVerificationResultCallBack(new PayResult(-10, goodsInfo.goodsID, "No Select Pay Platform"));
                return;
            }
            m_goodsInfo = goodsInfo;
            m_payPlatform = payPlatform;
            int price_cent = (int)(goodsInfo.localizedPrice * 100);
            Debug.Log("OnOnSelectPayPlatform SDK�� " + payPlatform.SDKName + " tag:" + payPlatform.payPlatformTag);
            // ʵ���������ж�  ��OnCheckPayLimitResult  �ص������
            RealNameManager.GetInstance().CheckPayLimit(price_cent);
        }

        /// ��SDK��ȡ��Ʒ��Ϣ�Ļص�
        private static void OnGoodsInfoCallBack(GoodsInfoFromSDK info)
        {
            for (int i = 0; i < productDefinitions.Count; i++)
            {
                if (productDefinitions[i].goodsID == info.goodsId)
                {
                    Debug.LogWarning("GetGoodsInfoFromSDK =====id:" + info.goodsId + "=====price:" + info.localizedPriceString);
                    productDefinitions[i].localizedPriceString = info.localizedPriceString;
                    return;
                }
            }
        }

        // �ж�֧�� ���ƵĻص�
        private static void OnCheckPayLimitResult(CheckPayLimitResultEvent e, object[] args)
        {
            Debug.Log("OnCheckPayLimitResult SDK�� " + e.payLimitType);
            if (e.payLimitType == PayLimitType.None)
            {
                PayInfo payInfo = new PayInfo(m_goodsInfo.goodsID, m_goodsInfo.localizedTitle, m_payPlatform.payPlatformTag, 
                    GoodsType.NORMAL, "", m_goodsInfo.localizedPrice, m_goodsInfo.isoCurrencyCode, user.userID, m_payPlatform.SDKName);
                if (Application.platform == RuntimePlatform.Android)
                {
                    SDKManager.Pay(m_payPlatform.SDKName, payInfo);
                }
                else
                {
                    SDKManager.Pay(payInfo);
                }
            }
            else if (e.payLimitType == PayLimitType.ChildLimit)
            {
                // δ���걾�����ѳ���
                OnVerificationResultCallBack(new PayResult(-21, m_goodsInfo.goodsID, "���������ѳ���δ��������"));
            }
            else if (e.payLimitType == PayLimitType.NoRealName)
            {
                // δʵ���ƣ��޷�֧��
                OnVerificationResultCallBack(new PayResult(-22, m_goodsInfo.goodsID, "�����ʵ������֤������"));
            }
            else
            {
                // ���󣬲�Ӧ�û����
            }
        }
    }
}