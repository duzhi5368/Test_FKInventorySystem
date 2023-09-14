using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GeneralShopTableData2Client : IMessageClass
    {
        public string classType;
        public List<string> content;

        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}