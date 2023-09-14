namespace FKGame
{
    public abstract class IPlayerLoginHandlerBase
    {
        public abstract uint LoginLogic(Login2Server msg, Session session, out Player player);
    }
}