namespace FKGame
{
    // ºÏ≤‚÷ß∏∂œﬁ÷∆
    public class CheckPayLimitEvent
    {
        public int payAmount;

        public CheckPayLimitEvent(int payAmount)
        {
            this.payAmount = payAmount;
        }

        static public void Dispatch(int l_payAmount)
        {
            GlobalEvent.DispatchTypeEvent<CheckPayLimitEvent>(new CheckPayLimitEvent(l_payAmount));
        }
    }
}