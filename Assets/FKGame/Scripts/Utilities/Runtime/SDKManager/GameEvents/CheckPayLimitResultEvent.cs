namespace FKGame
{
    // 检测支付限制
    public class CheckPayLimitResultEvent
    {
        public int payAmount;
        public PayLimitType payLimitType = PayLimitType.None;   // 限制购买（如： 本单超出未成年限制）

        public CheckPayLimitResultEvent(int payAmount, PayLimitType l_payLimitType)
        {
            this.payAmount = payAmount;
            payLimitType = l_payLimitType;
        }

        static public void Dispatch(int l_payAmount, PayLimitType l_payLimitType)
        {
            GlobalEvent.DispatchTypeEvent<CheckPayLimitResultEvent>(new CheckPayLimitResultEvent(l_payAmount, l_payLimitType));
        }
    }
}