using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RequsetAreadyBindPlatform2Client : IMessageClass
    {
        public List<LoginPlatform> areadyBindPlatforms = new List<LoginPlatform>();
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}