using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ClientReportClass : LogInterface
    {
        public override void Log(string eventID, Dictionary<string, string> data)
        {
            ClientReport2Server msg = new ClientReport2Server();
            msg.eventName = eventID;
            msg.datas = KeyValueData.Dictionary2KeyValueDataList(data);
            JsonMessageProcessingController.SendMessage(msg);
        }
    }
}