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
        bool payResponse = false;       // 是否得到订单的支付响应

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

        // 从SDK获取商品信息的回调
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

            //获取sdk 商品信息
            for (int i = 0; i < productDefinitions.Count; i++)
            {
                string goodsID = productDefinitions[i].goodsID;
                Debug.LogWarning("==============从sdk 获取商品信息：" + goodsID);
                SDKManagerNew.GetGoodsInfoFromSDK(null, goodsID);
            }
        }

        // 获得预支付订单
        private void OnPrePay(PrePay2Client e, object[] args)
        {
            Debug.LogWarning("OnPrePay=======partnerId==：" + e.prepay_id + "===" + e.storeName.ToString()); 
            // 判断是否需要重发支付
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

        // 统一支付入口
        public override void Pay(PayInfo payInfo)
        {
            userID = payInfo.userID;
            Debug.Log("send publicPay message storeName" + payInfo.storeName + " goodsID " + payInfo.goodsID);
            // 给服务器发预支付消息
            PrePay2Service.SendPrePayMsg((StoreName)Enum.Parse(typeof(StoreName), payInfo.storeName), payInfo.goodsID);
        }

        public override LocalizedGoodsInfo GetGoodsInfo(string goodsID)
        {
            return base.GetGoodsInfo(goodsID);
        }

        // 确认支付成功
        public override void ConfirmPay(string goodsID, string mch_orderID, string SDKName)
        {
            //PayReSend.Instance.ClearPrePayID(mch_orderID);
            Debug.Log("ConfirmPay  : " + goodsID);
            // 擦除sdk记录
            SDKManagerNew.ClearPurchaseBySDK(SDKName, goodsID, mch_orderID);
        }

        public override string GetUserID()
        {
            return userID;
        }

        // 长时间未响应
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

        //正常订单回调
        private void OnPayResultCallBack(PayResult result)
        {
            payResponse = true;
        }
    }
}