using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{

    public class PublicPayClass : PayInterface
    {
        PayInfo payInfo;
        GameObject androidListener;
        StoreName storeName = StoreName.None;
        string userID;
        bool payResponse = false;       // �Ƿ�õ�������֧����Ӧ

        public override List<RuntimePlatform> GetPlatform()
        {
            return new List<RuntimePlatform>() { RuntimePlatform.Android, RuntimePlatform.WindowsEditor };
        }

        public override StoreName GetStoreName()
        {
            return storeName;
        }

        public override void Init()
        {
            m_SDKName = "PublicPayClass";
            GlobalEvent.AddTypeEvent<PrePay2Client>(OnPrePay);
            GlobalEvent.DispatchTypeEvent<InputUIOnClickEvent>(null);
            GlobalEvent.AddTypeEvent<InputUIOnClickEvent>((e, objs) => {});
            SDKManager.GoodsInfoCallBack += OnGoodsInfoCallBack;
            StorePayController.OnPayCallBack += OnPayResultCallBack;
        }

        // ��SDK��ȡ��Ʒ��Ϣ�Ļص�
        private void OnGoodsInfoCallBack(GoodsInfoFromSDK info)
        {
            for (int i = 0; i < productDefinitions.Count; i++)
            {
                if (productDefinitions[i].goodsID == info.goodsId)
                {
                    productDefinitions[i].localizedPriceString = info.localizedPriceString;
                    return;
                }
            }
        }

        public override void ExtraInit(string tag)
        {
            base.ExtraInit(tag);

            //��ȡsdk ��Ʒ��Ϣ
            for (int i = 0; i < productDefinitions.Count; i++)
            {
                string goodsID = productDefinitions[i].goodsID;
                Debug.LogWarning("==============��sdk ��ȡ��Ʒ��Ϣ��" + goodsID);
                SDKManagerNew.GetGoodsInfoFromSDK(null, goodsID);
            }
        }

        // ���Ԥ֧������
        private void OnPrePay(PrePay2Client e, object[] args)
        {
            Debug.LogWarning("OnPrePay=======partnerId==��" + e.prepay_id + "===" + e.storeName.ToString()); 
            // �ж��Ƿ���Ҫ�ط�֧��
            if (SDKManager.GetReSendPay(e.storeName.ToString()))
            {
                OnPayInfo onPayInfo = new OnPayInfo();
                onPayInfo.isSuccess = true;
                onPayInfo.goodsId = e.goodsID;
                onPayInfo.storeName = e.storeName;
                onPayInfo.receipt = e.mch_orderID;
                onPayInfo.price = payInfo.price;
                //PayReSend.Instance.AddPrePayID(onPayInfo);
            }
            if (SDKManager.GetPrePay(e.storeName.ToString()))
            {
                payInfo.orderID = e.mch_orderID;
                payInfo.prepay_id = e.prepay_id;
                SDKManagerNew.Pay(payInfo);
                StartLongTimeNoResponse();
            }
        }

        // ͳһ֧�����
        public override void Pay(PayInfo payInfo)
        {
            userID = payInfo.userID;
            Debug.Log("send publicPay message storeName" + payInfo.storeName + " goodsID " + payInfo.goodsID);
            // ����������Ԥ֧����Ϣ
            PrePay2Service.SendPrePayMsg((StoreName)Enum.Parse(typeof(StoreName), payInfo.storeName), payInfo.goodsID);
        }

        public override LocalizedGoodsInfo GetGoodsInfo(string goodsID)
        {
            return base.GetGoodsInfo(goodsID);
        }

        // ȷ��֧���ɹ�
        public override void ConfirmPay(string goodsID, string mch_orderID, string SDKName)
        {
            //PayReSend.Instance.ClearPrePayID(mch_orderID);
            Debug.Log("ConfirmPay  : " + goodsID);
            // ����sdk��¼
            SDKManagerNew.ClearPurchaseBySDK(SDKName, goodsID, mch_orderID);
        }

        public override string GetUserID()
        {
            return userID;
        }

        // ��ʱ��δ��Ӧ
        private void StartLongTimeNoResponse()
        {
            payResponse = false;
            Debug.LogWarning("======StartLongTimeNoResponse=====  start  ===" + Time.timeSinceLevelLoad);
            TimerManager.DelayCallBack(5, (o) =>
            {
                Debug.LogWarning("======StartLongTimeNoResponse=====  end  ===" + payResponse + "=============" + Time.timeSinceLevelLoad);
                if (!payResponse)
                {
                    PayCallBack(new OnPayInfo(payInfo, false, (StoreName)Enum.Parse(typeof(StoreName), payInfo.storeName)));
                }
            });
        }

        //���������ص�
        private void OnPayResultCallBack(PayResult result)
        {
            payResponse = true;
        }
    }
}