using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SimplePlayerLoginHandler : IPlayerLoginHandlerBase
    {
        public override uint LoginLogic(Login2Server msg, Session session, out Player player)
        {
            RemoteConsoleSettingData config = RemoteConsoleSettingData.GetConfig();

            string key = msg.key;
            string pw = msg.password;


            if (config.loginKey.Equals(key) && config.loginPassword.Equals(pw))
            {
                player = new Player(session);
                player.playerID = Guid.NewGuid().ToString();

                return 0;
            }
            player = null;
            return 102;
        }
    }
}