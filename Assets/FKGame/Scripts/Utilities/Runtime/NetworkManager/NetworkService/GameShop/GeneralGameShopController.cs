using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ͨ���̵�
    public static class GeneralGameShopController
    {
        private static Dictionary<string, GameShopInfoData> allShopInfos = new Dictionary<string, GameShopInfoData>();
        private static Dictionary<string, List<string>> shopTableStringDatas = new Dictionary<string, List<string>>();
        private static Dictionary<string, object> shopTableObjectDatas = new Dictionary<string, object>();
        public static System.Action<GameShopInfoData> OnGeneralShopInfoUpdate;             // �̵���Ϣ����
        public static System.Action<GeneralShopBuyGoods2Client> OnPlayerBuyGoodsResult;    // ������Ʒ����
        public static System.Action OnGeneralShopTableDataUpdate;                          // ���±�����ʱ��

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            GlobalEvent.AddTypeEvent<UpdateGeneralShopInfo2Client>(OnUpdateGeneralShopInfo);
            GlobalEvent.AddTypeEvent<GeneralShopBuyGoods2Client>(OnGeneralShopBuyGoods);
            GlobalEvent.AddTypeEvent<GeneralShopTableData2Client>(OnGeneralShopTableData);
            GlobalEvent.AddTypeEvent<GeneralShopTableDataComplete2Client>(OnGeneralShopTableDataComplete);
        }

        private static void OnGeneralShopTableData(GeneralShopTableData2Client e, object[] args)
        {
            List<string> listCom = new List<string>();
            foreach (var item in e.content)
            {
                byte[] bys = Convert.FromBase64String(item);
                string ss = Encoding.UTF8.GetString(bys);
                listCom.Add(ss);
            }
            if (shopTableStringDatas.ContainsKey(e.classType))
            {
                shopTableStringDatas[e.classType] = listCom;
            }
            else
            {
                shopTableStringDatas.Add(e.classType, listCom);
            }
            if (shopTableObjectDatas.ContainsKey(e.classType))
                shopTableObjectDatas.Remove(e.classType);
        }

        private static void OnGeneralShopTableDataComplete(GeneralShopTableDataComplete2Client e, object[] args)
        {
            if (OnGeneralShopTableDataUpdate != null)
            {
                OnGeneralShopTableDataUpdate();
            }
        }

        private static void OnGeneralShopBuyGoods(GeneralShopBuyGoods2Client e, object[] args)
        {
            if (OnPlayerBuyGoodsResult != null)
            {
                OnPlayerBuyGoodsResult(e);
            }
            if (e.code != ErrorCodeDefine.Success)
            {
                Debug.LogError(e.shopType + "�̵깺��ʧ��! GoodID:" + e.goodsID + " code:" + e.code);
            }
        }

        private static void OnUpdateGeneralShopInfo(UpdateGeneralShopInfo2Client e, object[] args)
        {
            if (allShopInfos.ContainsKey(e.shopInfo.shopType))
            {
                allShopInfos[e.shopInfo.shopType] = e.shopInfo;
            }
            else
            {
                allShopInfos.Add(e.shopInfo.shopType, e.shopInfo);
            }

            if (OnGeneralShopInfoUpdate != null)
            {
                OnGeneralShopInfoUpdate(e.shopInfo);
            }
        }

        // ����
        /// <param name="shopType">�̵�����</param>
        /// <param name="goodsID">��ƷID</param>
        /// <param name="buyNum">��������</param>
        public static void Buy(string shopType, string goodsID, int buyNum = 1)
        {
            GeneralShopBuyGoods2Server msg = new GeneralShopBuyGoods2Server();
            msg.shopType = shopType;
            msg.goodsID = goodsID;
            msg.buyNum = buyNum;
            JsonMessageProcessingController.SendMessage(msg);
        }

        // ����̵���Ϣ
        public static GameShopInfoData GetShopInfo(string shopType)
        {
            if (allShopInfos.ContainsKey(shopType))
            {
                return allShopInfos[shopType];
            }
            return null;
        }

        // ��ȡ���紫�������̵�����
        public static List<T> GetShopTableData<T>() where T : IDataGenerateBase
        {
            string className = typeof(T).Name;
            if (shopTableObjectDatas.ContainsKey(className))
            {
                return (List<T>)shopTableObjectDatas[className];
            }
            else
            {
                List<T> listData = null;
                if (shopTableStringDatas.ContainsKey(className))
                {
                    listData = new List<T>();
                    foreach (var item in shopTableStringDatas[className])
                    {
                        T t = JsonSerializer.FromJson<T>(item);
                        listData.Add(t);
                    }
                    shopTableObjectDatas.Add(className, listData);
                }
                return listData;
            }
        }
    }
}