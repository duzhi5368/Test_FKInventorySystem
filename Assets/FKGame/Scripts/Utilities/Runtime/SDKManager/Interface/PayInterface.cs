using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public abstract class PayInterface : SDKInterfaceBase
    {
        protected List<LocalizedGoodsInfo> productDefinitions = new List<LocalizedGoodsInfo>();

        public virtual StoreName GetStoreName()
        {
            return StoreName.None;
        }

        public override void ExtraInit(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                productDefinitions = JsonSerializer.FromJson<List<LocalizedGoodsInfo>>(tag);
            }
            ExtraInit();
        }

        protected virtual void ExtraInit(){}

        virtual public void Pay(PayInfo payInfo){}

        // 适用于多种store的方式
        virtual public void ConfirmPay(string goodsID, string tag, string StoreName){}

        virtual public LocalizedGoodsInfo GetGoodsInfo(string goodsID)
        {
            for (int i = 0; i < productDefinitions.Count; i++)
            {
                if (productDefinitions[i].goodsID == goodsID)
                {
                    return productDefinitions[i];
                }
            }
            return null;
        }

        virtual public string GetUserID()
        {
            return "userID";
        }

        virtual public List<LocalizedGoodsInfo> GetAllGoodsInfo()
        {
            return productDefinitions;
        }

        public override void Init()
        {
            m_SDKName = GetStoreName().ToString();
        }

        protected void PayCallBack(OnPayInfo info)
        {
            if (SDKManager.PayCallBack != null)
                SDKManager.PayCallBack(info);
        }

        // 获取商品类型
        public GoodsType GetGoodType(string goodID)
        {
            for (int i = 0; i < productDefinitions.Count; i++)
            {
                if (productDefinitions[i].goodsID == goodID)
                {
                    return productDefinitions[i].goodsType;
                }
            }
            Debug.LogError(" pay productDefinitions goodID is not found" + "id: " + goodID + " count: " + productDefinitions.Count);
            return GoodsType.NORMAL;
        }
    }
}