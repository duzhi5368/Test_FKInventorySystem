using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��ǰ��Ϸ��������֤�ľ���ʵ��
    public class NetworkVerificationImplement : PaymentVerificationInterface
    {
        // ���з��ͷ������Ķ������� key = receipt
        static Dictionary<string, StoreBuyGoods2Server> goodsPayInfo = new Dictionary<string, StoreBuyGoods2Server>();

        public void CheckRecipe(OnPayInfo info)
        {
            StoreBuyGoods2Server msg = new StoreBuyGoods2Server();
            msg.storeName = info.storeName;
            msg.receipt = info.receipt;
            msg.id = info.goodsId;
            SetBuyResendMessage(msg, false);
            Debug.LogWarning(info.storeName);
            Debug.Log(" ��ǰ��Ϸ��������֤");
        }

        public void Init()
        {
            ResendMessageManager.ReceiveMsgCallBack += OnReceiveMsgCallBack;
        }

        private void OnReceiveMsgCallBack(IMessageClass resMsg)
        {
            StoreBuyGoods2Client e = (StoreBuyGoods2Client)resMsg;
            StoreName storeName = GetGoodsPayInfo(e.receipt).storeName;
            Debug.LogWarning("NetworkVerificationImplement   StoreBuyGoods2Client=========" + e.id + " storeName:" + storeName);
            PaymentVerificationManager.OnVerificationResult(e.code, e.id, e.repeatReceipt, e.receipt, null, storeName);
        }

        public static void SetBuyResendMessage(StoreBuyGoods2Server msg, bool noSend)
        {
            SaveGoodsPayInfo(msg.receipt, msg);
            ResendMessageManager.AddResendMessage(msg, typeof(StoreBuyGoods2Client).Name, noSend);
        }

        // �洢���͵���Ϣ
        static private void SaveGoodsPayInfo(string receipt, StoreBuyGoods2Server msg)
        {
            if (goodsPayInfo.ContainsKey(receipt))
            {
                Debug.LogError("Repeat GoodsPayInfo:" + receipt);
            }
            else
            {
                goodsPayInfo.Add(receipt, msg);
            }
        }

        // ��ѯ֮ǰ����Ķ�����Ϣ
        static private StoreBuyGoods2Server GetGoodsPayInfo(string receipt)
        {
            if (goodsPayInfo.ContainsKey(receipt))
            {
                return goodsPayInfo[receipt];
            }
            else
            {
                Debug.LogError("No Found GoodsPayInfo:" + receipt);
                return new StoreBuyGoods2Server();
            }
        }
    }
}