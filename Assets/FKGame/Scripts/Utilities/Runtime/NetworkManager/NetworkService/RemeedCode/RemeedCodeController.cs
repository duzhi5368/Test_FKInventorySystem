using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // π‹¿Ì∂“ªª¬Î
    public static class RemeedCodeController
    {
        public static CallBack<RemeedCodeUse2Client> OnUserRemeedCode;

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            GlobalEvent.AddTypeEvent<RemeedCodeUse2Client>(OnRemeedCodeUse);
        }

        private static void OnRemeedCodeUse(RemeedCodeUse2Client e, object[] args)
        {
            if (OnUserRemeedCode != null)
                OnUserRemeedCode(e);
        }

        public static void UseRemeedCode(string code)
        {
            RemeedCodeUse2Server msg = new RemeedCodeUse2Server();
            msg.code = code;
            JsonMessageProcessingController.SendMessage(msg);
        }
    }
}