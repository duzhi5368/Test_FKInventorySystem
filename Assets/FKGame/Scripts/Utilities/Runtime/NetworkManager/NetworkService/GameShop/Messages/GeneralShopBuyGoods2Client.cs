using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // ���򷵻���Ϣ
    public class GeneralShopBuyGoods2Client : CodeMessageBase
    {
        public String shopType;     // �̵�Type
        public String goodsID;      // ��ƷID
        public int buyNum;          // ��������

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}