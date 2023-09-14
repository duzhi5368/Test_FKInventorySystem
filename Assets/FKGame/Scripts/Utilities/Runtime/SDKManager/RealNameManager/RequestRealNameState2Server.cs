namespace FKGame
{
    public class RequestRealNameState2Server
    {
        public RealNameStatus realNameStatus = RealNameStatus.NotNeed;      // ÊµÃûÖÆ×´Ì¬
        public bool isAdult = true;

        public RequestRealNameState2Server(RealNameStatus realNameStatus, bool isAdult)
        {
            this.realNameStatus = realNameStatus;
            this.isAdult = isAdult;
        }

        public static void RequestRealName(RealNameStatus l_realNameStatus, bool l_isAdult)
        {
            JsonMessageProcessingController.SendMessage(new RequestRealNameState2Server(l_realNameStatus, l_isAdult));
        }
    }
}