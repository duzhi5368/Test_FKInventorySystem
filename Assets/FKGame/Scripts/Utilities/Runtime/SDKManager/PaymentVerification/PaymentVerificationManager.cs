using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ����֧����֤
    public class PaymentVerificationManager
    {
        public static CallBack<PayResult> onVerificationResultCallBack;
        private static PaymentVerificationInterface verificationInterface;

        public static void Init(PaymentVerificationInterface verificationInterface)
        {
            PaymentVerificationManager.verificationInterface = verificationInterface;
            verificationInterface.Init();
            SDKManager.PayCallBack += PayCallBack;
        }

        private static void PayCallBack(OnPayInfo info)
        {
            if (info.isSuccess)
            {
                verificationInterface.CheckRecipe(info);
            }
            else
            {
                Debug.Log("PaymentVerificationManager info.goodsId " + info.goodsId);
                int code = info.isSuccess ? 0 : -1;
                OnVerificationResult(code, info.goodsId, false, info.receipt, info.error, info.storeName);
            }
        }
        /// <summary>
        /// ��֤�������
        /// </summary>
        /// <param name="code">�Ƿ�ɹ�</param>
        /// <param name="goodsID">��ƷID</param>
        /// <param name="repeatReceipt">�Ƿ����ظ��Ķ���ƾ��</param>
        /// <param name="receipt">��ִ���̻������ŵ�</param>
        public static void OnVerificationResult(int code, string goodsID, bool repeatReceipt, string receipt, string error, StoreName storeName)
        {
            try
            {
                if (onVerificationResultCallBack != null)
                {
                    PayResult result = new PayResult(code, goodsID, error, storeName);
                    Debug.Log("��֤�ص� code " + code + " goodsID " + goodsID);
                    onVerificationResultCallBack(result);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            if (code == 0 || code == ErrorCodeDefine.StorePay_RepeatReceipt || repeatReceipt)
            {
                Debug.Log("����ȷ��" + goodsID);
                SDKManager.ConfirmPay(storeName.ToString(), goodsID, receipt);
            }
            // ��֤�ɹ�
            if (code != 0)
            {
                Debug.LogError("ƾ����֤ʧ�ܣ� goodID:" + goodsID);
            }
        }
    }
}