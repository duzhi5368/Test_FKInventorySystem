namespace FKGame
{
    // ���֧������
    public class CheckPayLimitResultEvent
    {
        public int payAmount;
        public PayLimitType payLimitType = PayLimitType.None;   // ���ƹ����磺 ��������δ�������ƣ�

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