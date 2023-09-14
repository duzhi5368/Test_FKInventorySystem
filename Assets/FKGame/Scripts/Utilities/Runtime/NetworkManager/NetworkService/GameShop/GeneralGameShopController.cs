using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 通用商店
    public static class GeneralGameShopController
    {
        private static Dictionary<string, GameShopInfoData> allShopInfos = new Dictionary<string, GameShopInfoData>();
        private static Dictionary<string, List<string>> shopTableStringDatas = new Dictionary<string, List<string>>();
        private static Dictionary<string, object> shopTableObjectDatas = new Dictionary<string, object>();
        public static System.Action<GameShopInfoData> OnGeneralShopInfoUpdate;             // 商店信息更新
        public static System.Action<GeneralShopBuyGoods2Client> OnPlayerBuyGoodsResult;    // 购买商品返回
        public static System.Action OnGeneralShopTableDataUpdate;                          // 更新表格完成时间

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
                Debug.LogError(e.shopType + "商店购买失败! GoodID:" + e.goodsID + " code:" + e.code);
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

        // 购买
        /// <param name="shopType">商店类型</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="buyNum">购买数量</param>
        public static void Buy(string shopType, string goodsID, int buyNum = 1)
        {
            GeneralShopBuyGoods2Server msg = new GeneralShopBuyGoods2Server();
            msg.shopType = shopType;
            msg.goodsID = goodsID;
            msg.buyNum = buyNum;
            JsonMessageProcessingController.SendMessage(msg);
        }

        // 获得商店信息
        public static GameShopInfoData GetShopInfo(string shopType)
        {
            if (allShopInfos.ContainsKey(shopType))
            {
                return allShopInfos[shopType];
            }
            return null;
        }

        // 获取网络传过来的商店表的类
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